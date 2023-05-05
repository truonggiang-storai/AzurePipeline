using Dapper.Contrib.Extensions;

namespace WebApi.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public long CreatedById { get; set; }
    }
}
