using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppleStore.Models
{
    public class Order
    {
        public int Id { get; set; }

        
        public string UserId { get; set; }       
       
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }

        public string Email_Order { get; set; }
        public string PhoneNumber_Order { get; set; }

        public string FullName_Order { get; set; }
        public string ShippingAddress { get; set; }
        public string Notes { get; set; }
      
        public string? Vnpay_transaction { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser applicationUser { get; set; }   
       
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
