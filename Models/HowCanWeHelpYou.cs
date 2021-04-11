using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MercyProject.Models
{
    public class HowCanWeHelpYou
    {
        public int Id { get; set; }
        [DataType(DataType.Text), Required, DisplayName("First Name"), MaxLength(20, ErrorMessage = "Name cannot be more than 20 character")]
        public string FirstName { get; set; }
        [DataType(DataType.Text), Required, DisplayName("Last Name"), MaxLength(20, ErrorMessage = "Name cannot be more than 20 character")]
        public string LastName { get; set; }
        [DataType(DataType.PhoneNumber), Display(Name = "Phone Number"), MaxLength(14, ErrorMessage = "Your Phone Number cannot exceed 11 digits"), Required]
        public string PhoneNumber { get; set; }
        [DataType(DataType.EmailAddress), Display(Name = "Email Address"), Required]
        public string Email { get; set; }
        [DataType(DataType.MultilineText), MaxLength(150, ErrorMessage ="Your Message should not be more than 150 words")]
        public string YourMessage { get; set; }
        [Required, /*DisplayFormat(DataFormatString ="{0:dd/mm/year}"),*/ DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
