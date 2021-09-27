namespace Shopping.Aggregator.Models
{
    using System.Collections.Generic;

    public class BasketModel
    {
        public string UserName { get; set; }

        public List<BasketItemExtenedModel> Items { get; set; } = new List<BasketItemExtenedModel>();

        public decimal TotalPrice { get; set; }
    }
}
