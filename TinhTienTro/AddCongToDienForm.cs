using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
namespace TinhTienTro
{
    public partial class AddCongToDienForm : Form
    {
        private int nhaTroID; // ID nhà trọ hiện tại
        private int? congToDienID; // ID công tơ (null nếu thêm mới)
        private List<PhongTro> danhSachPhongTro; // Danh sách phòng trọ

        public AddCongToDienForm(int nhaTroID, int? congToDienID = null)
        {
            InitializeComponent();
            this.nhaTroID = nhaTroID;
            this.congToDienID = congToDienID;

            // Load dữ liệu lên form
            LoadPhongTro();
            if (congToDienID.HasValue)
            {
                LoadCongToDienDetails();
            }
        }

        private void LoadPhongTro()
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var sql = @"SELECT * FROM PhongTro WHERE NhaTroID = @NhaTroID";
                danhSachPhongTro = connection.Query<PhongTro>(sql, new { NhaTroID = nhaTroID }).ToList();
            }

            foreach (var phong in danhSachPhongTro)
            {
                CheckBox checkBox = new CheckBox
                {
                    Text = phong.TenPhong,
                    Tag = phong.ID,
                    AutoSize = true
                };
                flowLayoutPanelPhongTro.Controls.Add(checkBox);
            }
        }

        private void LoadCongToDienDetails()
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                // Lấy chi tiết công tơ điện
                var sql = @"SELECT * FROM CongToDien WHERE ID = @ID";
                var congToDien = connection.QueryFirstOrDefault<CongToDien>(sql, new { ID = congToDienID });

                if (congToDien != null)
                {
                    txtChiSoCu.Text = congToDien.ChiSoCu.ToString();
                    txtChiSoMoi.Text = congToDien.ChiSoMoi.ToString();
                    chkTong.Checked = congToDien.isTong;
                    chkNau.Checked = congToDien.isNau;

                    // Lấy danh sách phòng trọ liên kết
                    var phongTroSql = @"SELECT PhongTroID FROM CongToPhongTro WHERE CongToDienID = @CongToDienID";
                    var phongTroIDs = connection.Query<int>(phongTroSql, new { CongToDienID = congToDienID }).ToList();

                    // Đánh dấu các phòng trọ đã liên kết
                    foreach (CheckBox checkBox in flowLayoutPanelPhongTro.Controls)
                    {
                        int phongTroID = (int)checkBox.Tag;
                        checkBox.Checked = phongTroIDs.Contains(phongTroID);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int congToID;
                        if (congToDienID.HasValue)
                        {
                            // Cập nhật công tơ điện
                            var sql = @"UPDATE CongToDien SET ChiSoCu = @ChiSoCu, ChiSoMoi = @ChiSoMoi, 
                                    isTong = @isTong, isNau = @isNau WHERE ID = @ID";
                            connection.Execute(sql, new
                            {
                                ChiSoCu = double.Parse(txtChiSoCu.Text),
                                ChiSoMoi = double.Parse(txtChiSoMoi.Text),
                                isTong = chkTong.Checked,
                                isNau = chkNau.Checked,
                                ID = congToDienID.Value
                            }, transaction);
                            congToID = congToDienID.Value;
                        }
                        else
                        {
                            // Thêm mới công tơ điện
                            var sql = @"INSERT INTO CongToDien (NhaTroID, ChiSoCu, ChiSoMoi, isTong, isNau) 
                                    VALUES (@NhaTroID, @ChiSoCu, @ChiSoMoi, @isTong, @isNau);
                                    SELECT last_insert_rowid();";
                            congToID = connection.ExecuteScalar<int>(sql, new
                            {
                                NhaTroID = nhaTroID,
                                ChiSoCu = double.Parse(txtChiSoCu.Text),
                                ChiSoMoi = double.Parse(txtChiSoMoi.Text),
                                isTong = chkTong.Checked,
                                isNau = chkNau.Checked
                            }, transaction);
                        }

                        // Xóa các liên kết cũ
                        var deleteSql = @"DELETE FROM CongToPhongTro WHERE CongToDienID = @CongToDienID";
                        connection.Execute(deleteSql, new { CongToDienID = congToID }, transaction);

                        // Thêm liên kết mới
                        foreach (CheckBox checkBox in flowLayoutPanelPhongTro.Controls)
                        {
                            if (checkBox.Checked)
                            {
                                var insertSql = @"INSERT INTO CongToPhongTro (CongToDienID, PhongTroID) 
                                              VALUES (@CongToDienID, @PhongTroID)";
                                connection.Execute(insertSql, new
                                {
                                    CongToDienID = congToID,
                                    PhongTroID = (int)checkBox.Tag
                                }, transaction);
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Lưu công tơ điện thành công!");
                        this.Close();
                    }
                    catch
                    {
                        transaction.Rollback();
                        MessageBox.Show("Có lỗi xảy ra, vui lòng thử lại.");
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
