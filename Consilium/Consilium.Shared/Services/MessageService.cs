using Consilium.Shared.Models;
using Consilium.Shared.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace Consilium.Shared.Services;

public class MessageService : IMessageService {
    private readonly IClientService client;

    public MessageService(IClientService client) {
        this.client = client;
    }
    public async Task<IEnumerable<string>> GetConversations() {
        var response = await client.GetAsync($"/messages/all");
        return await response.Content.ReadFromJsonAsync<ObservableCollection<string>>() ?? new ObservableCollection<string>();
    }

    public async Task<IEnumerable<Message>> InitializeMessagesAsync(string otherUser) {
        var response = await client.GetAsync($"/messages/{otherUser}");
        return await response.Content.ReadFromJsonAsync<ObservableCollection<Message>>() ?? new ObservableCollection<Message>();
    }

    public async Task<bool> SendMessageAsync(Message message) {
        var response = await client.PostAsync($"/messages", message);
        return response.IsSuccessStatusCode;
    }
}