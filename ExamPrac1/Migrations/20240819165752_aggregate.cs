using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamPrac1.Migrations
{
    /// <inheritdoc />
    public partial class aggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE or alter PROCEDURE EmpSum
                AS
                BEGIN
                    SELECT 
                        COUNT(*) AS Totale,
                        AVG(Salary) AS Avgs,
                        SUM(Salary) AS Totals,
                        MIN(Salary) AS Mins,
                        MAX(Salary) AS Maxs
                    FROM 
                        Employees;
                END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROECDURE IF EXISTS EmpSum");
        }
    }
}
