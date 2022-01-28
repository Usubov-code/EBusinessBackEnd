using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EBusinessBackEnd.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50),Required]
        public string Phone { get; set; }
        [MaxLength(50), Required]
        public string Email { get; set; }
    }
}
