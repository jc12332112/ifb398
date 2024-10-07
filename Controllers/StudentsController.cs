using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;


using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;


namespace StudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            // Ensure the grades are correctly linked to the student
            if (student.Grades != null && student.Grades.Any())
            {
                foreach (var grade in student.Grades)
                {
                    grade.Student = student;  // Link each grade to the student being created
                }
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }




        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return student;
        }
        [HttpGet("AverageGrade")]
        public async Task<ActionResult<double>> GetAverageGrade()
        {
            var students = await _context.Students
                .Include(s => s.Grades)
                .ToListAsync();

            if (!students.Any())
            {
                return NotFound("No students found.");
            }

            double overallAverage = students
                .Where(s => s.Grades.Any())
                .Average(s => s.Grades.Average(g => g.GradeValue));

            return Ok(overallAverage);
        }


        [HttpGet("grades")]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudentGrades()
        {
            var students = await _context.Students
                .Include(s => s.Grades)  // This includes each student's grades
                .ToListAsync();

            return Ok(students);
        }


        [HttpGet("{id}/overall-grade")]
        public async Task<ActionResult<double>> CalculateOverallGrade(int id)
        {
            // Fetch grades for the student from the Grade table
            var grades = await _context.Grades
                .Where(g => g.StudentId == id)
                .ToListAsync();

            if (grades == null || !grades.Any())
            {
                return NotFound("No grades found for this student.");
            }

            var overallGrade = grades.Average(g => g.GradeValue);
            return Ok(overallGrade);
        }
        [HttpGet("average-overall-grade")]
public async Task<ActionResult<double>> GetAverageOverallGrade()
{
    var students = await _context.Students
        .Include(s => s.Grades)
        .ToListAsync();

    if (!students.Any() || students.All(s => !s.Grades.Any()))
    {
        return NotFound("No students with grades found.");
    }

   
    double overallAverage = students
        .Where(s => s.Grades.Any()) 
        .SelectMany(s => s.Grades)  
        .Average(g => g.GradeValue);

    return Ok(overallAverage);
}




    }


}

