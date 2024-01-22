using Talabat2.core.Entities;

namespace Talabat.Apis2.Dtos
{
    public class CustomerBasketDto
    {

        public string Id { get; set; }


        public string? PaymentIntentId { get; set; }

        public string? ClientSecret { get; set; }

        public int? DeliveryMethodId { get; set; }

        public decimal ShippingCost { get; set; }
        public List<BasketItemDto> Items { get; set; }
    }
}
