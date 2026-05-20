namespace Login
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Login(object sender, EventArgs e)
        {
            string username=txtusername.Text;
            string password=txtpassword.Text;
            if(username== "dvdiep1234@gmail.com" &&  password== "0008168")
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo");
            }
            else
            {
                MessageBox.Show("Đăng nhập không thành công!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
