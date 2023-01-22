using StokKontrolProject.Entities.Entities;

namespace StokKontrolProject.UI.Areas.UserArea.Models
{
    public class ProductCart
    {
        private Dictionary<int, ProductCartVM> _cart = null;

        public ProductCart()
        {
            _cart = new Dictionary<int, ProductCartVM>();
        }

        public List<ProductCartVM> CartProductList
        {
            get
            {
                return _cart.Values.ToList(); // Sepet koleksiyonundaki değerleri(ürünleri) döndür.
            }
            //set; 
        }

        //Sepete Ekle
        public void AddCart(ProductCartVM product)
        {
            if (_cart.ContainsKey(product.ID))
            {
                _cart.Add(product.ID, product); // Eğer ürün sepette bulunmuyorsa, ürünün ID'sini koleksiyona key olarak, Kendisini value olarak eklesin.
            }
            else
            {
                _cart[product.ID].Quantity++; // Eğer ürün sepette var ise var olan ürünün sayısını 1 artır.
            }
        }
        //Sepet Sil
        public void RemoveCart(int productID)
        {
            _cart.Remove(productID); // Sepetten , ilgili ürünü sil.
        }
        //Sepette Miktarı Azalt
        public void DecreaseCart(int productID)
        {
            _cart[productID].Quantity--; // İlgili ürünün miktarını sepette 1 azalt.

            if (_cart[productID].Quantity <= 0) // eğer ürürnün miktarı sepette 0 veya daha az ise
            {
                _cart.Remove(productID); // ürünü sepetten sil.
            }
        }
        //Sepette Miktarı Artır
        public void IncreaseCart(int productID)
        {
            _cart[productID].Quantity++; // İlgili ürünün miktarını sepette 1 artır.

            if (_cart[productID].Quantity >= _cart[productID].Stock) // eğer sepette, ürünün miktarı ürünün stoğundan fazla olmaya çalışıyorsa
            {
                _cart[productID].Quantity = (short)_cart[productID].Stock; // ürünü miktarı en fazla stok kadar olabilir.
            }
        }
    }
}
