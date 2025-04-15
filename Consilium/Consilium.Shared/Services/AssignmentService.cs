using Consilium.Shared.Models;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Net.Http.Json;

namespace Consilium.Shared.Services;

public class AssignmentService(IClientService clientService) : IAssignmentService {

    public List<Assignment> AllAssignments { get; set; } = new();

    public Task AddAssignmentAsync(Assignment a) {
        throw new NotImplementedException();    
    }

    public Task DeleteAssignmentAsync(int assignmentId) {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Assignment>> GetAllAssignmentsAsync() {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Course>> GetAllCourses() {
        throw new NotImplementedException();
    }

    public async Task UpdateAssignment(Assignment a) {
        await clientService.PatchAsync("assignment", a);
    }
}