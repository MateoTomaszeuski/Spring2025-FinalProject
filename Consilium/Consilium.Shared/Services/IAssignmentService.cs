using Consilium.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consilium.Shared.Services;

interface IAssignmentService {
    Task AddAssignmentAsync(Assignment a);
    Task DeleteAssignmentAsync(int assignmentId);
    Task<IEnumerable<Assignment>> GetAllAssignmentsAsync();
    Task UpdateAssignment(Assignment a);
}