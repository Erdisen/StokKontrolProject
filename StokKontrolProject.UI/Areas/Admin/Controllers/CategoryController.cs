using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StokKontrolProject.Entities.Entities;
using System.Text;

namespace StokKontrolProject.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {

        string uri = "https://localhost:7163";
        public async Task<IActionResult> Index()
        {
            List<Category> kategoriler = new List<Category>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/TumKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            return View(kategoriler);
        }
        [HttpGet]
        public async Task<IActionResult> KategoriAktiflestir(int id)
        {
            //List<Category> kategoriler = new List<Category>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/KategoriAktiflestir/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            //return View(kategoriler);
            return RedirectToAction("Index");
        }
        //[HttpDelete]
        public async Task<IActionResult> KategoriSil(int id)
        {
            //List<Category> kategoriler = new List<Category>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Category/KategoriSil/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            //return View(kategoriler);
            return RedirectToAction("Index");
        }
      

        [HttpGet]
        public IActionResult KategoriEkle()
        {
            return View(); // Sadece Ekleme view'ını gösterecek
        }
        [HttpPost]
        public async Task<IActionResult> KategoriEkle(Category category)
        {
            category.IsActive = true;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PostAsync($"{uri}/api/Category/KategoriEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }
        //TODO : Kategori Güncelleme İşlemleri
        static Category updateCategory; // İlgili kategoriyi guncelleme işleminin devamında(put) kullanacağımız için o metottan da ualaşabilmek adına globalde tanımlayalım.
        //DateTime eklenmeTarihi = updateCategory.AddedDate;
        [HttpGet]
        public async Task<IActionResult> KategoriGuncelle(int id)// id ile ilgili kategoriyi bul getir.
        {
           
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/IdyeGoreKategoriGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updateCategory = JsonConvert.DeserializeObject<Category>(apiCevap);
                }
            }
            return View(updateCategory); // update edilecek kategoriyi güncelleme View'ınıa gösterecek.
        }
        [HttpPost]
        public async Task<IActionResult> KategoriGuncelle(Category guncelKategori) // Guncellenmiş kategori parametre olarak alınır
        {
            
            using (var httpClient = new HttpClient())
            {

                guncelKategori.AddedDate = updateCategory.AddedDate;
                guncelKategori.IsActive = true;

                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelKategori), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{uri}/api/Category/KategoriGuncelle/{guncelKategori.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //kategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }


            }
            return RedirectToAction("Index"); // update edilecek kategoriyi güncelleme View'ınıa gösterecek.
        }
    }

}
