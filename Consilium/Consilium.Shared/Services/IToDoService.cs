﻿using Consilium.Shared.Models;
using System.Collections.ObjectModel;

namespace Consilium.Shared.Services;
public interface IToDoService {
    ObservableCollection<TodoItem> GetTodoItems();
    Task<string> RemoveToDoAsync(int itemIndex);
    Task InitializeTodosAsync();
    Task AddItemAsync(TodoItem item);
    Task UpdateItemAsync(TodoItem item);
}