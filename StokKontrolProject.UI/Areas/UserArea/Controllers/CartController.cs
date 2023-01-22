using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrolProject.Entities.Entities;
using StokKontrolProject.UI.Areas.UserArea.Helper;
using StokKontrolProject.UI.Areas.UserArea.Models;
using System;
using System.Text.Json;
using System.Web.Mvc;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace StokKontrolProject.UI.Areas.UserArea.Controllers
{
    [Area("UserArea")]
    public class CartController : Controller
    {
        public CartController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult List()
        {
            if (SessionHelper.GetObjectFromJson<ProductCart>(HttpContext.Session, "sepet") != null)
            {
                ProductCart cart = SessionHelper.GetObjectFromJson<ProductCart>(HttpContext.Session, "sepet");

                List<ProductCartVM> productList = cart.CartProductList.Select(x => new ProductCartVM
                {
                    ID = x.ID,
                    ProductName = x.ProductName,
                    UnitPrice = x.UnitPrice,
                    Stock = x.Stock,
                    Quantity = x.Quantity
                }).ToList();
                return Json(productList, JsonRequestBehavior.AllowGet);
            }
            return Json("Empty", JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        public async Task<JsonResult> Add(int id)
        {
            string uri = "https://localhost:7163";

            Product product = new Product();  // Eklenecek ürünü bulmamız gerekiyor.

            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/IdyeGoreUrunGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<Product>(apiCevap);
                }
            }
            ProductCartVM pcVM = new ProductCartVM
            {
                ID = product.ID,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                CategoryID = product.CategoryID,
                Stock = product.Stock,
                Quantity = 1 // sepete 1 adet eklenecek
            };
            if (SessionHelper.GetObjectFromJson<ProductCart>(HttpContext.Session, "sepet") != null)
            {
                ProductCart cart = SessionHelper.GetObjectFromJson<ProductCart>(HttpContext.Session, "sepet");
                cart.AddCart(pcVM); // Ürünü var olan sepete ekle

                SessionHelper.SetObjectAsJson(HttpContext.Session, "sepet", cart); // Var olan sepete ürün eklenmiş halini Session olarak tut.
            }
            else
            {
                //Hazıra bir sepet session'ı yoksa.
                ProductCart cart = new ProductCart();
                cart.AddCart(pcVM);//Ürün ekle
                SessionHelper.SetObjectAsJson(HttpContext.Session, "sepet", cart); // Yeni bir sepet session'ı oluştur ve cart nesnesini ekle

            }
            return Json(""); // Boş bir json döndürsün.
        }
        public JsonResult DecreaseCart(int id)
        {
            if (SessionHelper.GetObjectFromJson<ProductCart>(HttpContext.Session, "sepet") != null)
            {
                ProductCart cart = SessionHelper.GetObjectFromJson<ProductCart>(HttpContext.Session, "sepet");
                cart.DecreaseCart(id); // Ürünü var olan sepette azalt

                SessionHelper.SetObjectAsJson(HttpContext.Session, "sepet", cart); 
            }
            return Json("");
        }
        public JsonResult IncreaseCart(int id)
        {
            if (SessionHelper.GetObjectFromJson<ProductCart>(HttpContext.Session, "sepet") != null)
            {
                ProductCart cart = SessionHelper.GetObjectFromJson<ProductCart>(HttpContext.Session, "sepet");
                cart.IncreaseCart(id); // Ürünü var olan sepette artır.

                SessionHelper.SetObjectAsJson(HttpContext.Session, "sepet", cart); 
            }
            return Json("");
        }

        public JsonResult RemoveCart(int id)
        {
            
                ProductCart cart = SessionHelper.GetObjectFromJson<ProductCart>(HttpContext.Session, "sepet");
                cart.RemoveCart(id); // Ürünü var olan sepetten sil

                SessionHelper.SetObjectAsJson(HttpContext.Session, "sepet", cart); 
           
            return Json("");
        }

    }
}
