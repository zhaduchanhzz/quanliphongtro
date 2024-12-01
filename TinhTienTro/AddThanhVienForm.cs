using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Forms;
using TinhTienTro;

namespace TinhTienTro
{

public partial class AddThanhVienForm : Form
{
    private int? thanhVienID; // Nullable, dùng cho chế độ chỉnh sửa
    private int nhaTroID;    // ID của nhà trọ hiện tại

    public AddThanhVienForm(int nhaTroID, int? thanhVienID = null)
    {
        InitializeComponent();
        this.nhaTroID = nhaTroID;
        this.thanhVienID = thanhVienID;
    }

    private void AddThanhVienForm_Load(object sender, EventArgs e)
    {
        // Tải danh sách phòng trọ vào ComboBox
        LoadPhongTro();

        if (thanhVienID.HasValue)
        {
            // Nếu ở chế độ chỉnh sửa, tải thông tin thành viên
            LoadThanhVienInfo(thanhVienID.Value);
        }
    }

    private void LoadPhongTro()
    {
        using (var connection = DatabaseHelper.GetConnection())
        {
            connection.Open();
            string query = "SELECT ID, TenPhong FROM PhongTro WHERE NhaTroID = @NhaTroID";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@NhaTroID", nhaTroID);
                using (var reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    cbPhongTro.DataSource = dt;
                    cbPhongTro.DisplayMember = "TenPhong";
                    cbPhongTro.ValueMember = "ID";
                }
            }
        }
    }

    private void LoadThanhVienInfo(int thanhVienID)
    {
            using (var connection = DatabaseHelper.GetConnection())
            {
            connection.Open();
            string query = @"
                SELECT TenThanhVien, SDT, SinhNam, NauAn, PhongTroID
                FROM ThanhVien
                WHERE ID = @ID";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ID", thanhVienID);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtTenThanhVien.Text = reader["TenThanhVien"].ToString();
                        txtSDT.Text = reader["SDT"].ToString();
                        txtSinhNam.Text = reader["SinhNam"].ToString();
                        chkNauAn.Checked = Convert.ToBoolean(reader["NauAn"]);
                        cbPhongTro.SelectedValue = Convert.ToInt32(reader["PhongTroID"]);
                    }
                }
            }
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        // Thu thập thông tin từ form
        string tenThanhVien = txtTenThanhVien.Text.Trim();
        string sdt = txtSDT.Text.Trim();
        string sinhNam = txtSinhNam.Text.Trim();
        bool nauAn = chkNauAn.Checked;
        int phongTroID = Convert.ToInt32(cbPhongTro.SelectedValue);

        if (string.IsNullOrEmpty(tenThanhVien))
        {
            MessageBox.Show("Vui lòng nhập tên thành viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

            using (var connection = DatabaseHelper.GetConnection())
            {
            connection.Open();

            if (thanhVienID.HasValue)
            {
                // Chế độ chỉnh sửa
                string updateQuery = @"
                    UPDATE ThanhVien
                    SET TenThanhVien = @TenThanhVien, 
                        SDT = @SDT, 
                        SinhNam = @SinhNam, 
                        NauAn = @NauAn, 
                        PhongTroID = @PhongTroID
                    WHERE ID = @ID";
                using (var command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@TenThanhVien", tenThanhVien);
                    command.Parameters.AddWithValue("@SDT", sdt);
                    command.Parameters.AddWithValue("@SinhNam", sinhNam);
                    command.Parameters.AddWithValue("@NauAn", nauAn);
                    command.Parameters.AddWithValue("@PhongTroID", phongTroID);
                    command.Parameters.AddWithValue("@ID", thanhVienID.Value);
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                // Chế độ thêm mới
                string insertQuery = @"
                    INSERT INTO ThanhVien (TenThanhVien, SDT, SinhNam, NauAn, PhongTroID)
                    VALUES (@TenThanhVien, @SDT, @SinhNam, @NauAn, @PhongTroID)";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@TenThanhVien", tenThanhVien);
                    command.Parameters.AddWithValue("@SDT", sdt);
                    command.Parameters.AddWithValue("@SinhNam", sinhNam);
                    command.Parameters.AddWithValue("@NauAn", nauAn);
                    command.Parameters.AddWithValue("@PhongTroID", phongTroID);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Thông báo thành công và đóng form
        MessageBox.Show("Lưu thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
}
