using CSharp_DuongVanDiep_0008168_68PM1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Login
{
    public partial class frm_login : Form
    {
        private const string CorrectEmail = "dvdiep1234@gmail.com";
        private const string CorrectMSSV = "0008168";
        public frm_login()
        {
            InitializeComponent();
        }

        private void Login(object sender, EventArgs e)
        {
            string email = txtusername.Text.Trim();
            string mssv = txtpassword.Text.Trim();

            if (email == CorrectEmail && mssv == CorrectMSSV)
            {
                MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frm_main form2 = new frm_main();
                form2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
