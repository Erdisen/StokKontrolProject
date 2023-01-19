using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProject.Entities.Entities;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace StokKontrolProject.UI.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class UserController : Controller
    {

        string uri = "https://localhost:7163";
        public async Task<IActionResult> Index()
        {
            List<User> kullanicilar = new List<User>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/TumKullanicilariGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    kullanicilar = JsonConvert.DeserializeObject<List<User>>(apiCevap);
                }
            }
            return View(kullanicilar);
        }
        [HttpGet]
        public async Task<IActionResult> KullaniciAktiflestir(int id)
        {
            //List<User> kullanicilar = new List<User>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/KullaniciAktiflestir/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kullanicilar = JsonConvert.DeserializeObject<List<User>>(apiCevap);
                }
            }
            //return View(kullanicilar);
            return RedirectToAction("Index");
        }
        //[HttpDelete]
        public async Task<IActionResult> KullaniciSil(int id)
        {
            //List<User> kullanicilar = new List<User>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/User/KullaniciSil/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kullanicilar = JsonConvert.DeserializeObject<List<User>>(apiCevap);
                }
            }
            //return View(kullanicilar);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult KullaniciEkle()
        {
            return View(); // Sadece Ekleme view'ını gösterecek
        }
        [HttpPost]
        public async Task<IActionResult> KullaniciEkle(User user)
        {
            user.IsActive = true;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PostAsync($"{uri}/api/User/KullaniciEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kullanicilar = JsonConvert.DeserializeObject<List<User>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }
        //TODO : Kullanici Güncelleme İşlemleri
        static User updateUser; // İlgili tedarikciyi guncelleme işleminin devamında(put) kullanacağımız için o metottan da ualaşabilmek adına globalde tanımlayalım.
                                        //DateTime eklenmeTarihi = updateUser.AddedDate;
        [HttpGet]
        public async Task<IActionResult> KullaniciGuncelle(int id)// id ile ilgili tedarikciyi bul getir.
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/IdyeGoreKullaniciGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updateUser = JsonConvert.DeserializeObject<User>(apiCevap);
                }
            }
            return View(updateUser); // update edilecek tedarikciyi güncelleme View'ınıa gösterecek.
        }
        [HttpPost]
        public async Task<IActionResult> KullaniciGuncelle(User guncelKullanici) // Guncellenmiş tedarikci parametre olarak alınır
        {

            using (var httpClient = new HttpClient())
            {

                guncelKullanici.AddedDate = updateUser.AddedDate;
                guncelKullanici.IsActive = true;
               
                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelKullanici), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{uri}/api/User/KullaniciGuncelle/{guncelKullanici.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kullanicilar = JsonConvert.DeserializeObject<List<User>>(apiCevap);
                }


            }
            return RedirectToAction("Index"); // update edilecek tedarikciyi güncelleme View'ınıa gösterecek.
        }
    }

}
