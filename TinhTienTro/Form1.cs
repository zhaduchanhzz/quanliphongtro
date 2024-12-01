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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadNhaTro();   

        }

        private void LoadNhaTro()
        {
            try
            {
                // Lấy danh sách nhà trọ từ service
                var nhaTroList = NhaTroService.GetAll();

                // Gán dữ liệu vào DataGridView
                dgvNhaTro.DataSource = null; // Xóa dữ liệu cũ
                dgvNhaTro.DataSource = nhaTroList;

                // Tùy chỉnh cột hiển thị
                dgvNhaTro.Columns["ID"].HeaderText = "Mã Nhà Trọ";
                dgvNhaTro.Columns["DiaChi"].HeaderText = "Địa Chỉ";

                // Thêm các nút sửa và xóa
                AddActionButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddActionButtons()
        {
            // Kiểm tra nếu chưa thêm nút "Sửa"
            if (dgvNhaTro.Columns["EditButton"] == null)
            {
                // Tạo cột nút "Sửa"
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn
                {
                    HeaderText = "Hành Động",
                    Name = "EditButton",
                    Text = "Sửa",
                    UseColumnTextForButtonValue = true,
                    Width = 60
                };
                dgvNhaTro.Columns.Add(btnEdit);
            }

            // Kiểm tra nếu chưa thêm nút "Xóa"
            if (dgvNhaTro.Columns["DeleteButton"] == null)
            {
                // Tạo cột nút "Xóa"
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn
                {
                    HeaderText = "",
                    Name = "DeleteButton",
                    Text = "Xóa",
                    UseColumnTextForButtonValue = true,
                    Width = 60
                };
                dgvNhaTro.Columns.Add(btnDelete);
            }

            // Thêm cột "Xem"
            if (dgvNhaTro.Columns["ViewButton"] == null)
            {
                DataGridViewButtonColumn btnView = new DataGridViewButtonColumn
                {
                    HeaderText = "",
                    Name = "ViewButton",
                    Text = "Xem",
                    UseColumnTextForButtonValue = true,
                    Width = 60
                };
                dgvNhaTro.Columns.Add(btnView);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            LoadNhaTro();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddNhaTroForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadNhaTro();
                }
            }
        }

        private void dgvNhaTro_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu dòng bị click là hợp lệ (không phải header)
            if (e.RowIndex >= 0)
            {
                // Kiểm tra cột "Sửa"
                if (dgvNhaTro.Columns[e.ColumnIndex].Name == "EditButton")
                {
                    // Lấy ID của nhà trọ được chọn
                    int nhaTroID = (int)dgvNhaTro.Rows[e.RowIndex].Cells["ID"].Value;

                    // Hiển thị form sửa nhà trọ hoặc thực hiện logic sửa
                    EditNhaTro(nhaTroID);
                }

                // Kiểm tra cột "Xóa"
                if (dgvNhaTro.Columns[e.ColumnIndex].Name == "DeleteButton")
                {
                    // Lấy ID của nhà trọ được chọn
                    int nhaTroID = (int)dgvNhaTro.Rows[e.RowIndex].Cells["ID"].Value;

                    // Hiển thị xác nhận xóa
                    var confirm = MessageBox.Show(
                        "Bạn có chắc chắn muốn xóa nhà trọ này?",
                        "Xác nhận xóa",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (confirm == DialogResult.Yes)
                    {
                        // Thực hiện xóa
                        DeleteNhaTro(nhaTroID);

                        // Tải lại dữ liệu sau khi xóa
                        LoadNhaTro();
                    }
                }

                // Kiểm tra cột "Xem"
                if (dgvNhaTro.Columns[e.ColumnIndex].Name == "ViewButton")
                {
                    // Lấy ID của nhà trọ được chọn
                    int nhaTroID = (int)dgvNhaTro.Rows[e.RowIndex].Cells["ID"].Value;

                    // Hiển thị thông tin chi tiết của nhà trọ
                    ShowNhaTroDetails(nhaTroID);
                }
            }
        }
        private void ShowNhaTroDetails(int nhaTroID)
        {
            using (var detailsForm = new NhaTroDetailsForm(nhaTroID))
            {
                detailsForm.ShowDialog();
            }
        }

        private void EditNhaTro(int nhaTroID)
        {
            var nhaTro = NhaTroService.GetById(nhaTroID); // Lấy thông tin nhà trọ
            if (nhaTro != null)
            {
                // Sử dụng AddNhaTroForm làm form chỉnh sửa
                using (var editForm = new AddNhaTroForm())
                {
                    editForm.IsEditMode = true; // Chuyển sang chế độ chỉnh sửa
                    editForm.NhaTro = nhaTro; // Gán dữ liệu nhà trọ cần sửa

                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // Tải lại danh sách nhà trọ sau khi chỉnh sửa
                        LoadNhaTro();
                    }
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhà trọ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteNhaTro(int nhaTroID)
        {
            try
            {
                NhaTroService.Delete(nhaTroID);
                MessageBox.Show("Xóa nhà trọ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa nhà trọ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
