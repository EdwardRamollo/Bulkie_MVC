using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkie.Models.ViewModels
{

    /* View Models are models specifically designed for a View. The advantage that we get because of that, is that the view will
       be strongly typed to one model. View models are also known as strongly typed views.
    
       If Someone will ask you 'what is a strongly typed view?' -- that specifically means there is a model that is specific for that 
       view, and because of that the view is strongly typed to a model 
     */
    public  class ProductVM
    {
        public Product Product { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
