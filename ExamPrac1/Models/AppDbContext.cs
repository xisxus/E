using Microsoft.EntityFrameworkCore;

namespace ExamPrac1.Models
{
    public class AppDbContext  : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Experience> Experiences { get; set; }

        //public async Task<AggreateVM> getEmp()
        //{
        //    var sql = "EXEC EmpSum";

        //    // Use `FromSqlRaw` to execute the stored procedure
        //    var result = await this.Set<AggreateVM>()
        //        .FromSqlRaw(sql)
        //        .AsNoTracking()
        //    .FirstOrDefaultAsync(); // Use AsSingleResultAsync for a single result

        //    return result;
        //}
    }
}
