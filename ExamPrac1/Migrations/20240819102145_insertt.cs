using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamPrac1.Migrations
{
    /// <inheritdoc />
    public partial class insertt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

                Create type exptype2 as table 
                ( Tittle nvarchar(max),
	                Duration int
	                )

                go 

                Create or alter Procedure InsertSp
                @Name nvarchar(max),
                @Active bit,
                @JoinDate datetime,
                @ImageUrl nvarchar(max),
                @Salary int,
                @Exp exptype2 readonly

                as
                begin
	                set nocount on ;

	                insert into Employees
	                values (@Name ,@Active,@JoinDate,@ImageUrl ,@Salary)

	                Declare @EmpId int ;

	                set @EmpId = SCOPE_IDENTITY();

	                insert into Experiences (EmployeeId, Tittle , Duration)
	                Select @EmpId, Tittle, Duration from @Exp
                end");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS InsertSp");
        }
    }
}
