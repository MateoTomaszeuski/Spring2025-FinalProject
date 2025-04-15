using Consilium.Shared.Models;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Net.Http.Json;

namespace Consilium.Shared.Services;

//public class AssignmentService(ClientService clientService) : IAssignmentService {
public class AssignmentService : IAssignmentService {

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

    public Task UpdateAssignment(Assignment a) {
        throw new NotImplementedException();
    }
}