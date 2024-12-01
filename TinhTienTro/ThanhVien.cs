using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinhTienTro
{
     public class ThanhVien
    {
        public int ID { get; set; }
        public string TenThanhVien { get; set; }
        public string SDT { get; set; }
        public string SinhNam { get; set; }
        public bool nauan { get; set; }
        public int PhongTroID  { get; set; }

    }
    public class PhongTro
    {
        public int ID { get; set; }
        public string TenPhong { get; set; }
        public List<ThanhVien> ThanhVien { get; set; }

        public int NhaTroID { get; set; }
    }

    public class CongToDien
    {
        public int ID { get; set; }
        public double ChiSoCu { get; set; }
        public double ChiSoMoi { get; set; }
        public bool isTong { get; set; }
        public bool isNau { get; set; }

        public List<PhongTro> PhongTro { get; set; }
    }
    public class NhaTro
    {
        public int ID { get; set; }
        public string DiaChi { get; set; }
        public List<CongToDien> CongToDien { get; set; }
        public List<PhongTro> PhongTro { get; set; }
    }

    public class ChiPhi
    {
        public int ID { get; set; }
        public string TenPhi { get; set; }
        public double GiaTien { get; set; }
    }

}
