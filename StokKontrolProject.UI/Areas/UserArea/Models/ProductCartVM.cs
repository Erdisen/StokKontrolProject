namespace StokKontrolProject.UI.Areas.UserArea.Models
{
    public class ProductCartVM
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? Stock { get; set; }
        public int CategoryID { get; set; }
        public short Quantity { get; set; }

        public decimal? TotalAmount { get { return UnitPrice * Quantity; } }
    }
}
