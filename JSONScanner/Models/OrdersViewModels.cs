using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JSONScanner.Models
{
    public class OrdersViewModels
    {
        public string toppings { get; set; }
        [Display(Name = "Number of times ordered")]
        public int OrderCount { get; set; }
    }
}