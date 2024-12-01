using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinhTienTro
{
    public class ThanhVienService
    {
        public static void Add(ThanhVien thanhVien)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO ThanhVien (TenThanhVien, SDT, SinhNam, NauAn) VALUES (@TenThanhVien, @SDT, @SinhNam, @NauAn)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenThanhVien", thanhVien.TenThanhVien);
                    command.Parameters.AddWithValue("@SDT", thanhVien.SDT);
                    command.Parameters.AddWithValue("@SinhNam", thanhVien.SinhNam);
                    command.Parameters.AddWithValue("@NauAn", thanhVien.nauan);

                    command.ExecuteNonQuery(); // Thực thi câu lệnh INSERT
                }
            }
        }
        public static void Update(ThanhVien thanhVien)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "UPDATE ThanhVien SET TenThanhVien = @TenThanhVien, SDT = @SDT, SinhNam = @SinhNam, NauAn = @NauAn WHERE ID = @ID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenThanhVien", thanhVien.TenThanhVien);
                    command.Parameters.AddWithValue("@SDT", thanhVien.SDT);
                    command.Parameters.AddWithValue("@SinhNam", thanhVien.SinhNam);
                    command.Parameters.AddWithValue("@NauAn", thanhVien.nauan);
                    command.Parameters.AddWithValue("@ID", thanhVien.ID);

                    command.ExecuteNonQuery(); // Thực thi câu lệnh UPDATE
                }
            }
        }
        public static void AddThanhVienToPhongTro(ThanhVien thanhVien, int phongTroID)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO ThanhVien (TenThanhVien, SDT, SinhNam, NauAn, PhongTroID) VALUES (@TenThanhVien, @SDT, @SinhNam, @NauAn, @PhongTroID)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenThanhVien", thanhVien.TenThanhVien);
                    command.Parameters.AddWithValue("@SDT", thanhVien.SDT);
                    command.Parameters.AddWithValue("@SinhNam", thanhVien.SinhNam);
                    command.Parameters.AddWithValue("@NauAn", thanhVien.nauan);
                    command.Parameters.AddWithValue("@PhongTroID", phongTroID);

                    command.ExecuteNonQuery(); // Thực thi câu lệnh INSERT
                }
            }
        }


    }

}
