using Microsoft.EntityFrameworkCore;
 
namespace Wedding_Planner.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        public DbSet<MainUser> user { get; set; }
        public DbSet<Wedding> wedding { get; set; }
        public DbSet<Guest> guest { get; set; }

    }
}