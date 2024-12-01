using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using TinhTienTro;

namespace TinhTienTro
{
    public class NhaTroService
    {
        public static List<NhaTro> GetAll()
        {
            var nhaTros = new List<NhaTro>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM NhaTro";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nhaTros.Add(new NhaTro
                            {
                                ID = reader.GetInt32(0),
                                DiaChi = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return nhaTros;
        }

        public static void Add(NhaTro nhaTro)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO NhaTro (DiaChi) VALUES (@DiaChi)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DiaChi", nhaTro.DiaChi);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static NhaTro GetById(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                // Lấy thông tin nhà trọ
                string nhaTroQuery = "SELECT ID, DiaChi FROM NhaTro WHERE ID = @ID";
                NhaTro nhaTro = null;

                using (var command = new SQLiteCommand(nhaTroQuery, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nhaTro = new NhaTro
                            {
                                ID = reader.GetInt32(0),
                                DiaChi = reader.GetString(1),
                                PhongTro = new List<PhongTro>()
                            };
                        }
                    }
                }

                if (nhaTro != null)
                {
                    // Lấy danh sách phòng trọ
                    string phongTroQuery = "SELECT ID, TenPhong FROM PhongTro WHERE NhaTroID = @NhaTroID";
                    using (var phongTroCommand = new SQLiteCommand(phongTroQuery, connection))
                    {
                        phongTroCommand.Parameters.AddWithValue("@NhaTroID", id);
                        using (var reader = phongTroCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nhaTro.PhongTro.Add(new PhongTro
                                {
                                    ID = reader.GetInt32(0),
                                    TenPhong = reader.GetString(1),
                                    ThanhVien = new List<ThanhVien>()
                                });
                            }
                        }
                    }
                }

                return nhaTro;
            }
        }
        public static NhaTro GetByIdWithDetails(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                // Lấy thông tin nhà trọ
                string nhaTroQuery = "SELECT ID, DiaChi FROM NhaTro WHERE ID = @ID";
                NhaTro nhaTro = null;

                using (var command = new SQLiteCommand(nhaTroQuery, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nhaTro = new NhaTro
                            {
                                ID = reader.GetInt32(0),
                                DiaChi = reader.GetString(1),
                                PhongTro = new List<PhongTro>()
                            };
                        }
                    }
                }

                if (nhaTro != null)
                {
                    // Lấy danh sách thành viên và phòng của họ
                    string query = @"
                SELECT 
                    p.ID AS PhongTroID, 
                    p.TenPhong, 
                    t.ID AS ThanhVienID, 
                    t.TenThanhVien, 
                    t.SDT, 
                    t.SinhNam, 
                    t.nauan
                FROM PhongTro p
                LEFT JOIN ThanhVien t ON t.PhongTroID = p.ID
                WHERE p.NhaTroID = @NhaTroID";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NhaTroID", id);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Lấy thông tin phòng trọ
                                var phongTroID = reader.GetInt32(0);
                                var phongTro = nhaTro.PhongTro.FirstOrDefault(p => p.ID == phongTroID);
                                if (phongTro == null)
                                {
                                    phongTro = new PhongTro
                                    {
                                        ID = phongTroID,
                                        TenPhong = reader.GetString(1),
                                        ThanhVien = new List<ThanhVien>()
                                    };
                                    nhaTro.PhongTro.Add(phongTro);
                                }

                                // Lấy thông tin thành viên
                                if (!reader.IsDBNull(2)) // Kiểm tra nếu thành viên tồn tại
                                {
                                    phongTro.ThanhVien.Add(new ThanhVien
                                    {
                                        ID = reader.GetInt32(2),
                                        TenThanhVien = reader.GetString(3),
                                        SDT = reader.GetString(4),
                                        SinhNam = reader.GetString(5),
                                        nauan = reader.GetBoolean(6)
                                    });
                                }
                            }
                        }
                    }
                }

                return nhaTro;
            }
        }

        public static DataTable GetThanhVienByNhaTroID(int nhaTroID)
        {
            DataTable dataTable = new DataTable();
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = @"
            SELECT 
                ThanhVien.ID AS ThanhVienID,
                ThanhVien.TenThanhVien,
                ThanhVien.SDT,
                ThanhVien.SinhNam,
                ThanhVien.NauAn,
                PhongTro.TenPhong
            FROM 
                ThanhVien
            INNER JOIN 
                PhongTro ON ThanhVien.PhongTroID = PhongTro.ID
            WHERE 
                PhongTro.NhaTroID = @NhaTroID;
        ";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NhaTroID", nhaTroID);
                    using (var adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }
        public static void Update(NhaTro nhaTro)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "UPDATE NhaTro SET DiaChi = @DiaChi WHERE ID = @ID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DiaChi", nhaTro.DiaChi);
                    command.Parameters.AddWithValue("@ID", nhaTro.ID);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Delete(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM NhaTro WHERE ID = @ID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
