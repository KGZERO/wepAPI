using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIapp.Model;

namespace WebAPIapp.Services
{
   public interface IHangHoaReponsitory
    {
        List<HangHoaModel> GetAll(string search, double? from, double? to, string sort,int page=1);
        HangHoaModel Add(HangHoasVm hang);
    }
}
