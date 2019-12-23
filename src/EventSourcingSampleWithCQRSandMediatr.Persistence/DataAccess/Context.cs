using EventSourcingSampleWithCQRSandMediatr.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventSourcingSampleWithCQRSandMediatr.Persistence.DataAccess
{
    public class Context : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Faul> Fauls { get; set; }
        public DbSet<Card> Cards { get; set; }


        public int MyProperty { get; set; }
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

    }
}
