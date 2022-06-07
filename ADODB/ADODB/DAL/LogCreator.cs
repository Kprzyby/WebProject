using System.Text.Json;
using ADODB.Models;

namespace ADODB.DAL
{
    public class LogCreator : ILogs
    {
        //method that returns a string including details about a product passed as an argument
        private string GetProductDetails(Product product)
        {
            string details= "id: " + product.id + "\n" +
                        "name: " + product.name + "\n" +
                        "price: " + product.price + "\n" +
                        "category id: " + product.CategoryId;

            return details;
        }
        //method that returns a string including details about a category passed as an argument
        private string GetCategoryDetails(Category category)
        {
            string details = "id: " + category.Id + "\n" +
                        "short name: " + category.shortName + "\n" +
                        "full name: " + category.longName;

            return details;
        }
        //returns the content of the log message depending on the type of the operation included in
        //the "logType" argument
        private string GetMessage(string logType, string details)
        {
            string message=default;

            if(logType=="CreateUser")
            {
                NewUser user = JsonSerializer.Deserialize<NewUser>(details);
                message = "User " + user.UserName + " (role - " + user.role + ") has been created";
                return message;
            }
            else if (logType == "LogIn") return "User " + details + " logged in.";
            else if (logType == "LogOut") return "User logged out.";
            else
            {
                Product product=new Product();
                string productDetails=null;
                Category category=new Category();
                string categoryDetails = null;

                if (logType=="EditProduct" || logType=="CreateProduct" || logType=="DeleteProduct")
                {
                    product = JsonSerializer.Deserialize<Product>(details);
                    productDetails = GetProductDetails(product);
                }
                else if (logType == "EditCategory" || logType == "CreateCategory" || logType == "DeleteCategory")
                {
                    category = JsonSerializer.Deserialize<Category>(details);
                    categoryDetails = GetCategoryDetails(category);
                }

                if(logType=="EditProduct")
                {
                    message = "A product with id="+product.id+" has been modified." +
                        " Updated product details:\n" + productDetails;
                }
                else if (logType == "CreateProduct")
                {
                    message = "A new product has been created. Product details:\n" + productDetails;
                }
                else if (logType == "DeleteProduct")
                {
                    message = "A product with id=" + product.id + " has been deleted.";
                }
                else if (logType == "EditCategory")
                {
                    message = "A category with id=" + category.Id + " has been modified." +
                        " Updated category details:\n" + categoryDetails;
                }
                else if (logType == "CreateCategory")
                {
                    message = "A new category has been created. Category details:\n" + categoryDetails;
                }
                else if (logType == "DeleteCategory")
                {
                    message = "A category with id=" + category.Id + " has been deleted.";
                }
            }

            return message;
        }
        public void CreateLog(string logType, string details)
        {
            string message = GetMessage(logType, details);

            using (StreamWriter writer = new StreamWriter("wwwroot/files/Logs.txt", append: true))
            {
                writer.WriteLine(message);
            }
            
        }
    }
}
