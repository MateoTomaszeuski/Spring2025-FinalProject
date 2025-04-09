using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consilium.Shared.Models;

public class Assignment {
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int CourseId { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? DateStarted { get; set; }
    public DateTime? DateCompleted { get; set; }
}