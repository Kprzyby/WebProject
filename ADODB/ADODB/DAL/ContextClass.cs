using Microsoft.EntityFrameworkCore;
using ADODB.Models;

namespace ADODB.DAL
{
    public class ContextClass : DbContext
    {
        public ContextClass(DbContextOptions<ContextClass> options) : base(options)
        {

        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}
