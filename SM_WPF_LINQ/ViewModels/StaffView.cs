using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM_WPF_LINQ.ViewModels
{
    public class StaffView
    {
		private int _MaNV;

		private string _HoTen;

		private System.DateTime _NgaySinh;

		private bool _GioiTinh;

		private string _SoDT;

		private double _HeSoLuong;

		private string _MaPhong;

		private string _MaChucVu;

        public int MaNV { get => _MaNV; set => _MaNV = value; }
        public string HoTen { get => _HoTen; set => _HoTen = value; }
        public DateTime NgaySinh { get => _NgaySinh; set => _NgaySinh = value; }
        public bool GioiTinh { get => _GioiTinh; set => _GioiTinh = value; }
        public string SoDT { get => _SoDT; set => _SoDT = value; }
        public double HeSoLuong { get => _HeSoLuong; set => _HeSoLuong = value; }
        public string MaPhong { get => _MaPhong; set => _MaPhong = value; }
        public string MaChucVu { get => _MaChucVu; set => _MaChucVu = value; }

        public StaffView() { }

        public StaffView(int MaNV, string HoTen, DateTime NgaySinh, bool GioiTinh, string SoDT, double HeSoLuong,
            string MaPhong, string MaChucVu)
        {
            this.MaNV = MaNV;
            this.HoTen = HoTen;
            this.NgaySinh = NgaySinh;
            this.GioiTinh = GioiTinh;
            this.SoDT = SoDT;
            this.HeSoLuong = HeSoLuong;
            this.MaPhong = MaPhong;
            this.MaChucVu = MaChucVu;
        }
    }
}
