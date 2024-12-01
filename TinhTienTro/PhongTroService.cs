using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;
using TinhTienTro;

namespace TinhTienTro
{
    public class PhongTroService
    {
        public static List<PhongTro> GetAllPhongTro()
        {
            var phongTros = new List<PhongTro>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT pt.ID, pt.TenPhong, nt.DiaChi 
                    FROM PhongTro pt
                    JOIN NhaTro nt ON pt.NhaTroID = nt.ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            phongTros.Add(new PhongTro
                            {
                                ID = reader.GetInt32(0),
                                TenPhong = reader.GetString(1),
                                // Bạn có thể thêm các trường cần thiết
                            });
                        }
                    }
                }
            }
            return phongTros;
        }
        public static List<PhongTro> GetByNhaTroID(int nhaTroID)
        {
            List<PhongTro> phongTroList = new List<PhongTro>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM PhongTro WHERE NhaTroID = @NhaTroID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NhaTroID", nhaTroID);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PhongTro phongTro = new PhongTro
                            {
                                ID = reader.GetInt32(0),
                                TenPhong = reader.GetString(1)
                            };

                            // Lấy danh sách thành viên của phòng trọ này
                            phongTro.ThanhVien = GetThanhVienByPhongTroID(phongTro.ID);

                            phongTroList.Add(phongTro);
                        }
                    }
                }
            }

            return phongTroList;
        }

        private static List<ThanhVien> GetThanhVienByPhongTroID(int phongTroID)
        {
            List<ThanhVien> thanhVienList = new List<ThanhVien>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM ThanhVien WHERE PhongTroID = @PhongTroID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PhongTroID", phongTroID);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ThanhVien thanhVien = new ThanhVien
                            {
                                ID = reader.GetInt32(0),
                                TenThanhVien = reader.GetString(1),
                                SDT = reader.GetString(2),
                                SinhNam = reader.GetString(3),
                                nauan = reader.GetBoolean(4)
                            };

                            thanhVienList.Add(thanhVien);
                        }
                    }
                }
            }

            return thanhVienList;
        }

    }
}
