using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProject.Entities.Entities;
using StokKontrolProject.Service.Abstract;

namespace StokKontrolProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericService<Product> _service;

        public ProductController(IGenericService<Product> service)
        {
            _service = service;
        }

        // GET: api/Product/TumUrunleriGetir
        [HttpGet]
        public IActionResult TumUrunleriGetir()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/Product/AktifUrunleriGetir/5
        [HttpGet]
        public IActionResult AktifUrunleriGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreUrunGetir(int id)
        {
            return Ok(_service.GetByID(id));
        }
        // POST: api/Product/UrunEkle
        [HttpPost]
        public IActionResult UrunEkle(Product Product)
        {
            _service.Add(Product);

            return CreatedAtAction("IdyeGoreUrunGetir", new { id = Product.ID }, Product);

//            {
//                "isActive": true,
//  "productName": "Coca Cola 1 LT",
//  "unitPrice": 17.50,
//  "stock": 1000,
//  "expireDate": "2024-01-14T11:59:32.479Z",
//  "categoryID": 4,
//  "supplierID": 1
//}
        }

        // PUT: api/Product/UrunleriGuncelle/5
        [HttpPut("{id}")]
        public IActionResult UrunleriGuncelle(int id, Product Product)
        {
            if (id != Product.ID)
            {
                return BadRequest();
            }

            //_service.Entry(Product).State = EntityState.Modified;

            try
            {
                _service.Update(Product);
                return Ok(Product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }

            }

            return NoContent();
        }



        // DELETE: api/Product/UrunSil/5
        [HttpDelete("{id}")]
        public IActionResult UrunSil(int id)
        {
            var Product = _service.GetByID(id);
            if (Product == null)
            {
                return NotFound();
            }
            try
            {
                _service.Remove(Product);
                return Ok("Urun Silindi!");
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

        private bool ProductExists(int id)
        {
            return _service.Any(e => e.ID == id);
        }


        [HttpGet("{id}")]
        public IActionResult UrunAktiflestir(int id)
        {
            var Product = _service.GetByID(id);
            if (Product == null)
            {
                return NotFound();
            }
            try
            {
                _service.Activate(id);
                return Ok(_service.GetByID(id));

            }
            catch (Exception)
            {

                return BadRequest();
            }


        }
    }
}
