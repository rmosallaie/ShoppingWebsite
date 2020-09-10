using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using ShoppingWebsite.Data;
using ShoppingWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ShoppingWebsite.Pages.ProductsManagement
{
    public class EditProductModel : PageModel
    {
        private readonly ShoppingWebsiteContext context;
        private readonly IWebHostEnvironment iweb;

        public EditProductModel(ShoppingWebsiteContext context, IWebHostEnvironment iweb)
        {
            this.context = context;
            this.iweb = iweb;
        }

        [BindProperty]
        public Product ProductEdit { get; set; } = new Product();

        public List<string> CategoriesList { get; set; } = new List<string>();

        public List<string> BrandsList { get; set; } = new List<string>();

        [BindProperty]
        public IFormFile Photo { get; set; }

        public static string oldPhotoPath = "";

        public void OnGet(int? id)
        {
            string query = "Select * from Product where ProductId = " + id;
            using (SqlConnection connection = new SqlConnection(Globals.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    ProductEdit.ProductId = int.Parse(reader["ProductId"].ToString());
                    ProductEdit.Name = Convert.ToString(reader["ProductName"]);
                    ProductEdit.UnitPrice = Convert.ToDecimal(reader["ProductUnitPrice"]);
                    ProductEdit.Category = Convert.ToString(reader["ProductCategory"]);
                    ProductEdit.Brand = Convert.ToString(reader["ProductBrand"]);
                    ProductEdit.Quantity = int.Parse(reader["ProductQuantity"].ToString());
                    ProductEdit.ProductNumber = Convert.ToString(reader["ProductNumber"]);
                    ProductEdit.ProductPhotoPath = Convert.ToString(reader["ProductPhotoPath"]);
                    oldPhotoPath = ProductEdit.ProductPhotoPath;
                }
                connection.Close();
            }

            using (SqlConnection connection = new SqlConnection(Globals.ConnectionString))
            {
                query = "Select CategoryName from Categories";
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
            string query = "";
            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    System.IO.File.Delete(Path.Combine(iweb.WebRootPath, "images/ProductImages/" + oldPhotoPath));

                    var PhotoPath = ProcessUploadedFile();
                    query = "Update Product set ProductName=N'" + ProductEdit.Name
                                + "',ProductBrand=N'" + ProductEdit.Brand
                                + "',ProductUnitPrice='" + ProductEdit.UnitPrice
                                + "',ProductCategory=N'" + ProductEdit.Category
                                + "',ProductNumber='" + ProductEdit.ProductNumber
                                + "',ProductPhotoPath='" + PhotoPath
                                + "',ProductQuantity='" + ProductEdit.Quantity
                                + "' WHERE ProductId = " + ProductEdit.ProductId;
                }
                else
                {
                    query = "UPDATE Product SET ProductName=N'" + ProductEdit.Name
                                + "',ProductBrand=N'" + ProductEdit.Brand
                                + "',ProductUnitPrice='" + ProductEdit.UnitPrice
                                + "',ProductCategory=N'" + ProductEdit.Category
                                + "',ProductNumber='" + ProductEdit.ProductNumber
                                + "',ProductQuantity='" + ProductEdit.Quantity
                                + "' WHERE ProductId = " + ProductEdit.ProductId;
                }

                using (SqlConnection connection = new SqlConnection(Globals.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToPage("/ProductsManagement/ProductsList");
            }

            ModelState.AddModelError(string.Empty, "Fill the required field");
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

    }
}