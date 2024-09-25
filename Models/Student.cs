using System.Diagnostics;

namespace WebApplication1.Models
{
    public class Student
    {

        public int Id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();


    }

    public class Grade
    {
        public int Id { get; set; }  // Primary Key
        public int StudentId { get; set; }  // Foreign Key
        public string Subject { get; set; }
        public double GradeValue { get; set; }  // Grade for the subject

        // Navigation property to link back to the student
        public Student Student { get; set; }
    }



}
