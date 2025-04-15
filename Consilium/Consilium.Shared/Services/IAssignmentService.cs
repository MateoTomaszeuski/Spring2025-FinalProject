using Consilium.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consilium.Shared.Services;

public interface IAssignmentService {
    List<Assignment> AllAssignments { get; set; }
    Task<IEnumerable<Course>> GetAllCourses();
    Task AddAssignmentAsync(Assignment a);
    Task DeleteAssignmentAsync(int assignmentId);
    Task<IEnumerable<Assignment>> GetAllAssignmentsAsync();
    Task UpdateAssignment(Assignment a);
}