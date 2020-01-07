namespace CinsDownloadManager
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_send = new System.Windows.Forms.Button();
            this.pb_upload = new System.Windows.Forms.ProgressBar();
            this.dlg_open_file = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(342, 269);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(233, 23);
            this.btn_send.TabIndex = 3;
            this.btn_send.Text = "Select and Download";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click_1);
            // 
            // pb_upload
            // 
            this.pb_upload.Location = new System.Drawing.Point(60, 142);
            this.pb_upload.Name = "pb_upload";
            this.pb_upload.Size = new System.Drawing.Size(435, 23);
            this.pb_upload.TabIndex = 4;
            // 
            // dlg_open_file
            // 
            this.dlg_open_file.FileName = "dlg_open_file";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(167, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Welcome To Cins Download Manager";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(151, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(316, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Please Click the button below and start download";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(167, 213);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(270, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Please change the save folder at line 179";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 327);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pb_upload);
            this.Controls.Add(this.btn_send);
            this.Name = "Form1";
            this.Text = "Cins Download Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.ProgressBar pb_upload;
        private System.Windows.Forms.OpenFileDialog dlg_open_file;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

