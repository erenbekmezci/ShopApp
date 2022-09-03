using ShopApp.entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.webui.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage ="Name zorunlu bir alan.")]
        [Display(Name = "Name" , Prompt = "Enter Category Name")]
        [StringLength(100 , MinimumLength =5 , ErrorMessage ="5 - 100 arası karakter giriniz.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Url zorunlu bir alan.")]
        public string Url { get; set; }

        public List<Product> Products { get; set; }
    }
}
