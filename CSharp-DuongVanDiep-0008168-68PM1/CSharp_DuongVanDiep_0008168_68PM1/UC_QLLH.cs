using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSharp_DuongVanDiep_0008168_68PM1
{
    public partial class UC_QLLH : UserControl
    {
        private SQLHelper db = new SQLHelper();
        private int selectedID = -1;

        public UC_QLLH()
        {
            InitializeComponent();
            this.Load += UC_QLLH_Load;
            button1.Click += btnAddClass_Click;
            button2.Click += btnUpdateClass_Click;
            button3.Click += btnDeleteClass_Click;
            button4.Click += btnRefreshClass_Click;
            button10.Click += btnSearchClass_Click;
            button5.Click += btnViewStudents_Click;  // Thêm sự kiện
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
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
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            label7.Text = "";
        }

        private void ConfigListView()
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Columns.Clear();
            listView1.Columns.Add("STT", 60, HorizontalAlignment.Center);
            listView1.Columns.Add("Mã Lớp", 130, HorizontalAlignment.Left);
            listView1.Columns.Add("Tên Lớp Học", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("Ghi Chú", 200, HorizontalAlignment.Left);
        }

        private void LoadClassList()
        {
            try
            {
                listView1.Items.Clear();
                string query = "SELECT ID, MaLop, TenLop, GhiChu FROM LopHoc";
                string searchCondition = "";
                string keyword = textBox5.Text.Trim();
                if (!string.IsNullOrEmpty(keyword))
                {
                    searchCondition = $" WHERE MaLop LIKE '%{keyword}%' OR TenLop LIKE '%{keyword}%' OR GhiChu LIKE '%{keyword}%'";
                }
                DataTable dt = db.ExecuteQuery(query + searchCondition + " ORDER BY ID");
                int stt = 1;
                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(stt.ToString());
                    item.SubItems.Add(row["MaLop"].ToString());
                    item.SubItems.Add(row["TenLop"].ToString());
                    item.SubItems.Add(row["GhiChu"] != DBNull.Value ? row["GhiChu"].ToString() : "");
                    item.Tag = row["ID"];
                    listView1.Items.Add(item);
                    stt++;
                }
                label7.Text = $"Tổng số lớp học: {dt.Rows.Count}";
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Columns[0].Width = 60;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị danh sách lớp học: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddClass_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã lớp!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                string query = @"INSERT INTO LopHoc (MaLop, TenLop, GhiChu) VALUES (@MaLop, @TenLop, @GhiChu)";
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
                    ClearInputs();
                    LoadClassList();
                }
                else
                {
                    MessageBox.Show("Không thể lưu thông tin lớp học.", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                MessageBox.Show("Mã lớp học này đã tồn tại!", "Lỗi trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateClass_Click(object sender, EventArgs e)
        {
            if (selectedID == -1)
            {
                MessageBox.Show("Vui lòng chọn lớp học cần sửa!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Mã lớp không được để trống!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Tên lớp không được để trống!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return;
            }

            try
            {
                string query = @"UPDATE LopHoc SET MaLop=@MaLop, TenLop=@TenLop, GhiChu=@GhiChu WHERE ID=@ID";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ID", selectedID),
                    new SqlParameter("@MaLop", textBox2.Text.Trim().ToUpper()),
                    new SqlParameter("@TenLop", textBox3.Text.Trim()),
                    new SqlParameter("@GhiChu", string.IsNullOrWhiteSpace(textBox4.Text) ? (object)DBNull.Value : textBox4.Text.Trim())
                };

                int rows = db.ExecuteNonQuery(query, parameters);
                if (rows > 0)
                {
                    MessageBox.Show("Cập nhật lớp học thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    LoadClassList();
                    selectedID = -1;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy lớp học để cập nhật.", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                MessageBox.Show("Mã lớp bị trùng với lớp khác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteClass_Click(object sender, EventArgs e)
        {
            if (selectedID == -1)
            {
                MessageBox.Show("Vui lòng chọn lớp học cần xóa!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string checkQuery = "SELECT COUNT(*) FROM SinhVien WHERE MaLop = @MaLop";
            SqlParameter[] checkParams = new SqlParameter[] { new SqlParameter("@MaLop", textBox2.Text.Trim()) };
            int count = Convert.ToInt32(db.ExecuteScalar(checkQuery, checkParams));
            if (count > 0)
            {
                MessageBox.Show($"Lớp này đang có {count} sinh viên. Không thể xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa lớp học này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM LopHoc WHERE ID=@ID";
                    SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@ID", selectedID) };
                    int rows = db.ExecuteNonQuery(query, parameters);
                    if (rows > 0)
                    {
                        MessageBox.Show("Xóa lớp học thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearInputs();
                        LoadClassList();
                        selectedID = -1;
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy lớp để xóa.", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefreshClass_Click(object sender, EventArgs e)
        {
            ClearInputs();
            textBox5.Clear();
            LoadClassList();
        }

        private void btnSearchClass_Click(object sender, EventArgs e)
        {
            LoadClassList();
        }

        private void btnViewStudents_Click(object sender, EventArgs e)
        {
            if (selectedID == -1)
            {
                MessageBox.Show("Vui lòng chọn lớp học để xem danh sách sinh viên.", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maLop = textBox2.Text.Trim();
            string tenLop = textBox3.Text.Trim();
            FrmStudentListByClass frm = new FrmStudentListByClass(maLop, tenLop);
            frm.ShowDialog();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                selectedID = Convert.ToInt32(item.Tag);
                textBox1.Text = selectedID.ToString();
                textBox2.Text = item.SubItems[1].Text;
                textBox3.Text = item.SubItems[2].Text;
                textBox4.Text = item.SubItems[3].Text;
            }
        }

        private void ClearInputs()
        {
            textBox1.Text = "";
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            selectedID = -1;
        }
    }
}