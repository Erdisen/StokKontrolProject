using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProject.Entities.Entities;
using StokKontrolProject.Service.Abstract;

namespace StokKontrolProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IGenericService<Supplier> _service;

        public SupplierController(IGenericService<Supplier> service)
        {
            _service = service;
        }

        // GET: api/Supplier/TumTedarikcileriGetir
        [HttpGet]
        public IActionResult TumTedarikcileriGetir()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/Supplier/AktifTedarikcileriGetir/5
        [HttpGet]
        public IActionResult AktifTedarikcileriGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyeTedarikcileriGetir(int id)
        {
            return Ok(_service.GetByID(id));
        }
        // POST: api/Supplier/TedarikcileriEkle
        [HttpPost]
        public IActionResult TedarikcileriEkle(Supplier supplier)
        {
            _service.Add(supplier);

            return CreatedAtAction("IdyeGoreKategoriGetir", new { id = supplier.ID }, supplier);
        }

        // PUT: api/Supplier/TedarikcileriGuncelle/5
        [HttpPut("{id}")]
        public IActionResult TedarikcileriGuncelle(int id, Supplier supplier)
        {
            if (id != supplier.ID)
            {
                return BadRequest();
            }


            try
            {
                _service.Update(supplier);
                return Ok(supplier);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
                {
                    return NotFound();
                }

            }

            return NoContent();
        }



        // DELETE: api/Supplier/TedarikciSil/5
        [HttpDelete("{id}")]
        public IActionResult TedarikciSil(int id)
        {
            var supplier = _service.GetByID(id);
            if (supplier == null)
            {
                return NotFound();
            }
            try
            {
                _service.Remove(supplier);
                return Ok("Tedarikçi Silindi!");
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

        private bool SupplierExists(int id)
        {
            return _service.Any(e => e.ID == id);
        }


        [HttpGet("{id}")]
        public IActionResult TedarikciAktiflestir(int id)
        {
            var supplier = _service.GetByID(id);
            if (supplier == null)
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
