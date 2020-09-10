using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http;

namespace ShoppingWebsite.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "شماره کالا وارد نشده است")]
        [Display(Name = "شماره کالا")]
        public string ProductNumber { get; set; }

        [Required(ErrorMessage = "نام کالا وارد نشده است")]
        [MaxLength(30, ErrorMessage = "نام کالا نباید بیشتر از 30 حرف باشد")]
        [Display(Name = "نام کالا")]
        public string Name { get; set; }

        [Required(ErrorMessage = "قیمت واحد کالا وارد نشده است")]
        [Display(Name = "قیمت")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "دسته بندی کالا انتخاب نشده است")]
        [Display(Name = "دسته بندی")]
        public string Category { get; set; }

        [Required(ErrorMessage = "برند کالا انتخاب نشده است")]
        [Display(Name = "برند")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "تعداد کالا انتخاب نشده است")]
        [Display(Name="تعداد موجود")]
        public int Quantity { get; set; }

        [Display(Name = "مسیر عکس کالا")]
        public string ProductPhotoPath { get; set; }
    }
}
