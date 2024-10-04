using Microsoft.AspNetCore.Mvc;
using NotesApi.Data;
using NotesApi.Models;

namespace NotesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        private readonly DBNotesContext db;
        private readonly ILogger<NotesController> _logger;

        public NotesController(DBNotesContext _db, ILogger<NotesController> logger)
        {
            db = _db;
            _logger = logger;   
        }
        [HttpGet]
        [Route("GetAllNotes")]
        public ActionResult GetAllNotes()
        {
            try
            {
                var res = db.Notes.ToList();
                return Ok(res);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);

            }
        }

        [HttpGet]
        [Route("GetNotesById{id:Guid}")]
        [ActionName("GetNotesById")]
        public ActionResult GetNotesById([FromRoute] Guid id)
        {
            try
            {
                _logger.LogInformation("before db");
                var res = db.Notes.Find(id);
                _logger.LogInformation("after db");
                if (res == null)
                {
                    return NotFound();
                }
                return Ok(res);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);

            }
        }

        [HttpPost]
        [Route("AddNotes")]
        public ActionResult AddNotes(Note note)
        {
            note.Id = Guid.NewGuid();
            db.Notes.Add(note);
            db.SaveChanges();
            return CreatedAtAction(nameof(GetNotesById), new { id = note.Id }, note);
        }


        [HttpPut]
        [Route("EditNotes{id:Guid}")]
        public ActionResult EditNotes([FromRoute] Guid id, [FromBody] Note EditedNote)
        {
            if (id == EditedNote.Id)
            {
                try
                {
                    db.Notes.Update(EditedNote);
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("DeleteNotes{id:Guid}")]
        public ActionResult DeleteNotes([FromRoute] Guid id)
        {

            if (id != Guid.Empty)
            {
                var deletedNote = db.Notes.Find(id);
                try
                {
                    db.Notes.Remove(deletedNote);
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return Ok();
            }
            return NotFound();
        }
    }
}
