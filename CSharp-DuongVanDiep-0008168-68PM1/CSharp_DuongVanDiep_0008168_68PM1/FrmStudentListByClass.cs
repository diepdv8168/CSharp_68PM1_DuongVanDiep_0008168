using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSharp_DuongVanDiep_0008168_68PM1
{
    public partial class FrmStudentListByClass : Form
    {
        private string maLop;
        private string tenLop;
        private SQLHelper db = new SQLHelper();

        public FrmStudentListByClass(string maLop, string tenLop)
        {
            InitializeComponent();
            this.maLop = maLop;
            this.tenLop = tenLop;
            this.Text = $"Danh sách sinh viên lớp {tenLop}";
            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                string query = "SELECT MaSV, HoTen, NgaySinh, GioiTinh FROM SinhVien WHERE MaLop = @MaLop";
                SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@MaLop", maLop) };
                DataTable dt = db.ExecuteQuery(query, parameters);
                dataGridView1.DataSource = dt;
                lblTotal.Text = $"Tổng số sinh viên: {dt.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}