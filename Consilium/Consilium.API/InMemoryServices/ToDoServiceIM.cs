
using Consilium.Shared.ViewModels;

namespace Consilium.API.InMemoryServices;

public class ToDoServiceIM : IToDoService {
    private List<TodoItem> tasks = new();
    public void AddTask(TodoItem task) {
        tasks.Add(task);
    }

    public int TaskCount() {
        return tasks.Count;
    }
}