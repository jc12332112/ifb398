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
            if (!_context.Students.Any())
            {
                return NotFound("No students found.");
            }

            double averageGrade = await _context.Students.AverageAsync(s => s.grade);
            return averageGrade; 
        }

        [HttpGet("grades")]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudentGrades()
        {
            var students = await _context.Students.ToListAsync();
            return Ok(students);
        }

        [HttpGet("{id}/overall-grade")]
        public async Task<ActionResult<double>> CalculateOverallGrade(int id)
        {
            var grades = await _context.Students
                .Where(g => g.Id == id)
                .ToListAsync();

            if (grades == null || !grades.Any())
            {
                return NotFound("No grades found for this student.");
            }

            var overallGrade = grades.Average(g => g.grade);
            return Ok(overallGrade);
        }


    }


}

