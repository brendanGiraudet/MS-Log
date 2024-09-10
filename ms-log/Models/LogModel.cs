using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ms_log.Models;

[Table("Logs")]
public record LogModel
{
    [Key]
    public Guid Id { get; set; }

    public required string Level { get; set; }
    
    public required string Message { get; set; }
    
    public required DateTime Timestamp { get; set; }
    
    public required Guid ApplicationCode { get; set; }
    
    public required bool Deleted { get; set; }
}
