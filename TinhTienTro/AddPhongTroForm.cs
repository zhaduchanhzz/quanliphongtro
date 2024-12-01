using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace TinhTienTro
{
    public partial class AddPhongTroForm : Form
    {
        public bool IsEditMode { get; set; } = false; // Kiểm tra chế độ chỉnh sửa hay thêm mới
        public PhongTro PhongTro { get; set; } // Đối tượng phòng trọ cần chỉnh sửa
        public int NhaTroID { get; set; } // ID nhà trọ (không thay đổi khi chỉnh sửa phòng trọ)

        public AddPhongTroForm(int nhaTroID)
        {
            InitializeComponent();
            NhaTroID = nhaTroID;  // Gán ID nhà trọ cho form
        }

        private void AddPhongTroForm_Load(object sender, EventArgs e)
        {
            // Nếu là chế độ chỉnh sửa, điền tên phòng trọ vào textbox
            if (IsEditMode && PhongTro != null)
            {
                txtTenPhong.Text = PhongTro.TenPhong;
            }
            else
            {
                // Chế độ thêm mới: Tên phòng trọ chưa có, textbox trống
                txtTenPhong.Clear();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu có thông tin hợp lệ
            if (string.IsNullOrEmpty(txtTenPhong.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Tạo đối tượng PhongTro từ thông tin trên form
            PhongTro phongTro = new PhongTro
            {
                TenPhong = txtTenPhong.Text,
                NhaTroID = NhaTroID // Chỉ giữ NhaTroID khi chỉnh sửa, không thay đổi
            };

            // Nếu là chỉnh sửa, cập nhật thông tin
            if (IsEditMode)
            {
                phongTro.ID = PhongTro.ID; // Gán ID để biết đây là phòng trọ cần chỉnh sửa
                UpdatePhongTro(phongTro);
            }
            else
            {
                // Nếu là thêm mới, gọi phương thức thêm phòng trọ
                AddPhongTro(phongTro);
            }

            // Đóng form sau khi lưu
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void AddPhongTro(PhongTro phongTro)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO PhongTro (TenPhong, NhaTroID) VALUES (@TenPhong, @NhaTroID)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenPhong", phongTro.TenPhong);
                    command.Parameters.AddWithValue("@NhaTroID", phongTro.NhaTroID);

                    command.ExecuteNonQuery(); // Thực thi câu lệnh INSERT
                }
            }
        }

        private void UpdatePhongTro(PhongTro phongTro)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "UPDATE PhongTro SET TenPhong = @TenPhong WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenPhong", phongTro.TenPhong);
                    command.Parameters.AddWithValue("@ID", phongTro.ID);

                    command.ExecuteNonQuery(); // Thực thi câu lệnh UPDATE
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
