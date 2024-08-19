namespace ExamPrac1.Models
{
    public class Experience
    {
        public int ExperienceId { get; set; }
        public int EmployeeId { get; set; }
        public string Tittle { get; set; }
        public int Duration { get; set; }
        public virtual Employee Employee { get; set; }
    }
}