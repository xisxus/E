namespace ExamPrac1.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime JoinDate { get; set; }
        public string ImageUrl { get; set; }
        public int Salary { get; set; }

        public virtual ICollection<Experience> Experiences { get; set;} = new List<Experience>();
    }
}
