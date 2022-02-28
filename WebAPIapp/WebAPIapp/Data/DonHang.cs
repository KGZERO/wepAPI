using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIapp.Data.DetailEnumData;

namespace WebAPIapp.Data
{
    public class DonHang 
    {
        public Guid MaDh { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime? NgayGiao { get; set;}
        public  TrinhDonDatHang trinhDonDatHang { get; set; }
        public string NguoiNhan { get; set; }
        public string DiaChiGiao { get; set; }
        public string SDT { get; set; }
        public ICollection<DonHangChiTiet> DonHangChiTiets { get; set; }
        public DonHang()
        {
            DonHangChiTiets = new List<DonHangChiTiet>();
        }




    }
}
