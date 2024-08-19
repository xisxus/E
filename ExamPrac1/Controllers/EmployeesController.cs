using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExamPrac1.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ExamPrac1.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        

        public EmployeesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        public async Task<IActionResult> AGGIndex()
        {
            var emp = new AggreateVM();

            await using var con = _context.Database.GetDbConnection();
            await con.OpenAsync();

            await using var comm = con.CreateCommand();
            comm.CommandText = "EmpSum";
            comm.CommandType = CommandType.StoredProcedure;

            await using var reader = await comm.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                emp = new AggreateVM()
                {
                    TotalEmployees = reader.GetInt32(0),
                    AvgSalary = reader.GetInt32(1),
                    TotalSalary = reader.GetInt32(2),
                    MinSalary = reader.GetInt32(3),
                    MaxSalary = reader.GetInt32(4),
                };
               
            }
            return View(emp);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {

            return View(new EmpViewModel());
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmpViewModel employee)
        {

            string ImageUrl = null;
            if (employee.ImageFile != null)
            {
                var imgName = Guid.NewGuid().ToString() + ".jpg";
                var filePath = Path.Combine(_env.WebRootPath, "img", imgName);

                using (var fileStrem= new FileStream(filePath, FileMode.Create))
                {
                    await employee.ImageFile.CopyToAsync(fileStrem);
                }

                ImageUrl = "/img/" + imgName;
            }

            var expTable = new DataTable();
            expTable.Columns.Add("Tittle" , typeof(string));
            expTable.Columns.Add("Duration" , typeof(int));

            foreach (var item in employee.Experiences)
            {
                expTable.Rows.Add(item.Tittle, item.Duration);
            }

            var param = new[]
            {
                new SqlParameter("@Name" , employee.Name) ,
                new SqlParameter("@Active" , employee.Active) ,
                new SqlParameter("@JoinDate" , employee.JoinDate) ,
                new SqlParameter("@ImageUrl" , ImageUrl) ,
                new SqlParameter("@Salary" , employee.Salary) ,
                new SqlParameter("@Exp" , expTable) { SqlDbType = SqlDbType.Structured, TypeName = "dbo.exptype2"}
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC InsertSp @Name, @Active ,@JoinDate , @ImageUrl , @Salary , @Exp "  , param);

            return RedirectToAction("Index");
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,Name,Active,JoinDate,ImageUrl,Salary")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
