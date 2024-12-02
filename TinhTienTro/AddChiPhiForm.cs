using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinhTienTro
{
    using System;
    using System.Data;
    using System.Data.SQLite;
    using System.Windows.Forms;

    public partial class AddChiPhiForm : Form
    {
        public AddChiPhiForm()
        {
            InitializeComponent();
            LoadChiPhiList();
            txtID.ReadOnly = true; // ID chỉ hiển thị, không cho chỉnh sửa
        }

        // Load danh sách chi phí từ DB vào DataGridView
        private void LoadChiPhiList()
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string sql = "SELECT ID, TenPhi, GiaTien FROM ChiPhi";
                var command = new SQLiteCommand(sql, connection);
                var reader = command.ExecuteReader();

                var dataTable = new DataTable();
                dataTable.Load(reader);

                dataGridViewChiPhi.DataSource = dataTable;
            }
        }

        // Thêm mới chi phí
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string tenPhi = txtTenPhi.Text.Trim();
            double giaTien = (double)nudGiaTien.Value;

            if (string.IsNullOrEmpty(tenPhi))
            {
                MessageBox.Show("Vui lòng nhập tên phí.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string sql = "INSERT INTO ChiPhi (TenPhi, GiaTien) VALUES (@TenPhi, @GiaTien)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TenPhi", tenPhi);
                    command.Parameters.AddWithValue("@GiaTien", giaTien);
                    command.ExecuteNonQuery();
                }
            }

            ClearForm();
            LoadChiPhiList();
        }

        // Sửa chi phí
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Vui lòng chọn một chi phí để sửa.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(txtID.Text);
            string tenPhi = txtTenPhi.Text.Trim();
            double giaTien = (double)nudGiaTien.Value;

            if (string.IsNullOrEmpty(tenPhi))
            {
                MessageBox.Show("Vui lòng nhập tên phí.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string sql = "UPDATE ChiPhi SET TenPhi = @TenPhi, GiaTien = @GiaTien WHERE ID = @ID";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@TenPhi", tenPhi);
                    command.Parameters.AddWithValue("@GiaTien", giaTien);
                    command.ExecuteNonQuery();
                }
            }

            ClearForm();
            LoadChiPhiList();
        }

        // Xóa chi phí
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Vui lòng chọn một chi phí để xóa.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(txtID.Text);

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa chi phí này?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string sql = "DELETE FROM ChiPhi WHERE ID = @ID";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ID", id);
                        command.ExecuteNonQuery();
                    }
                }

                ClearForm();
                LoadChiPhiList();
            }
        }

        // Khi chọn một dòng trên DataGridView, điền dữ liệu vào TextBox
        private void dataGridViewChiPhi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewChiPhi.Rows[e.RowIndex];
                txtID.Text = row.Cells["ID"].Value.ToString();
                txtTenPhi.Text = row.Cells["TenPhi"].Value.ToString();
                nudGiaTien.Value = Convert.ToDecimal(row.Cells["GiaTien"].Value);
            }
        }

        // Đóng form
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Xóa nội dung form
        private void ClearForm()
        {
            txtID.Clear();
            txtTenPhi.Clear();
            nudGiaTien.Value = 0;
        }
    }

}
