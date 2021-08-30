namespace Basket.API.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    public class ShoppingCart
    {
        public ShoppingCart(string userName)
        {
            this.UserName = userName;
        }

        public string UserName { get; set; }

        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
    }
}
