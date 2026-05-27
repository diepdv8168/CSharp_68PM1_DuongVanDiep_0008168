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
    public partial class UC_QLSV : UserControl
    {
        private SQLHelper db = new SQLHelper();
        public UC_QLSV()
        {
            InitializeComponent();
            this.Load += UC_QLSV_Load;
            button1.Click += btnAddStudent_Click;
        }

        private void UC_QLSV_Load(object sender, EventArgs e)
        {
            ConfigListView();    
            LoadGenderData();    
            LoadClassData();     
            LoadStudentList();   
        }

        
        private void ConfigListView()
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            listView1.Columns.Clear();
            listView1.Columns.Add("STT", 50, HorizontalAlignment.Center);
            listView1.Columns.Add("Mã SV", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Họ và Tên", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("Ngày Sinh", 110, HorizontalAlignment.Left);
            listView1.Columns.Add("Giới Tính", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("Lớp Học", 180, HorizontalAlignment.Left);
        }

        
        private void LoadGenderData()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Nam");
            comboBox1.Items.Add("Nữ");
            comboBox1.SelectedIndex = 0; 
        }

        
        private void LoadClassData()
        {
            try
            {
                string query = "SELECT MaLop, TenLop FROM LopHoc";
                DataTable dt = db.ExecuteQuery(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    comboBox2.DataSource = dt;
                    comboBox2.DisplayMember = "TenLop"; 
                    comboBox2.ValueMember = "MaLop";    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục lớp học: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void LoadStudentList()
        {
            try
            {
                listView1.Items.Clear();

                
                string query = @"SELECT sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh, lh.TenLop 
                                 FROM SinhVien sv 
                                 INNER JOIN LopHoc lh ON sv.MaLop = lh.MaLop";

                DataTable dt = db.ExecuteQuery(query);
                int stt = 1;

                foreach (DataRow row in dt.Rows)
                {
                    
                    DateTime ngaySinh = Convert.ToDateTime(row["NgaySinh"]);

                    ListViewItem item = new ListViewItem(stt.ToString());
                    item.SubItems.Add(row["MaSV"].ToString());
                    item.SubItems.Add(row["HoTen"].ToString());
                    item.SubItems.Add(ngaySinh.ToString("dd/MM/yyyy"));
                    item.SubItems.Add(row["GioiTinh"].ToString());
                    item.SubItems.Add(row["TenLop"].ToString());

                    listView1.Items.Add(item);
                    stt++;
                }

                
                label11.Text = $"Trang 1/1   |    {dt.Rows.Count} bản ghi";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị danh sách sinh viên: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            listView1.AutoResizeColumn(5, ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        
        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã sinh viên!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Vui lòng nhập Họ tên sinh viên!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }
            if (comboBox2.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Lớp học hợp lệ!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                
                string query = @"INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, MaLop) 
                                 VALUES (@MaSV, @HoTen, @NgaySinh, @GioiTinh, @MaLop)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MaSV", textBox1.Text.Trim()),
                    new SqlParameter("@HoTen", textBox2.Text.Trim()),
                    new SqlParameter("@NgaySinh", dateTimePicker1.Value.Date),
                    new SqlParameter("@GioiTinh", comboBox1.SelectedItem.ToString()),
                    new SqlParameter("@MaLop", comboBox2.SelectedValue.ToString())
                };

                int rowsAffected = db.ExecuteNonQuery(query, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Thêm mới sinh viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    
                    textBox1.Clear();
                    textBox2.Clear();
                    dateTimePicker1.Value = DateTime.Now;
                    comboBox1.SelectedIndex = 0;

                    
                    LoadStudentList();
                }
                else
                {
                    MessageBox.Show("Không thể thêm dữ liệu. Vui lòng kiểm tra lại cấu trúc kết nối.", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                
                if (ex.Number == 2627)
                {
                    MessageBox.Show("Mã sinh viên này đã tồn tại trong hệ thống!", "Lỗi trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Lỗi cơ sở dữ liệu: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra hệ thống: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
