using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using ShoppingWebsite.Data;
using ShoppingWebsite.Models;

namespace ShoppingWebsite.Pages
{
    public class AddProductModel : PageModel
    {
        private readonly ShoppingWebsiteContext context;
        private readonly IWebHostEnvironment iweb;

        public AddProductModel(ShoppingWebsiteContext context, IWebHostEnvironment iweb)
        {
            this.context = context;
            this.iweb = iweb;
        }

        [BindProperty]
        public Product NewProduct { get; set; }

        //public IList<Image> Imagesave { get; set; }

        public List<string> CategoriesList { get; set; } = new List<string>();

        public List<string> BrandsList { get; set; } = new List<string>();

        [BindProperty]
        [Required]
        public IFormFile Photo { get; set; }

        string cs = Globals.ConnectionString;
        

        public void OnGet()
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                string query = "Select CategoryName from Categories";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        CategoriesList.Add(Convert.ToString(reader["CategoryName"]));
                    }
                    reader.Close();
                    connection.Close();
                }

                query = "Select BrandName from Brands";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        BrandsList.Add(Convert.ToString(reader["BrandName"]));
                    }
                    reader.Close();
                    connection.Close();
                }
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Fill the required field");
                return Page();
            }

            var PhotoPath = ProcessUploadedFile();

            using (SqlConnection connection = new SqlConnection(cs))
            {
                string query = "INSERT INTO product(ProductName,ProductBrand,ProductUnitPrice,ProductCategory,ProductNumber,ProductPhotoPath,ProductQuantity)" +
                    "VALUES(N'" + NewProduct.Name + "',N'" + NewProduct.Brand + "'," + NewProduct.UnitPrice + ",N'" + NewProduct.Category + "','"
                    + NewProduct.ProductNumber + "','" + PhotoPath + "','" + NewProduct.Quantity + "')";
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    int x = command.ExecuteNonQuery();
                    if (x != 0)
                    {
                        return RedirectToPage("AddProductConfirmation");
                    }
                }
            }

            return Page();
        }


        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;

            if (Photo != null)
            {
                string uploadsFolder = Path.Combine(iweb.WebRootPath, "images/ProductImages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        public IActionResult OnGetCheckPN(string pn)
        {
            int resutl;
            using (SqlConnection connection = new SqlConnection(cs))
            {
                string query = "select * from Products where ProductNumber = '" + pn + "'";
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    resutl = command.ExecuteNonQuery();
                }
            }

            var json = new JsonResult(resutl);
            return json;
        }

        
    }
}