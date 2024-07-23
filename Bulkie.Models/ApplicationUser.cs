using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkie.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public int? CompanyId { get; set; } // Navigation property
        [ForeignKey("CompanyId")]
        [ValidateNever]
        public Company? Company { get; set; }
        /*Add a column here, but we do not want to add that to the Database.*/
        [NotMapped] //Will make sure that Role is not post to the Database
        public string Role {  get; set; }
    }
}
