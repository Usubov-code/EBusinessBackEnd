using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EBusinessBackEnd.Models
{
    public class Social
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(250), MinLength(3), Required]
        public string Link { get; set; }
        [MaxLength(250), MinLength(3), Required]
        public string Icon { get; set; }

        public List<Team> Teams { get; set; }
    }
}
