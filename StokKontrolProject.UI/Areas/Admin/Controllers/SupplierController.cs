using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StokKontrolProject.Entities.Entities;
using System.Text;

namespace StokKontrolProject.UI.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class SupplierController : Controller
    {

        string uri = "https://localhost:7163";
        public async Task<IActionResult> Index()
        {
            List<Supplier> tedarikciler = new List<Supplier>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/TumTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    tedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            return View(tedarikciler);
        }
        [HttpGet]
        public async Task<IActionResult> TedarikciAktiflestir(int id)
        {
            //List<Supplier> tedarikciler = new List<Supplier>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/TedarikciAktiflestir/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //tedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            //return View(tedarikciler);
            return RedirectToAction("Index");
        }
        //[HttpDelete]
        public async Task<IActionResult> TedarikciSil(int id)
        {
            //List<Supplier> tedarikciler = new List<Supplier>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Supplier/TedarikciSil/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //tedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            //return View(tedarikciler);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult TedarikciEkle()
        {
            return View(); // Sadece Ekleme view'ını gösterecek
        }
        [HttpPost]
        public async Task<IActionResult> TedarikciEkle(Supplier supplier)
        {
            supplier.IsActive = true;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(supplier), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PostAsync($"{uri}/api/Supplier/TedarikciEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //tedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }
        //TODO : Tedarikci Güncelleme İşlemleri
        static Supplier updateSupplier; // İlgili tedarikciyi guncelleme işleminin devamında(put) kullanacağımız için o metottan da ualaşabilmek adına globalde tanımlayalım.
                                        //DateTime eklenmeTarihi = updateSupplier.AddedDate;
        [HttpGet]
        public async Task<IActionResult> TedarikciGuncelle(int id)// id ile ilgili tedarikciyi bul getir.
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/IdyeGoreTedarikciGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updateSupplier = JsonConvert.DeserializeObject<Supplier>(apiCevap);
                }
            }
            return View(updateSupplier); // update edilecek tedarikciyi güncelleme View'ınıa gösterecek.
        }
        [HttpPost]
        public async Task<IActionResult> TedarikciGuncelle(Supplier guncelTedarikci) // Guncellenmiş tedarikci parametre olarak alınır
        {

            using (var httpClient = new HttpClient())
            {

                guncelTedarikci.AddedDate = updateSupplier.AddedDate;
                guncelTedarikci.IsActive = true;
                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelTedarikci), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{uri}/api/Supplier/TedarikciGuncelle/{guncelTedarikci.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //tedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }


            }
            return RedirectToAction("Index"); // update edilecek tedarikciyi güncelleme View'ınıa gösterecek.
        }
    }

}
