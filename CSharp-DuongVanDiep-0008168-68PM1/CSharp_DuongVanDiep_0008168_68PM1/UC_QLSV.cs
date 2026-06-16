using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSharp_DuongVanDiep_0008168_68PM1
{
    public partial class UC_QLSV : UserControl
    {
        private SQLHelper db = new SQLHelper();
        private int selectedID = -1;

        public UC_QLSV()
        {
            InitializeComponent();
            this.Load += UC_QLSV_Load;
            button1.Click += btnAddStudent_Click;
            button2.Click += btnUpdateStudent_Click;
            button3.Click += btnDeleteStudent_Click;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
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
            listView1.Columns.Add("STT", 60, HorizontalAlignment.Center);
            listView1.Columns.Add("Mã SV", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("Họ và Tên", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("Ngày Sinh", 130, HorizontalAlignment.Left);
            listView1.Columns.Add("Giới Tính", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Lớp Học", 220, HorizontalAlignment.Left);
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
                string query = @"SELECT sv.ID, sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh, lh.TenLop 
                                 FROM SinhVien sv 
                                 INNER JOIN LopHoc lh ON sv.MaLop = lh.MaLop
                                 ORDER BY sv.ID";
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
                    item.Tag = row["ID"];
                    listView1.Items.Add(item);
                    stt++;
                }
                label11.Text = $"Tổng số sinh viên: {dt.Rows.Count}";
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Columns[0].Width = 60;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị danh sách sinh viên: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    ClearInputs();
                    LoadStudentList();
                }
                else
                {
                    MessageBox.Show("Không thể thêm dữ liệu.", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                MessageBox.Show("Mã sinh viên này đã tồn tại trong hệ thống!", "Lỗi trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateStudent_Click(object sender, EventArgs e)
        {
            if (selectedID == -1)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần sửa!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Mã sinh viên không được để trống!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Họ tên không được để trống!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                string query = @"UPDATE SinhVien 
                                 SET MaSV = @MaSV, 
                                     HoTen = @HoTen, 
                                     NgaySinh = @NgaySinh, 
                                     GioiTinh = @GioiTinh, 
                                     MaLop = @MaLop
                                 WHERE ID = @ID";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ID", selectedID),
                    new SqlParameter("@MaSV", textBox1.Text.Trim()),
                    new SqlParameter("@HoTen", textBox2.Text.Trim()),
                    new SqlParameter("@NgaySinh", dateTimePicker1.Value.Date),
                    new SqlParameter("@GioiTinh", comboBox1.SelectedItem.ToString()),
                    new SqlParameter("@MaLop", comboBox2.SelectedValue.ToString())
                };

                int rows = db.ExecuteNonQuery(query, parameters);
                if (rows > 0)
                {
                    MessageBox.Show("Cập nhật sinh viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    LoadStudentList();
                    selectedID = -1;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên để cập nhật.", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                MessageBox.Show("Mã sinh viên mới bị trùng với sinh viên khác!", "Lỗi trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            if (selectedID == -1)
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM SinhVien WHERE ID = @ID";
                    SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@ID", selectedID) };
                    int rows = db.ExecuteNonQuery(query, parameters);
                    if (rows > 0)
                    {
                        MessageBox.Show("Xóa sinh viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearInputs();
                        LoadStudentList();
                        selectedID = -1;
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên để xóa.", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                selectedID = Convert.ToInt32(item.Tag);
                textBox1.Text = item.SubItems[1].Text;
                textBox2.Text = item.SubItems[2].Text;
                DateTime ngaySinh = DateTime.ParseExact(item.SubItems[3].Text, "dd/MM/yyyy", null);
                dateTimePicker1.Value = ngaySinh;
                comboBox1.SelectedItem = item.SubItems[4].Text;
                string tenLop = item.SubItems[5].Text;
                foreach (DataRowView drv in comboBox2.Items)
                {
                    if (drv["TenLop"].ToString() == tenLop)
                    {
                        comboBox2.SelectedValue = drv["MaLop"].ToString();
                        break;
                    }
                }
            }
        }

        private void ClearInputs()
        {
            textBox1.Clear();
            textBox2.Clear();
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.SelectedIndex = 0;
            if (comboBox2.Items.Count > 0) comboBox2.SelectedIndex = 0;
            selectedID = -1;
        }
    }
}