using Consilium.Shared.Models;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Net.Http.Json;

namespace Consilium.Shared.Services;

public class AssignmentService : IAssignmentService {
    private readonly HttpClient client;

    public AssignmentService(IHttpClientFactory factory) {
        client = factory.CreateClient("ApiClient");
        client.DefaultRequestHeaders.Add("Consilium-User", "password");
    }



    //public async Task<ObservableCollection<Assignment>>
}