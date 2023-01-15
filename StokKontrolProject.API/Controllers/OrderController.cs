using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StokKontrolProject.Entities.Entities;
using StokKontrolProject.Entities.Enum;
using StokKontrolProject.Service.Abstract;

namespace StokKontrolProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<User> _userService;
        private readonly IGenericService<Product> _productService;
        private readonly IGenericService<OrderDetails> _odService;
        private readonly IGenericService<Order> _orderService;

        public OrderController(IGenericService<User> userService,IGenericService<Product> productService,IGenericService<OrderDetails> odService,IGenericService<Order> orderService)
        {
            _userService = userService;
            _productService = productService;
            _odService = odService;
            _orderService = orderService;
        }

        // GET: api/Order/TumSiparisleriGetir
        [HttpGet]
        public IActionResult TumSiparisleriGetir()
        {
            return Ok(_orderService.GetAll());
        }

        // GET: api/Order/AktifSiparisleriGetir/5
        [HttpGet]
        public IActionResult AktifSiparisleriGetir()
        {
            return Ok(_orderService.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreSiparisGetir(int id)
        {
            return Ok(_orderService.GetByID(id));
        }
        // GET: api/Order/TumSiparisleriGetir
        [HttpGet]
        public IActionResult BekleyenSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x=>x.Status==Status.Pending).ToList());
        }
        // GET: api/Order/TumSiparisleriGetir
        [HttpGet]
        public IActionResult OnaylananSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Confirmed).ToList());
        }
        [HttpGet]
        public IActionResult ReddedilenSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Cancelled).ToList());
        }

        [HttpPost]
        public IActionResult SiparisEkle(int UserID,[FromQuery] int[] productsIDs,[FromQuery] short[] quantities)
        {
            Order yeniSiparis = new Order();
            yeniSiparis.UserID = UserID;
            yeniSiparis.Status = Status.Pending;
            yeniSiparis.IsActive = true;   

            _orderService.Add(yeniSiparis); // DB'ye eklendiğinde ID oluşturuluyor.

            
                for (int i = 0; i < productsIDs.Length; i++)
                {
                    OrderDetails yeniDetay = new OrderDetails();
                    yeniDetay.OrderID = yeniSiparis.ID;
                    yeniDetay.OrderID = productsIDs[i];
                    yeniDetay.Quantity = quantities[i];
                    yeniDetay.UnitPrice = _productService.GetByID(productsIDs[i]).UnitPrice;
                    yeniDetay.IsActive = true;

                    _odService.Add(yeniDetay);
                }

            return Ok(yeniSiparis);
        }
        [HttpGet("{id}")]
        public IActionResult SiparisOnayla(int id)
        {

            Order confirmedOrder = _orderService.GetByID(id);
            if (confirmedOrder == null)
            {
                return NotFound();
            }
            else
            {
                confirmedOrder.Status = Status.Confirmed;
                confirmedOrder.IsActive = false;
                _orderService.Update(confirmedOrder);
                List<OrderDetails> detaylar = _odService.GetDefault(x => x.OrderID == confirmedOrder.ID).ToList();
                foreach (OrderDetails item in detaylar)
                {
                    Product productInOrder = _productService.GetByID(item.ProductID);
                    productInOrder.Stock -=Convert.ToInt16(item.Quantity);
                    _productService.Update(productInOrder);
                }
                return Ok(confirmedOrder);
            }
            
        }
        [HttpGet("{id}")]
        public IActionResult SiparisReddet(int id)
        {

            Order cancelledOrder = _orderService.GetByID(id);
            if (cancelledOrder == null)
            {
                return NotFound();
            }
            else
            {
                cancelledOrder.Status = Status.Cancelled;
                cancelledOrder.IsActive = false;
                _orderService.Update(cancelledOrder);
                return Ok(cancelledOrder);
            }

        }
    }
}
