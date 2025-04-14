using Consilium.Shared.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace Consilium.Shared.Services;

public class AssignmentService : IAssignmentService {
    private readonly HttpClient client;

    public AssignmentService(IHttpClientFactory factory) {
        client = factory.CreateClient("ApiClient");
        client.DefaultRequestHeaders.Add("Consilium-User", "password");
    }

    public void AddAssignment(Assignment a, string email) {
        string addAssignment = """"
            INSERT INTO assignment
            (course_id, assignment_name, assignment_description, due_date, mark_started, mark_complete)
            VALUES (@course_id, @assignment_name, @assignment_description, @due_date, @mark_started, @mark_complete);
            """";
    }

    //public async Task<ObservableCollection<Assignment>>
}