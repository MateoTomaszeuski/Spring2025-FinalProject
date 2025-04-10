﻿using Consilium.Shared.Models;

namespace Consilium.Shared.Services;

public interface IPersistenceService {
    TodoList? GetToDoLists();
    void SaveList(TodoList list);
    void SaveToken(string email, string token);
    void OnStartup();
}