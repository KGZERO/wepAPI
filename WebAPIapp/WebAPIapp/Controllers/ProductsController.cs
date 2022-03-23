using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIapp.Model;
using WebAPIapp.Services;

namespace WebAPIapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IHangHoaReponsitory _hangHoaReponsitory;

        public ProductsController(IHangHoaReponsitory hangHoaReponsitory)
        {
            _hangHoaReponsitory = hangHoaReponsitory;
        }
        [HttpGet]
        public IActionResult GetAll (string search, double? from, double? to, string sort,int page=1)
        {
            try
            {
                var result = _hangHoaReponsitory.GetAll(search, from, to, sort,page);
                if (result == null)
                    return BadRequest("hang hoa null");
                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


            
          
                
            
        }
        [HttpPost]
        public IActionResult Create(HangHoasVm hang)
        {
            try
            {
               
                var resut = _hangHoaReponsitory.Add(hang);
                if (resut == null)
                    return BadRequest();
                return Ok(resut);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
