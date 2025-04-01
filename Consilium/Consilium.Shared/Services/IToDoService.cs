using Consilium.Shared.Models;
using System.Collections.ObjectModel;

namespace Consilium.Shared.Services;
public interface IToDoService {
    ObservableCollection<TodoItem> GetTodoItemsAsync();
    Task<string> RemoveToDoAsync(int itemIndex);
    Task InitializeTodosAsync();
}