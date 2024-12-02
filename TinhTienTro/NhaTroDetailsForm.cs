using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
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
            LoadCongToDienWithPhongTro(NhaTroID);
            dataGridViewThanhVien.CellClick += dataGridViewThanhVien_CellClick;
            dataGridViewCongto.CellClick += dataGridViewCongto_CellClick;


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
                dataGridViewThanhVien.Columns["SinhNam"].Width = 65;
                dataGridViewThanhVien.Columns["NauAn"].HeaderText = "Nấu Ăn";
                dataGridViewThanhVien.Columns["NauAn"].Width = 50;
                dataGridViewThanhVien.Columns["TenPhong"].HeaderText = "Tên Phòng";
                dataGridViewThanhVien.Columns["TenPhong"].Width = 100;

                // Tùy chọn nếu muốn ẩn cột ID (nếu không cần hiển thị)
                dataGridViewThanhVien.Columns["ThanhVienID"].Visible = false;
                AddActionButtonsToGrid(dataGridViewThanhVien); // Thêm cột hành động

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
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadThanhVienData(NhaTroID);
                    LoadCongToDienWithPhongTro(NhaTroID);

                }
            }

        }

        private void btnAddCongTo_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddCongToDienForm(NhaTroID))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {


                }
            }
            LoadCongToDienWithPhongTro(NhaTroID);
        }
        private void LoadCongToDienWithPhongTro(int nhaTroID)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                // Truy vấn để lấy thông tin công tơ và tên phòng trọ liên kết
                var sql = @"SELECT 
    ct.ID, 
    ct.ChiSoCu, 
    ct.ChiSoMoi, 
    ct.isTong, 
    ct.isNau, 
    GROUP_CONCAT(pt.TenPhong) AS TenPhong
FROM CongToDien ct
LEFT JOIN CongToPhongTro ctp ON ct.ID = ctp.CongToDienID
LEFT JOIN PhongTro pt ON ctp.PhongTroID = pt.ID
WHERE ct.NhaTroID = @NhaTroID
GROUP BY ct.ID, ct.ChiSoCu, ct.ChiSoMoi, ct.isTong, ct.isNau
ORDER BY ct.ID;";

                var command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@NhaTroID", nhaTroID); // Thêm tham số cho NhaTroID

                var reader = command.ExecuteReader();

                var listCongTo = new List<CongToDienWithPhongTro>();

                while (reader.Read())
                {
                    listCongTo.Add(new CongToDienWithPhongTro
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        ChiSoCu = Convert.ToDouble(reader["ChiSoCu"]),
                        ChiSoMoi = Convert.ToDouble(reader["ChiSoMoi"]),
                        IsTong = Convert.ToBoolean(reader["isTong"]),
                        IsNau = Convert.ToBoolean(reader["isNau"]),
                        TenPhong = reader["TenPhong"].ToString() // Lưu tên phòng vào
                    });
                }

                // Gán dữ liệu vào DataGridView
                dataGridViewCongto.DataSource = new BindingList<CongToDienWithPhongTro>(listCongTo);
                AddActionButtonsToGrid(dataGridViewCongto); // Thêm cột hành động

            }
        }

        private void dataGridViewThanhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataGridViewThanhVien.Rows[e.RowIndex];

                // Kiểm tra nếu cột là nút Sửa
                if (dataGridViewThanhVien.Columns[e.ColumnIndex].Name == "EditButton")
                {
                    int thanhVienID = Convert.ToInt32(selectedRow.Cells["ThanhVienID"].Value);
                    OpenEditThanhVienForm(thanhVienID);
                }

                // Kiểm tra nếu cột là nút Xóa
                if (dataGridViewThanhVien.Columns[e.ColumnIndex].Name == "DeleteButton")
                {
                    int thanhVienID = Convert.ToInt32(selectedRow.Cells["ThanhVienID"].Value);
                    DeleteThanhVien(thanhVienID);
                }
            }
        }

        private void dataGridViewCongto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dataGridViewCongto.Rows[e.RowIndex];

                // Kiểm tra nếu cột là nút Sửa
                if (dataGridViewCongto.Columns[e.ColumnIndex].Name == "EditButton")
                {
                    int congToID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                    OpenEditCongToForm(congToID);
                }

                // Kiểm tra nếu cột là nút Xóa
                if (dataGridViewCongto.Columns[e.ColumnIndex].Name == "DeleteButton")
                {
                    int congToID = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                    DeleteCongTo(congToID);
                }
            }
        }
        private void OpenEditThanhVienForm(int thanhVienID)
        {
            using (var editForm = new AddThanhVienForm(NhaTroID , thanhVienID))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadThanhVienData(NhaTroID); // Tải lại danh sách
                }
            }
        }
        private void OpenEditCongToForm(int congToID)
        {
            using (var editForm = new AddCongToDienForm(NhaTroID,congToID))
            {

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadCongToDienWithPhongTro(NhaTroID); // Tải lại danh sách
                }
            }
        }

        private void DeleteThanhVien(int thanhVienID)
        {
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa thành viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM ThanhVien WHERE ID = @ID";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", thanhVienID);
                        command.ExecuteNonQuery();
                    }
                }

                LoadThanhVienData(NhaTroID); // Tải lại danh sách
            }
        }

        private void DeleteCongTo(int congToID)
        {
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa công tơ này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM CongToDien WHERE ID = @ID";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", congToID);
                        command.ExecuteNonQuery();
                    }
                }

                LoadCongToDienWithPhongTro(NhaTroID); // Tải lại danh sách
            }
        }

        private void AddActionButtonsToGrid(DataGridView grid)
        {
            // Thêm nút Sửa
            if (!grid.Columns.Contains("EditButton"))
            {
                var editButton = new DataGridViewButtonColumn
                {
                    Name = "EditButton",
                    HeaderText = "Sửa",
                    Text = "Sửa",
                    UseColumnTextForButtonValue = true
                };
                editButton.Width = 50;
                grid.Columns.Add(editButton);
            }

            // Thêm nút Xóa
            if (!grid.Columns.Contains("DeleteButton"))
            {
                var deleteButton = new DataGridViewButtonColumn
                {
                    Name = "DeleteButton",
                    HeaderText = "Xóa",
                    Text = "Xóa",
                    UseColumnTextForButtonValue = true
                };
                deleteButton.Width = 50;
                grid.Columns.Add(deleteButton);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new AddChiPhiForm();
            form.ShowDialog();
        }
    }

}
