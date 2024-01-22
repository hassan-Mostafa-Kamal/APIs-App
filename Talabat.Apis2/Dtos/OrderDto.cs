using System.ComponentModel.DataAnnotations;

namespace Talabat.Apis2.Dtos
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto shippingAddress { get; set; }

    }
}
