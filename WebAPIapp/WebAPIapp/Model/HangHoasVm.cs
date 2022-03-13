using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIapp.Model
{
    public class HangHoasVm
    {

        [Required]
        [MaxLength(100)]
        public string TenHh { get; set; }
        [Range(0, double.MaxValue)]
        public double Dongia { get; set; }
        public string TenLoai { get; set; }

    }
}
