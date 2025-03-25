
namespace Consilium.API.InMemoryServices;

public class ToDoServiceIM : IToDoService {
    private List<Task> tasks = new();
    public void AddTask(Task task) {
        tasks.Add(task);
    }
}