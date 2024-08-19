namespace ExamPrac1.Models
{
    public class EmpViewModel
    {
        public EmpViewModel()
        {
            Experiences = new List<ExperienceViewModel>();
        }

        public int? EmployeeId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime JoinDate { get; set; }
        public IFormFile ImageFile { get; set; }
        public int Salary { get; set; }

        public  List<ExperienceViewModel> Experiences { get; set; } 
    }
}
