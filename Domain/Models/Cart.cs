using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
	public class Cart
	{
        [Key]
        public int CartId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastUpdatedDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }

    }
}

