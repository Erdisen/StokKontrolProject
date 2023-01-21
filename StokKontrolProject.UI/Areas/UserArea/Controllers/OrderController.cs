using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProject.Entities.Entities;
using System.Net.Http;
using System.Text;

namespace StokKontrolProject.UI.Areas.UserArea.Controllers
{

    [Area("UserArea")]
    public class OrderController : Controller
    {

        string uri = "https://localhost:7163";
        public async Task<IActionResult> Index()
        {
            List<Order> siparisler = new List<Order>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/TumSiparisleriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    siparisler = JsonConvert.DeserializeObject<List<Order>>(apiCevap);
                }
            }
            return View(siparisler);
        }
        [HttpGet]
        public async Task<IActionResult> SiparisOnayla(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/SiparisOnayla/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> SiparisReddet(int id)
        {

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/SiparisReddet/{id}"))
                {

                }
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> SiparisDetayiGor(int id)
        {
            List<Order> siparis = new List<Order>();
            List<Product> urunListesi = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/IdyeGoreSiparisGetir/{id}"))
                {
                    // TODO : Sipariş Detayları listeli geldiği için deserilaze edemiyor. Kontrol edilmeli!
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    siparis = JsonConvert.DeserializeObject<List<Order>>(apiCevap);
                }

                foreach (Order item in siparis)
                {
                    foreach (OrderDetails item1 in item.SiparisDetaylari)
                    {
                        using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/IdyeGoreSiparisGetir/{item1.ProductID}"))
                        {
                            // TODO : Sipariş Detayları listeli geldiği için deserilaze edemiyor. Kontrol edilmeli!
                            string apiCevap = await cevap.Content.ReadAsStringAsync();
                            urunListesi.Add(JsonConvert.DeserializeObject<Product>(apiCevap));
                        }
                    }
                }
            }
            return View(new Tuple<List<Order>, List<Product>>(siparis, urunListesi)); // Tuple = Birden fazla modeli döndürmemizi sağlayan yapıdır.
        }



    }
}

