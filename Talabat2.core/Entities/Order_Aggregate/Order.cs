using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat2.core.Entities.Order_Aggregate
{
    public class Order:BaseEntity
    {

        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal,string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string  BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Address ShippingAddress { get; set; }

        // I make the FK Although I don't need it at bussines to make it nullable becouse I will chinge the onDelete to setNull at migration
        public int? DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
                       
        public decimal  SubTotal { get; set; }  // SubTotal of price

        [NotMapped]  //Total of Price
        public decimal Total { get => SubTotal + DeliveryMethod.Cost; }  //اقدر اشيل الاقواس والجت
                                                                         //اقدر اعملها ك فنكشن كده(GetTotal)

        public string PaymentIntentId { get; set; } 

        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;
        //{
        //    return SubTotal+ DeliveryMethod.Cost;
        //}


    }
}
