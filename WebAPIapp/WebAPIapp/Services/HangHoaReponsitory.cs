using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIapp.Data;
using WebAPIapp.Model;
using WebAPIapp.Model.Common;

namespace WebAPIapp.Service
{
    public class HangHoaReponsitory : IHangHoaReponsitory
    {
        private readonly MyDbContext _context;
        public static int PAGE_SIZE { get; set; } = 3;

        public HangHoaReponsitory(MyDbContext context)
        {
            _context = context;
        }

        public HangHoaModel Add(HangHoasVm hang)
        {
            var hanghoa = new HangHoa
            {
                TenHh = hang.TenHh,
                Dongia = hang.Dongia,
                Loai = new Loai()
                {
                    TenLoai = hang.TenLoai
                }
            };
            _context.Add(hanghoa);
            _context.SaveChanges();
            return new HangHoaModel
            {
                MaHangHoa = hanghoa.MaHh,
                TenHangHoa = hanghoa.TenHh,
                DonGia = hanghoa.Dongia,
                TenLoai = hanghoa.Loai.TenLoai
            };
        }

        public List<HangHoaModel> GetAll(string search, double? from, double? to, string sort, int page = 1)
        {

            var searcha = _context.HangHoas.Include(hh => hh.Loai).AsQueryable();
            #region filtering
            if (!string.IsNullOrEmpty(search))
            {
                searcha = searcha.Where(x => x.TenHh.Contains(search) || x.MaHh.ToString().Contains(search));
            }
            if (from.HasValue)
            {
                searcha = searcha.Where(x => x.Dongia >= from);
            }
            if (to.HasValue)
            {
                searcha = searcha.Where(x => x.Dongia <= to);
            }
            #endregion
            #region sort
            searcha = searcha.OrderBy(x => x.TenHh);
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "ten_desc": searcha = searcha.OrderByDescending(x => x.TenHh); break;
                    case "ten_asc": searcha = searcha.OrderBy(x => x.TenHh); break;
                    case "gia_desc": searcha = searcha.OrderByDescending(x => x.Dongia); break;
                    case "gia_asc": searcha = searcha.OrderBy(x => x.Dongia); break;
                }
            }
            #endregion

            //searcha = searcha.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            //return searcha.Select(x => new HangHoaModel
            //{
            //    MaHangHoa = x.MaHh,
            //    TenHangHoa = x.TenHh,
            //    DonGia = x.Dongia,
            //    TenLoai = x.Loai.TenLoai
            //}).ToList();
            var result = PageResultBase<HangHoa>.Create(searcha, page, PAGE_SIZE);
            return result.Select(x => new HangHoaModel
            {
                MaHangHoa = x.MaHh,
                TenHangHoa = x.TenHh,
                DonGia = x.Dongia,
                TenLoai = x.Loai?.TenLoai
            }).ToList();



        }


    }
}
