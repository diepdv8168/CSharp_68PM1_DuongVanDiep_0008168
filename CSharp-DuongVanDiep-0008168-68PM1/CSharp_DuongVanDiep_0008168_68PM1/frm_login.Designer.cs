namespace Login
{
    partial class frm_login
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtusername = new TextBox();
            txtpassword = new TextBox();
            btnLogin_Click = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // txtusername
            // 
            txtusername.Location = new Point(276, 147);
            txtusername.Name = "txtusername";
            txtusername.Size = new Size(253, 27);
            txtusername.TabIndex = 0;
            // 
            // txtpassword
            // 
            txtpassword.Location = new Point(276, 205);
            txtpassword.Name = "txtpassword";
            txtpassword.Size = new Size(253, 27);
            txtpassword.TabIndex = 1;
            txtpassword.UseSystemPasswordChar = true;
            // 
            // btnLogin_Click
            // 
            btnLogin_Click.Location = new Point(325, 254);
            btnLogin_Click.Name = "btnLogin_Click";
            btnLogin_Click.Size = new Size(155, 29);
            btnLogin_Click.TabIndex = 2;
            btnLogin_Click.Text = "Đăng Nhập";
            btnLogin_Click.UseVisualStyleBackColor = true;
            btnLogin_Click.Click += Login;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(274, 124);
            label1.Name = "label1";
            label1.Size = new Size(75, 20);
            label1.TabIndex = 3;
            label1.Text = "Username";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(276, 182);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 4;
            label2.Text = "Password";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(257, 45);
            label3.Name = "label3";
            label3.Size = new Size(295, 38);
            label3.TabIndex = 5;
            label3.Text = "Đăng Nhập Hệ Thống";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnLogin_Click);
            Controls.Add(txtpassword);
            Controls.Add(txtusername);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtusername;
        private TextBox txtpassword;
        private Button btnLogin_Click;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}
