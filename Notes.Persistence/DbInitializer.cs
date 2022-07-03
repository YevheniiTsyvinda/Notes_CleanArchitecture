
using Notes.Application.Interfaces;

namespace Notes.Persistence;

public class DbInitializer
{
    public static void Initialize(NotesDbContext context)
    {
        context.Database.EnsureCreated();
    }
}
