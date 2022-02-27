using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIapp.Model;

namespace WebAPIapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangHoaController : ControllerBase
    {
        public static List<HangHoa> hangHoas = new List<HangHoa>();
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(hangHoas);
        }
        [HttpGet("id")]
        public IActionResult GetById(string id)
        {
            try
            {
                var hanghao = hangHoas.SingleOrDefault(x => x.MaHangHoa == Guid.Parse(id));
                if (hanghao == null)
                    return NotFound();
                return Ok(hanghao);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Create(HangHoaMV hangHoaMV)
        {
            var hanghoa = new HangHoa
            {
                MaHangHoa = Guid.NewGuid(),
                TenHangHoa = hangHoaMV.TenHangHoa,
                DonGia = hangHoaMV.DonGia
            };
            hangHoas.Add(hanghoa);
            return Ok(new
            {
                Success = true,
                data = hanghoa
            });
            
        }
        [HttpPut("{id}")]
        public IActionResult Edit(string id,HangHoa hangHoaEdit )
        {
            try
            {
                var hanghoa = hangHoas.SingleOrDefault(x => x.MaHangHoa == Guid.Parse(id));
                if (hanghoa == null)
                    return NotFound();
                if(id != hanghoa.MaHangHoa.ToString())
                {
                    return BadRequest();
                }
                hanghoa.TenHangHoa = hangHoaEdit.TenHangHoa;
                hanghoa.DonGia = hangHoaEdit.DonGia;
                return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(string id, HangHoa hangHoaEdit)
        {
            try
            {
                var hanghoa = hangHoas.SingleOrDefault(x => x.MaHangHoa == Guid.Parse(id));
                if (hanghoa == null)
                    return NotFound();
                if (id != hanghoa.MaHangHoa.ToString())
                {
                    return BadRequest();
                }
                hangHoas.Remove(hanghoa);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
