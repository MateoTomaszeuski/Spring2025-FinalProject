using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consilium.Shared.Models;

public class Assignment {
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public bool IsCompleted { get; set; }
    public int CourseId { get; set; }
}