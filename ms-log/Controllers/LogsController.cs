using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ms_log.Data;
using ms_log.Models;

namespace ms_log.Controllers;

public class LogsController(DatabaseContext context) : ODataController
{
    private readonly DatabaseContext _context = context;

    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_context.Logs);
    }

    [EnableQuery]
    public IActionResult Get([FromODataUri] Guid key)
    {
        var config = _context.Logs.FirstOrDefault(c => c.Id == key);
        if (config == null)
        {
            return NotFound();
        }
        return Ok(config);
    }

    [HttpPost]
    public IActionResult Post([FromBody] LogModel log)
    {
        _context.Logs.Add(log);
        _context.SaveChanges();

        return Created(log);
    }
}
