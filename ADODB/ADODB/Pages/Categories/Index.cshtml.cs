using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ADODB.Models;
using ADODB.DAL;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ADODB.Pages.Categories
{
    public class IndexModel : PageModel
    {
        public IndexModel(IConfiguration configuration, ContextClass context)
        {
            _configuration = configuration;
            this.context=context;
            CategoryList = new List<Category>();
            ProductValues = new List<decimal>();
            ProductCounts = new List<int>();
        }

        private readonly IConfiguration _configuration;
        private readonly ContextClass context;
        public List<Category> CategoryList;

        //a list of total values of products for each category
        public List<decimal> ProductValues;

        //a list of the number of products in each category
        public List<int> ProductCounts;
        public void OnGet()
        {
            CategoryList = context.Category.ToList<Category>();

            //a LINQ query to get the number of products in each category
            var counts = from result in
                         (from category in context.Category
                          join product in context.Product
                          on category.Id equals product.CategoryId into joined
                          from processedJoined in joined.DefaultIfEmpty()
                          select new
                          {
                              Id = category.Id,
                              HasProduct = (processedJoined != default) ? 1 : 0
                          })
                         group result by result.Id into groupped
                         select new
                         {
                             Count = groupped.Sum(e => e.HasProduct)
                         };

            foreach(var count in counts)
            {
                ProductCounts.Add((int)count.Count);
            }

            //a SQL query to get the total value of products in each category
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyCompanyDB")))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = @"Select sum(isnull(price,0))" +
                    " from Product P right join Category C on P.CategoryId=C.Id" +
                    " Group by C.Id";

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (rd.HasRows)
                    {
                        while(rd.Read())
                        {
                            ProductValues.Add((decimal)rd[0]);
                        }
                    }
                }
            }
        }
        
    }
}
