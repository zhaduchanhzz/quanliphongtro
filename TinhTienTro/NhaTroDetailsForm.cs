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
    public partial class NhaTroDetailsForm : Form
    {
        public int NhaTroID { get; set; } // Thuộc tính lưu ID nhà trọ

        public NhaTroDetailsForm(int nhaTroID)
        {
            InitializeComponent();
            NhaTroID = nhaTroID;
        }

        private void NhaTroDetailsForm_Load(object sender, EventArgs e)
        {
            LoadThanhVienData(NhaTroID);
        }
        private void LoadData()
        {
            var nhaTro = NhaTroService.GetByIdWithDetails(NhaTroID);

            if (nhaTro != null)
            {
                // Hiển thị địa chỉ nhà trọ
                lblDiaChi.Text = $"Địa chỉ: {nhaTro.DiaChi}";

                // Tạo danh sách thành viên với tên phòng và các thông tin cần hiển thị
                var thanhVienList = nhaTro.PhongTro
                    .SelectMany(p => p.ThanhVien.Select(t => new
                    {
                        TenPhong = p.TenPhong,
                        TenThanhVien = t.TenThanhVien,
                        SDT = t.SDT,
                        SinhNam = t.SinhNam,
                        NauAn = t.nauan // Thêm cột Nấu ăn
                    }))
                    .ToList();

                // Đặt danh sách thành viên làm nguồn dữ liệu cho DataGridView
                dataGridViewThanhVien.DataSource = thanhVienList;

                // Tùy chỉnh DataGridView sau khi gán DataSource
                CustomizeDataGridView();
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin nhà trọ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void LoadThanhVienData(int nhaTroID)
        {
            try
            {
                NhaTro nhaTro = NhaTroService.GetById(NhaTroID);
                lblDiaChi.Text = $"Địa chỉ: {nhaTro.DiaChi}";
                // Lấy dữ liệu thành viên theo NhaTroID
                DataTable dataTable = NhaTroService.GetThanhVienByNhaTroID(nhaTroID);

                // Gán dữ liệu vào DataGridView
                dataGridViewThanhVien.DataSource = dataTable;

                // Tùy chỉnh cột hiển thị
                dataGridViewThanhVien.Columns["ThanhVienID"].HeaderText = "Mã Thành Viên";
                dataGridViewThanhVien.Columns["TenThanhVien"].HeaderText = "Tên Thành Viên";
                dataGridViewThanhVien.Columns["SDT"].HeaderText = "Số Điện Thoại";
                dataGridViewThanhVien.Columns["SinhNam"].HeaderText = "Năm Sinh";
                dataGridViewThanhVien.Columns["NauAn"].HeaderText = "Nấu Ăn";
                dataGridViewThanhVien.Columns["TenPhong"].HeaderText = "Tên Phòng";

                // Tùy chọn nếu muốn ẩn cột ID (nếu không cần hiển thị)
                dataGridViewThanhVien.Columns["ThanhVienID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CustomizeDataGridView()
        {
            // Tùy chỉnh tiêu đề cột
            dataGridViewThanhVien.Columns["TenPhong"].HeaderText = "Tên Phòng";
            dataGridViewThanhVien.Columns["TenThanhVien"].HeaderText = "Tên Thành Viên";
            dataGridViewThanhVien.Columns["SDT"].HeaderText = "Số Điện Thoại";
            dataGridViewThanhVien.Columns["SinhNam"].HeaderText = "Năm Sinh";
            dataGridViewThanhVien.Columns["NauAn"].HeaderText = "Nấu Ăn";

            // Thay đổi cột "Nấu ăn" thành CheckboxColumn
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "NauAn";
            checkBoxColumn.HeaderText = "Nấu Ăn";
            checkBoxColumn.DataPropertyName = "NauAn"; // Liên kết với thuộc tính "NauAn" trong danh sách dữ liệu

            // Xóa cột "NauAn" cũ, nếu có
            if (dataGridViewThanhVien.Columns.Contains("NauAn"))
            {
                dataGridViewThanhVien.Columns.Remove("NauAn");
            }

            // Thêm cột Checkbox vào DataGridView
            dataGridViewThanhVien.Columns.Add(checkBoxColumn);

            // Đảm bảo dữ liệu của cột "Nấu ăn" là Boolean (true/false)
            foreach (DataGridViewRow row in dataGridViewThanhVien.Rows)
            {
                bool nauAn = Convert.ToBoolean(row.Cells["NauAn"].Value);
                row.Cells["NauAn"].Value = nauAn;
            }
        }

        private void AddThanhVien_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddThanhVienForm(NhaTroID))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadThanhVienData(NhaTroID);
                }
            }
        }

        private void addPhongTro_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddPhongTroForm(NhaTroID))
            {
                addForm.IsEditMode = false;  // Chế độ thêm mới
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadThanhVienData(NhaTroID);
                }
            }

        }
    }

}
