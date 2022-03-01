using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIapp.Data;
using WebAPIapp.Model;

namespace WebAPIapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaisController : ControllerBase
    {
        private readonly MyDbContext _context;
        public LoaisController(MyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        
        public IActionResult GetAll()
        {
            var dsLoai = _context.Loais.ToList();
            return Ok(dsLoai);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var dsLoai = _context.Loais.SingleOrDefault(x=>x.MaLoai==id);
            if (dsLoai == null)
                return NotFound();
            return Ok(dsLoai);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Create(LoaiModel model)
        {
            try
            {
                var loai = new Loai
                {
                    TenLoai = model.TenLoai
                };
                _context.Add(loai);
                _context.SaveChanges();
                return Ok(loai);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public IActionResult    UpdateLoaiById(int id,LoaiModel model)
        {
            var dsLoai = _context.Loais.SingleOrDefault(x => x.MaLoai == id);
            if (dsLoai == null)
                return NotFound();
            dsLoai.TenLoai = model.TenLoai;
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteLoaiById(int id)
        {
            var dsLoai = _context.Loais.SingleOrDefault(x => x.MaLoai == id);
            if (dsLoai == null)
                return NotFound();
           
            _context.Remove(dsLoai);
            return NoContent();
        }
    }
}
