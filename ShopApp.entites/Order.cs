using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.entites
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }

        public string UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public string PaymentId { get; set; }
        public string ConversationId { get; set; }

        public enumOrderState OrderState { get; set; }
        public enumPaymentType PaymentType { get; set; }

        public List<OrderItem> OrderItems { get; set; }


    }
    public enum enumPaymentType
    {
        CreditCard= 0,
        Eft = 1

    }
    public enum enumOrderState
    {
        waiting = 0,
        unpaid = 1,
        completed = 2
    }
}
