using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MercyProject.ViewModel
{
    public class Products
    {
        public int Id { get; set; }
        [Required, MaxLength(18, ErrorMessage = "name cannot exceed 18 characters")]
        public string Name { get; set; }
        [Required,DisplayName("Upload File")]
        public byte Photo { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile Image { get; set; }
    }
}
