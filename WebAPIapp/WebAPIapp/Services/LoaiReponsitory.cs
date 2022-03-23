using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIapp.Data;
using WebAPIapp.Model;

namespace WebAPIapp.Services
{
    public class LoaiReponsitory : ILoaiReponsitory
    {
        private readonly MyDbContext _context;
        public LoaiReponsitory(MyDbContext context)
        {
            _context = context;
        }
        public LoaiVm Add(LoaiModel loai)
        {
            var loais = new Loai
            {
                TenLoai=loai.TenLoai
            };
            _context.Add(loais);
            _context.SaveChanges();
            return new LoaiVm
            {
                MaLoai=loais.MaLoai,
                TenLoai=loais.TenLoai
            };
        }

        public List<LoaiVm> GetAll()
        {
            var loais = _context.Loais.Select(x => new LoaiVm
            {
                MaLoai = x.MaLoai,
                TenLoai = x.TenLoai,
            }).ToList();
            return loais;
        }

        public LoaiVm GetById(int id)
        {
            var loais = _context.Loais.SingleOrDefault(x => x.MaLoai == id);
            if (loais == null)
                return null;
          return new LoaiVm
            {
                MaLoai = loais.MaLoai,
                TenLoai =loais.TenLoai
            };
            
        }

        public void Delete(int id)
        {
            var loais = _context.Loais.FirstOrDefault(x => x.MaLoai == id);
            if (loais != null)
            _context.Remove(loais);
            _context.SaveChanges();
        }

        public void Update(int id,LoaiModel loai)
        {
            var loais = _context.Loais.FirstOrDefault(x => x.MaLoai == id);
            if (loais != null)
                loai.TenLoai = loais.TenLoai;
                _context.Add(loais);
            _context.SaveChanges();
        }
    }
}
