using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace TinhTienTro
{
    public partial class AddPhongTroForm : Form
    {
        public int NhaTroID { get; set; } // ID nhà trọ để lọc danh sách phòng trọ

        public AddPhongTroForm(int nhaTroID)
        {
            InitializeComponent();
            NhaTroID = nhaTroID; // Gán ID nhà trọ
        }

        private void AddPhongTroForm_Load(object sender, EventArgs e)
        {
            LoadPhongTroList(); // Hiển thị danh sách phòng trọ
        }

        // Phương thức Load danh sách phòng trọ vào DataGridView
        private void LoadPhongTroList()
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT ID, TenPhong, GiaPhong FROM PhongTro WHERE NhaTroID = @NhaTroID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NhaTroID", NhaTroID);
                    var reader = command.ExecuteReader();

                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridViewPhongTro.DataSource = dataTable;
                }
            }
        }

        // Nút thêm phòng trọ
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string tenPhong = txtTenPhong.Text.Trim();
            if (string.IsNullOrEmpty(tenPhong))
            {
                MessageBox.Show("Vui lòng nhập tên phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double giaPhong = 0;
            if (!double.TryParse(txtGiaPhong.Text, out giaPhong))
            {
                MessageBox.Show("Giá phòng không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO PhongTro (TenPhong, NhaTroID, GiaPhong) VALUES (@TenPhong, @NhaTroID, @GiaPhong)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenPhong", tenPhong);
                    command.Parameters.AddWithValue("@NhaTroID", NhaTroID);
                    command.Parameters.AddWithValue("@GiaPhong", giaPhong);
                    command.ExecuteNonQuery();
                }
            }

            ClearForm();
            LoadPhongTroList();
        }

        // Nút sửa phòng trọ
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewPhongTro.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một phòng trọ để chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenPhong = txtTenPhong.Text.Trim();
            if (string.IsNullOrEmpty(tenPhong))
            {
                MessageBox.Show("Vui lòng nhập tên phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double giaPhong = 0;
            if (!double.TryParse(txtGiaPhong.Text, out giaPhong))
            {
                MessageBox.Show("Giá phòng không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int phongTroID = Convert.ToInt32(dataGridViewPhongTro.SelectedRows[0].Cells["ID"].Value);

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "UPDATE PhongTro SET TenPhong = @TenPhong, GiaPhong = @GiaPhong WHERE ID = @ID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenPhong", tenPhong);
                    command.Parameters.AddWithValue("@GiaPhong", giaPhong);
                    command.Parameters.AddWithValue("@ID", phongTroID);
                    command.ExecuteNonQuery();
                }
            }

            ClearForm();
            LoadPhongTroList();
        }

        // Nút xóa phòng trọ
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewPhongTro.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một phòng trọ để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int phongTroID = Convert.ToInt32(dataGridViewPhongTro.SelectedRows[0].Cells["ID"].Value);

            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa phòng trọ này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM PhongTro WHERE ID = @ID";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", phongTroID);
                        command.ExecuteNonQuery();
                    }
                }

                ClearForm();
                LoadPhongTroList();
            }
        }

        // Khi chọn dòng trên DataGridView, điền dữ liệu vào TextBox
        private void dataGridViewPhongTro_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewPhongTro.Rows[e.RowIndex];
                txtTenPhong.Text = row.Cells["TenPhong"].Value.ToString();
                txtGiaPhong.Text = row.Cells["GiaPhong"].Value.ToString();
            }
        }

        // Nút đóng form
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Xóa nội dung form
        private void ClearForm()
        {
            txtTenPhong.Clear();
            txtGiaPhong.Clear();
        }
    }
}
