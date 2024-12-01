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
    public partial class AddNhaTroForm : Form
    {
        public bool IsEditMode { get; set; } = false; // Để biết đây là thêm hay sửa
        public NhaTro NhaTro { get; set; } // Dữ liệu nhà trọ cần chỉnh sửa
        public AddNhaTroForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ nhà trọ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (IsEditMode && NhaTro != null) // Nếu đang ở chế độ sửa
                {
                    // Cập nhật thông tin nhà trọ
                    NhaTro.DiaChi = txtDiaChi.Text.Trim();

                    // Gọi service để cập nhật
                    NhaTroService.Update(NhaTro);

                    MessageBox.Show("Cập nhật nhà trọ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else // Nếu đang ở chế độ thêm
                {
                    // Tạo đối tượng nhà trọ mới
                    var nhaTro = new NhaTro
                    {
                        DiaChi = txtDiaChi.Text.Trim()
                    };

                    // Gọi service để thêm
                    NhaTroService.Add(nhaTro);

                    MessageBox.Show("Thêm nhà trọ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Đóng form và trả về kết quả OK
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu nhà trọ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Đóng form
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void AddNhaTroForm_Load(object sender, EventArgs e)
        {
            if (IsEditMode && NhaTro != null)
            {
                // Gán giá trị cho các trường khi ở chế độ sửa
                txtDiaChi.Text = NhaTro.DiaChi;
                this.Text = "Chỉnh Sửa Nhà Trọ";
            }
            else
            {
                this.Text = "Thêm Nhà Trọ";
            }
        }
    }
}
