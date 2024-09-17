using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ms_log.Models;

[Table("Logs")]
public record LogModel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string Level { get; set; }
    
    public required string Message { get; set; }
    
    public required string Payload { get; set; }
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    public required string ApplicationName { get; set; }
    
    public bool Deleted { get; set; } = false;
}
