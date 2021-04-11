using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MercyProject.ViewModel
{
    public class ViewDetails
    {
        public int Id { get; set; }
        [Required, MaxLength(18, ErrorMessage = "name cannot exceed 18 characters")]
        public string Name { get; set; }
        [Required]
        public string Photo { get; set; }

    }
}
