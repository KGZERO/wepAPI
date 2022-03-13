using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIapp.Model.Common
{
    public class PageResultBase<T>:List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }
        
        public PageResultBase(List<T> items,int count,int pageIndex, int pagesize)
        {
          
            PageIndex = pageIndex;
             TotalPage = (int)Math.Ceiling(count / (double)pagesize);
            AddRange(items);
        }

        public static PageResultBase<T> Create(IQueryable<T> soure, int pageIndex, int pagesize)
        {
            var count = soure.Count();
            var items = soure.Skip((pageIndex - 1) * pagesize).Take(pagesize).ToList();
            return new PageResultBase<T>(items, count, pageIndex, pagesize);
        }
    }
}



