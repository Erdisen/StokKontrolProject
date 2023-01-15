using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrolProject.Entities.Entities;
using StokKontrolProject.Service.Abstract;

namespace StokKontrolProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericService<User> _service;

        public UserController(IGenericService<User> service)
        {
            _service = service;
        }

        // GET: api/User/TumKullanicilariGetir
        [HttpGet]
        public IActionResult TumKullanicilariGetir()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/User/AktifKullanicilariGetir/5
        [HttpGet]
        public IActionResult AktifKullanicilariGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreKullaniciGetir(int id)
        {
            return Ok(_service.GetByID(id));
        }
        // POST: api/User/KullaniciEkle
        [HttpPost]
        public IActionResult KullaniciEkle(User user)
        {
            _service.Add(user);

            return CreatedAtAction("IdyeGoreKullaniciGetir", new { id = user.ID }, user);


        }

        // PUT: api/User/KullanicilariGuncelle/5
        [HttpPut("{id}")]
        public IActionResult KullanicilariGuncelle(int id, User user)
        {
            if (id != user.ID)
            {
                return BadRequest();
            }

            //_service.Entry(User).State = EntityState.Modified;

            try
            {
                _service.Update(user);
                return Ok(User);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }

            }

            return NoContent();
        }



        // DELETE: api/User/KullaniciSil/5
        [HttpDelete("{id}")]
        public IActionResult KullaniciSil(int id)
        {
            var User = _service.GetByID(id);
            if (User == null)
            {
                return NotFound();
            }
            try
            {
                _service.Remove(User);
                return Ok("Kullanici Silindi!");
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

        private bool UserExists(int id)
        {
            return _service.Any(e => e.ID == id);
        }


        [HttpGet("{id}")]
        public IActionResult KullaniciAktiflestir(int id)
        {
            var user = _service.GetByID(id);
            if (user == null)
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

        [HttpGet]
        public IActionResult Login(string email, string parola)
        {
            if (_service.Any(x => x.Email == email && x.Password == parola))
            {
                User logged = _service.GetByDefault(x => x.Email == email && x.Password == parola);
                return Ok(logged);
            }
            return NotFound();

        }  
    }
}
