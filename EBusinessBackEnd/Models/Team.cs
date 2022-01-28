using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EBusinessBackEnd.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50),MinLength(3),Required]
        public string FullName { get; set; }

        [MaxLength(250)]

        public string Image { get; set; }
        [NotMapped]

        public IFormFile ImageFile { get; set; }

        [MaxLength(50),MinLength(3),Required]
        public string Job { get; set; }

        [ForeignKey("Social")]
        public int SocialId { get; set; }

        public Social Social { get; set; }


    }
}
