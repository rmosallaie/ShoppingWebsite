using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShoppingWebsite.Data;
using ShoppingWebsite.Models;

namespace ShoppingWebsite.Pages.ProductsManagement
{
    public class ProductsListModel : PageModel
    {
        private readonly ShoppingWebsiteContext context;

        public ProductsListModel(ShoppingWebsiteContext context)
        {
            this.context = context;
        }

        public List<Product> products { get; set; } = new List<Product>();

        public Product ProductItem { get; set; } = new Product();

        public void OnGet()
        {
            string query = "Select * from product";
            using (SqlConnection connection = new SqlConnection(Globals.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        //ProductItem.ProductId = int.Parse(reader["ProductId"].ToString());
                        //ProductItem.Name = Convert.ToString(reader["ProductName"]);
                        //ProductItem.UnitPrice = Convert.ToDecimal(reader["ProductUnitPrice"]);
                        //ProductItem.Category = Convert.ToString(reader["ProductCategory"]);
                        //ProductItem.Brand = Convert.ToString(reader["ProductBrand"]);
                        //ProductItem.Quantity = int.Parse(reader["ProductQuantity"].ToString());
                        //ProductItem.ProductNumber = Convert.ToString(reader["ProductNumber"]);
                        products.Add(new Product() 
                        {
                            ProductId = int.Parse(reader["ProductId"].ToString()),
                            Name = Convert.ToString(reader["ProductName"]),
                            UnitPrice = Convert.ToDecimal(reader["ProductUnitPrice"]),
                            Category = Convert.ToString(reader["ProductCategory"]),
                            Brand = Convert.ToString(reader["ProductBrand"]),
                            Quantity = int.Parse(reader["ProductQuantity"].ToString()),
                            ProductNumber = Convert.ToString(reader["ProductNumber"])
                        });
                    }
                    reader.Close();
                }
                connection.Close();
            }
        }
    }
}