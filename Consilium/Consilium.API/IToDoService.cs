using Consilium.API.InMemoryServices;
using Consilium.Shared.ViewModels;

namespace Consilium.API;

public interface IToDoService {
    public void AddTask(TodoItem task);
}