using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIapp.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }
        #region Dbset
        public DbSet<HangHoa> HangHoas{ get; set; }
        public DbSet<Loai> Loais { get; set; }
        #endregion
    }
}
