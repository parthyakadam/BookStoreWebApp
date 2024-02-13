using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models
{
    public class ApplicationUser : IdentityUser
    {
        //here the ApplicationUser now has all the properties of IdentityUser and here we can now add more properties that we require. We'll be able to identify the user like Customer, Company form below properties

        [Required]
        public string? Name { get; set; }

        public string? StreetAddress { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? PostalCode { get; set; }

        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [ValidateNever]
        public Company Company { get; set; }
        //above foreign key relation is added to check if the added user is Company user or not. this data won't be populated on view
    }
}