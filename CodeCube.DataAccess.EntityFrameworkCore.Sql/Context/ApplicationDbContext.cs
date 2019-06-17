using Microsoft.EntityFrameworkCore;

namespace CodeCube.DataAccess.EntityFrameworkCore.Sql.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
    }
}