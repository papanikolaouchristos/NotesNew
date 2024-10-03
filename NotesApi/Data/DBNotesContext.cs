using Microsoft.EntityFrameworkCore;
using NotesApi.Models;

namespace NotesApi.Data
{
    public class DBNotesContext : DbContext
    {
        public DBNotesContext(DbContextOptions<DBNotesContext> options) : base(options)
        {

        }

        public virtual DbSet<Note> Notes { get; set; }
    }
}
