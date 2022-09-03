using ShopApp.entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.webui.Models
{
    public class ProductModel //form için ayrı model oluşturduk 
    {
        public int ProductId { get; set; }
        [Display(Name = "Name" , Prompt ="Enter Product Name")]
        [Required(ErrorMessage ="Name zorunlu bir alan.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price zorunlu bir alan.")]
        [Range(500,80000,ErrorMessage ="500 ile 80000 arası bir değer giriniz.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Url zorunlu bir alan.")]

        
        public string Url { get; set; }
        [Required(ErrorMessage ="Description zorunlu bir alan.")]
        [StringLength(100,MinimumLength =5,ErrorMessage ="5-100 karakter arası giriniz")]
        public string Description { get; set; }

        public bool isApproved { get; set; }
        public bool isHome { get; set; }
        [Required(ErrorMessage = "ImageUrl zorunlu bir alan.")]

        public string ImageUrl { get; set; }
        public List<Category> SelectedCategories { get; set; }
         

    }
}
