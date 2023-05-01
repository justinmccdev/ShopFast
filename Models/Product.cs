namespace ShopFast.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        // Foreign key for the category
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

}
