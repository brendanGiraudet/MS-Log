using Microsoft.EntityFrameworkCore;
using ms_log.Models;

namespace ms_log.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<LogModel> Logs { get; set; }
}
