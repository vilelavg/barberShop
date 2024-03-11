using System.Numerics;

namespace barberShop.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }       
        public long Telephone { get; set; }
        public DateTime Desde { get; set; }

        
    }
}
