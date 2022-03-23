using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIapp.Model;

namespace WebAPIapp.Services
{
  public  interface ILoaiReponsitory
    {
        List<LoaiVm> GetAll();
        LoaiVm GetById(int id);
        LoaiVm Add(LoaiModel loai);
        void Update(int id,LoaiModel loai);
        void Delete(int id);


    }
}
