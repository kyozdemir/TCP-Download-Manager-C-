using System;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace CinsDownloadManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_start_server_Click(object sender, EventArgs e)
        {
            TCPServer obj_server = new TCPServer();
            System.Threading.Thread obj_thread = new System.Threading.Thread(obj_server.StartServer);
            obj_thread.Start();
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            
        }
        public byte[] ReadStream(NetworkStream ns)// calculates chunk number
        {
            byte[] data_buff = null;

            int b = 0;
            string buff_length = "";
            while ((b = ns.ReadByte()) != 4)
            {
                buff_length += (char)b;
            }
            int data_length = Convert.ToInt32(buff_length);
            data_buff = new byte[data_length];
            int byte_read = 0;
            int byte_offset = 0;
            while (byte_offset < data_length)
            {
                byte_read = ns.Read(data_buff, byte_offset, data_length - byte_offset);
                byte_offset += byte_read;
            }

            return data_buff;
        }
        private byte[] CreateDataPacket(byte[] cmd, byte[] data) //creating small chunks
        {
            byte[] initialize = new byte[1];
            initialize[0] = 2;
            byte[] separator = new byte[1];
            separator[0] = 4;
            byte[] datalength = Encoding.UTF8.GetBytes(Convert.ToString(data.Length));
            MemoryStream ms = new MemoryStream();
            ms.Write(initialize, 0, initialize.Length);
            ms.Write(cmd, 0, cmd.Length);
            ms.Write(datalength, 0, datalength.Length);
            ms.Write(separator, 0, separator.Length);
            ms.Write(data, 0, data.Length);
            return ms.ToArray();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TCPServer obj_server = new TCPServer();
            System.Threading.Thread obj_thread = new System.Threading.Thread(obj_server.StartServer);
            obj_thread.Start();
        }

        private void btn_send_Click_1(object sender, EventArgs e)
        {
            if (dlg_open_file.ShowDialog() == DialogResult.OK)
            {
                string selected_file = dlg_open_file.FileName;
                string file_name = Path.GetFileName(selected_file);
                FileStream fs = new FileStream(selected_file, FileMode.Open);
                TcpClient tc = new TcpClient("127.0.0.1", 5555);
                NetworkStream ns = tc.GetStream();
                byte[] data_tosend = CreateDataPacket(Encoding.UTF8.GetBytes("125"), Encoding.UTF8.GetBytes(file_name));
                ns.Write(data_tosend, 0, data_tosend.Length);
                ns.Flush();
                bool loop_break = false;
                while (true)
                {
                    if (ns.ReadByte() == 2)
                    {
                        byte[] cmd_buff = new byte[3];
                        ns.Read(cmd_buff, 0, cmd_buff.Length);
                        byte[] recv_data = ReadStream(ns);
                        switch (Convert.ToInt32(Encoding.UTF8.GetString(cmd_buff)))
                        {
                            case 126:
                                long recv_file_pointer = long.Parse(Encoding.UTF8.GetString(recv_data));
                                if (recv_file_pointer != fs.Length)
                                {
                                    fs.Seek(recv_file_pointer, SeekOrigin.Begin);
                                    int temp_buff_length = (int)(fs.Length - recv_file_pointer < 20000 ? fs.Length - recv_file_pointer : 20000);
                                    byte[] temp_buff = new byte[temp_buff_length];
                                    fs.Read(temp_buff, 0, temp_buff.Length);
                                    byte[] data_to_send = CreateDataPacket(Encoding.UTF8.GetBytes("127"), temp_buff);
                                    ns.Write(data_to_send, 0, data_to_send.Length);
                                    ns.Flush();
                                    pb_upload.Value = (int)Math.Ceiling((double)recv_file_pointer / (double)fs.Length * 100);
                                }
                                else
                                {
                                    byte[] data_to_send = CreateDataPacket(Encoding.UTF8.GetBytes("128"), Encoding.UTF8.GetBytes("Close"));
                                    ns.Write(data_to_send, 0, data_to_send.Length);
                                    ns.Flush();
                                    fs.Close();
                                    loop_break = true;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    if (loop_break == true)
                    {
                        ns.Close();
                        break;
                    }
                }
            }
        }

        
    }

    class TCPServer
    {
        TcpListener obj_server;
        public TCPServer()
        {
            obj_server = new TcpListener(IPAddress.Any, 5555);
        }

        public void StartServer()
        {
            obj_server.Start();
            while (true)
            {
                TcpClient tc = obj_server.AcceptTcpClient();
                SocketHandler obj_handler = new SocketHandler(tc);
                System.Threading.Thread obj_thread = new System.Threading.Thread(obj_handler.ProcessSocketRequest);
                obj_thread.Start();
            }
        }

        class SocketHandler
        {
            NetworkStream ns;
            public SocketHandler(TcpClient tc)
            {
                ns = tc.GetStream();
            }

            public void ProcessSocketRequest()
            {
                FileStream fs = null;
                long current_file_pointer = 0;
                bool loop_break = false;
                while (true)
                {
                    if (ns.ReadByte() == 2)
                    {
                        byte[] cmd_buff = new byte[3];
                        ns.Read(cmd_buff, 0, cmd_buff.Length);
                        byte[] recv_data = ReadStream();
                        switch (Convert.ToInt32(Encoding.UTF8.GetString(cmd_buff)))
                        {
                            case 125:
                                {
                                    fs = new FileStream(@"D:\İndirmeler" + Encoding.UTF8.GetString(recv_data), FileMode.CreateNew); // don't forget to change the save folder!!!
                                    byte[] data_to_send = CreateDataPacket(Encoding.UTF8.GetBytes("126"), Encoding.UTF8.GetBytes(Convert.ToString(current_file_pointer)));
                                    ns.Write(data_to_send, 0, data_to_send.Length);
                                    ns.Flush();
                                }
                                break;
                            case 127:
                                {
                                    fs.Seek(current_file_pointer, SeekOrigin.Begin);
                                    fs.Write(recv_data, 0, recv_data.Length);
                                    current_file_pointer = fs.Position;
                                    byte[] data_to_send = CreateDataPacket(Encoding.UTF8.GetBytes("126"), Encoding.UTF8.GetBytes(Convert.ToString(current_file_pointer)));
                                    ns.Write(data_to_send, 0, data_to_send.Length);
                                    ns.Flush();
                                }
                                break;
                            case 128:
                                {
                                    fs.Close();
                                    loop_break = true;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    if (loop_break == true)
                    {
                        ns.Close();
                        break;
                    }
                }
            }

            public byte[] ReadStream()
            {
                byte[] data_buff = null;

                int b = 0;
                string buff_length = "";
                while ((b = ns.ReadByte()) != 4)
                {
                    buff_length += (char)b;
                }
                int data_length = Convert.ToInt32(buff_length);
                data_buff = new byte[data_length];
                int byte_read = 0;
                int byte_offset = 0;
                while (byte_offset < data_length)
                {
                    byte_read = ns.Read(data_buff, byte_offset, data_length - byte_offset);
                    byte_offset += byte_read;
                }

                return data_buff;
            }

            private byte[] CreateDataPacket(byte[] cmd, byte[] data)
            {
                byte[] initialize = new byte[1];
                initialize[0] = 2;
                byte[] separator = new byte[1];
                separator[0] = 4;
                byte[] datalength = Encoding.UTF8.GetBytes(Convert.ToString(data.Length));
                MemoryStream ms = new MemoryStream();
                ms.Write(initialize, 0, initialize.Length);
                ms.Write(cmd, 0, cmd.Length);
                ms.Write(datalength, 0, datalength.Length);
                ms.Write(separator, 0, separator.Length);
                ms.Write(data, 0, data.Length);
                return ms.ToArray();
            }
        }

    }
}