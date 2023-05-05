using WebApi.Models;

namespace WebApi.Resourses
{
    public class ProductPresentResource
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public User? CreatedBy { get; set; }
    }
}
