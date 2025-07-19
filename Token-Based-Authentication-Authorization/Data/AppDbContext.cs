using Microsoft.EntityFrameworkCore;

namespace Token_Based_Authentication_Authorization.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }

    }
}