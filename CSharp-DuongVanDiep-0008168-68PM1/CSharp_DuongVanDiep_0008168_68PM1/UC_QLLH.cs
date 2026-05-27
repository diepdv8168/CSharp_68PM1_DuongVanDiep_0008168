using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSharp_DuongVanDiep_0008168_68PM1
{
    public partial class UC_QLLH : UserControl
    {
        private SQLHelper db = new SQLHelper();

        public UC_QLLH()
        {
            InitializeComponent();


            this.Load += UC_QLLH_Load;


            button1.Click += btnAddClass_Click;
        }


        private void UC_QLLH_Load(object sender, EventArgs e)
        {
            ConfigListView();
            SetupUI();
            LoadClassList();
        }


        private void SetupUI()
        {

            textBox1.ReadOnly = true;
            textBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            textBox1.Text = "";
        }


        private void ConfigListView()
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            listView1.Columns.Clear();
            listView1.Columns.Add("STT", 50, HorizontalAlignment.Center);
            listView1.Columns.Add("Mã ID", 70, HorizontalAlignment.Center);
            listView1.Columns.Add("Mã Lớp", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("Tên Lớp Học", 180, HorizontalAlignment.Left);
            listView1.Columns.Add("Ghi Chú", 150, HorizontalAlignment.Left);
        }


        private void LoadClassList()
        {
            try
            {
                listView1.Items.Clear();

                string query = "SELECT ID, MaLop, TenLop, GhiChu FROM LopHoc";
                DataTable dt = db.ExecuteQuery(query);
                int stt = 1;

                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(stt.ToString());
                    item.SubItems.Add(row["ID"].ToString());
                    item.SubItems.Add(row["MaLop"].ToString());
                    item.SubItems.Add(row["TenLop"].ToString());
                    item.SubItems.Add(row["GhiChu"] != DBNull.Value ? row["GhiChu"].ToString() : "");

                    listView1.Items.Add(item);
                    stt++;
                }


                label7.Text = $"Trang 1/1   |    {dt.Rows.Count} bản ghi";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị danh sách lớp học: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            listView1.AutoResizeColumn(5, ColumnHeaderAutoResizeStyle.ColumnContent);
        }


        private void btnAddClass_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã lớp (Ví dụ: 68PM1)!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên lớp học!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return;
            }

            try
            {

                string query = @"INSERT INTO LopHoc (MaLop, TenLop, GhiChu) 
                                 VALUES (@MaLop, @TenLop, @GhiChu)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaLop", textBox2.Text.Trim().ToUpper()),
                    new SqlParameter("@TenLop", textBox3.Text.Trim()),
                    new SqlParameter("@GhiChu", string.IsNullOrWhiteSpace(textBox4.Text) ? (object)DBNull.Value : textBox4.Text.Trim())
                };

                int rowsAffected = db.ExecuteNonQuery(query, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Thêm mới lớp học thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();


                    LoadClassList();
                }
                else
                {
                    MessageBox.Show("Không thể lưu thông tin lớp học. Vui lòng kiểm tra lại.", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {

                if (ex.Number == 2627)
                {
                    MessageBox.Show("Mã lớp học này đã tồn tại trên hệ thống, không thể tạo trùng!", "Lỗi trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message, "Lỗi SQL Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra hệ thống: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
