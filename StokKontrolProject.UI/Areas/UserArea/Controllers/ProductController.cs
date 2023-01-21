using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StokKontrolProject.Entities.Entities;
using System.Text;

namespace StokKontrolProject.UI.Areas.UserArea.Controllers
{
    [Area("UserArea")]
    public class ProductController : Controller
    {

        string uri = "https://localhost:7163";
        public async Task<IActionResult> Index()
        {
            List<Product> urunler = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/TumUrunleriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }
            }
            return View(urunler);
        }
        [HttpGet]
        public async Task<IActionResult> UrunAktiflestir(int id)
        {
            //List<Product> urunler = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/UrunAktiflestir/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }
            }
            //return View(urunler);
            return RedirectToAction("Index");
        }
        //[HttpDelete]
        public async Task<IActionResult> UrunSil(int id)
        {
            //List<Product> urunler = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Product/UrunSil/{id}"))
                {
                    //string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }
            }
            //return View(urunler);
            return RedirectToAction("Index");
        }

        static List<Category> aktifKategoriler;
        static List<Supplier> aktifTedarikciler;
        [HttpGet]
        public async Task<IActionResult> UrunEkle()
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/AktifKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifKategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/AktifTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifTedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }

            ViewBag.AktifKategoriler = new SelectList(aktifKategoriler, "ID", "CategoryName");
            ViewBag.AktifTedarikciler = new SelectList(aktifTedarikciler, "ID", "SupplierName");
            return View(); // Sadece Ekleme view'ını gösterecek
        }
        [HttpPost]
        public async Task<IActionResult> UrunEkle(Product product)
        {
            product.IsActive = true;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
                using (var cevap = await httpClient.PostAsync($"{uri}/api/Product/UrunEkle", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }
            }
            return RedirectToAction("Index");
        }
        //TODO : Urun Güncelleme İşlemleri
        static Product updateProduct; // İlgili urunyi guncelleme işleminin devamında(put) kullanacağımız için o metottan da ualaşabilmek adına globalde tanımlayalım.
        //DateTime eklenmeTarihi = updateProduct.AddedDate;
        [HttpGet]
        public async Task<IActionResult> UrunGuncelle(int id)// id ile ilgili urunyi bul getir.
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/IdyeGoreUrunGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updateProduct = JsonConvert.DeserializeObject<Product>(apiCevap);
                }
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/AktifKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifKategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/AktifTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifTedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            ViewBag.AktifKategoriler = new SelectList(aktifKategoriler, "ID", "CategoryName");
            ViewBag.AktifTedarikciler = new SelectList(aktifTedarikciler, "ID", "SupplierName");
            return View(updateProduct); // update edilecek urunyi güncelleme View'ınıa gösterecek.
        }


        [HttpPost]
        public async Task<IActionResult> UrunGuncelle(Product guncelUrun) // Guncellenmiş urun parametre olarak alınır
        {

            using (var httpClient = new HttpClient())
            {

                guncelUrun.AddedDate = updateProduct.AddedDate;
                guncelUrun.IsActive = true;

                StringContent content = new StringContent(JsonConvert.SerializeObject(guncelUrun), Encoding.UTF8, "application/json");

                using (var cevap = await httpClient.PutAsync($"{uri}/api/Product/UrunGuncelle/{guncelUrun.ID}", content))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    //urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }


            }
            return RedirectToAction("Index"); // update edilecek urunyi güncelleme View'ınıa gösterecek.
        }
    }
}
