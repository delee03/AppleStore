using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppleStore.Models
{
    public class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Order = new HashSet<Order>();
        }
    
        [Key]
        public int user_id { get; set; }

        [Required]
        [StringLength(50)]
        public string user_fullname { get; set; }

        [Required]
        [StringLength(50)]
        public string user_email { get; set; }

        [StringLength(50)]
        public string user_password { get; set; }

        [StringLength(15)]
        public string user_phone_number { get; set; }

        [Column(TypeName = "text")]
        public string user_address { get; set; }

        [StringLength(10)]
        public string user_login_with { get; set; }

        public DateTime? user_created_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }
    }
}
