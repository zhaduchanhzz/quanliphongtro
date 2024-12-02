using System;
using System.Data.SQLite;

namespace TinhTienTro
{
    public static class DatabaseHelper
    {
        private static string connectionString = "Data Source=nhatro.db;Version=3;";

        // Kết nối đến SQLite
        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        // Tạo database và các bảng
        public static void InitializeDatabase()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                // Tạo bảng NhaTro
                var createNhaTroTable = @"
                    CREATE TABLE IF NOT EXISTS NhaTro (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        DiaChi NVARCHAR(255) NOT NULL
                    );";
                ExecuteNonQuery(createNhaTroTable, connection);

                // Tạo bảng PhongTro
                var createPhongTroTable = @"
                    CREATE TABLE IF NOT EXISTS PhongTro (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        NhaTroID INTEGER NOT NULL,
                        TenPhong NVARCHAR(50) NOT NULL,
GiaPhong REAL NOT NULL,
                        FOREIGN KEY (NhaTroID) REFERENCES NhaTro(ID) ON DELETE CASCADE
                    );";
                ExecuteNonQuery(createPhongTroTable, connection);

                // Tạo bảng ThanhVien
                var createThanhVienTable = @"
                    CREATE TABLE IF NOT EXISTS ThanhVien (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        PhongTroID INTEGER NOT NULL,
                        TenThanhVien NVARCHAR(100) NOT NULL,
                        SDT NVARCHAR(15),
                        SinhNam NVARCHAR(10),
                        NauAn BOOLEAN NOT NULL,
                        FOREIGN KEY (PhongTroID) REFERENCES PhongTro(ID) ON DELETE CASCADE
                    );";
                ExecuteNonQuery(createThanhVienTable, connection);

                // Tạo bảng CongToDien
                var createCongToDienTable = @"
                    CREATE TABLE IF NOT EXISTS CongToDien (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        NhaTroID INTEGER NOT NULL,
                        ChiSoCu REAL NOT NULL,
                        ChiSoMoi REAL NOT NULL,
                        isTong BOOLEAN NOT NULL,
                        isNau BOOLEAN NOT NULL,
                        FOREIGN KEY (NhaTroID) REFERENCES NhaTro(ID) ON DELETE CASCADE
                    );";
                ExecuteNonQuery(createCongToDienTable, connection);

                // Tạo bảng ChiPhi
                var createChiPhiTable = @"
                    CREATE TABLE IF NOT EXISTS ChiPhi (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        TenPhi NVARCHAR(100) NOT NULL,
                        GiaTien REAL NOT NULL
                    );";
                ExecuteNonQuery(createChiPhiTable, connection);
                var createCongToPhongTro = @"
                   CREATE TABLE IF NOT EXISTS CongToPhongTro (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    CongToDienID INTEGER NOT NULL,
    PhongTroID INTEGER NOT NULL,
    FOREIGN KEY (CongToDienID) REFERENCES CongToDien(ID) ON DELETE CASCADE,
    FOREIGN KEY (PhongTroID) REFERENCES PhongTro(ID) ON DELETE CASCADE
);";
                ExecuteNonQuery(createCongToPhongTro, connection);
            }
        }

        private static void ExecuteNonQuery(string query, SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
