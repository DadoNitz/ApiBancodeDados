using System.Collections.Generic;
using System.Linq;
using apibase.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apibase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruitmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecruitmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Recruitment>> GetRecruitments()
        {
            return _context.Recruitment.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Recruitment> GetRecruitment(int id)
        {
            var recruitment = _context.Recruitment.Find(id);

            if (recruitment == null)
            {
                return NotFound();
            }

            return recruitment;
        }

        [HttpPost]
        public ActionResult<Recruitment> PostRecruitment(Recruitment recruitment)
        {
            _context.Recruitment.Add(recruitment);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetRecruitment), new { id = recruitment.ID }, recruitment);
        }

        [HttpPut("{id}")]
        public IActionResult PutRecruitment(int id, Recruitment updatedRecruitment)
        {
            var existingRecruitment = _context.Recruitment.Find(id);
            if (existingRecruitment == null)
            {
                return NotFound();
            }

            if (id != updatedRecruitment.ID)
            {
                return BadRequest();
            }

            existingRecruitment.Exportador = updatedRecruitment.Exportador;
            existingRecruitment.Importador = updatedRecruitment.Importador;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status409Conflict, "A entidade foi modificada por outra operação.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Recruitment> DeleteRecruitment(int id)
        {
            var recruitment = _context.Recruitment.Find(id);
            if (recruitment == null)
            {
                return NotFound();
            }

            _context.Recruitment.Remove(recruitment);
            _context.SaveChanges();

            return recruitment;
        }
    }
}
