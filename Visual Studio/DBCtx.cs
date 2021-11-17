using Courses.Models;
using Microsoft.EntityFrameworkCore;

namespace Courses
{
    public class DBCtx : DbContext
    {
        public DBCtx(DbContextOptions<DBCtx> options) : base(options)
        {
        }

        public DbSet<Models.Courses> Courses { get; set; }
        public DbSet<Modules> Modules { get; set; }
    }
}