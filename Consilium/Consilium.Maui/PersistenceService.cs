using Consilium.Shared.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace Consilium.Shared.Services;

public class PersistenceService(IClientService clientService) : IPersistenceService {
    public TodoList? GetToDoLists() {
        string output = Preferences.Get("todo-list", "{}");
        return JsonSerializer.Deserialize<TodoList>(output);
    }

    public void SaveList(TodoList list) {
        string serialized = JsonSerializer.Serialize(list);
        Preferences.Set("todo-list", serialized);
    }

    public void SaveToken(string email, string token) {
        Preferences.Set("auth-header-email", email);
        Preferences.Set("auth-header-token", token);

        clientService.UpdateHeaders(email, token);
    }

    public void DeleteToken() {
        Preferences.Set("auth-header-email", string.Empty);
        Preferences.Set("auth-header-token", string.Empty);
        clientService.UpdateHeaders(string.Empty, string.Empty);

    }

    public void OnStartup() {
        string email = Preferences.Get("auth-header-email", String.Empty);
        string token = Preferences.Get("auth-header-token", String.Empty);
        clientService.UpdateHeaders(email, token);
    }
    public string GetUserName() {
        return Preferences.Get("auth-header-email", String.Empty);
    }
    public async Task<bool> CheckAuthStatus() {

        var response = await clientService.GetAsync("/account/valid");
        return response.IsSuccessStatusCode;

    }
    public bool CheckLoginStatus() {
        string email = Preferences.Get("auth-header-email", String.Empty);
        string token = Preferences.Get("auth-header-token", String.Empty);
        return !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(token);
    }
}