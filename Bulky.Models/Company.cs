using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required]
        public string? CompanyName { get; set; }

        public string? CompanyAddress { get; set;}

        public string? CompanyCity { get; set; }

        public string? CompanyState { get; set;}

        public string? CompanyPostalCode { get; set;}

        public string? CompanyPhoneNumber { get; set;}
    }
}
