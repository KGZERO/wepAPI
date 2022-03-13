using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIapp.Model
{
    public class HangHoaMV
    {
        public string TenHangHoa { get; set; }
        public double DonGia { get; set; }

    }
    public class HangHoam: HangHoaMV
    {
        public Guid MaHangHoa { get; set; }
        

    }
}
