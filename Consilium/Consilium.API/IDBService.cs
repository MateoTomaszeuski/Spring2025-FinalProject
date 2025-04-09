using Consilium.API.InMemoryServices;
using Consilium.Shared.Models;

namespace Consilium.API;

public interface IDBService {
    #region Todos
    public int AddToDo(TodoItem Todo, string email);
    IEnumerable<TodoItem> GetTodoList(string email);
    public void UpdateToDo(TodoItem Todo, string email);
    public void RemoveToDo(int id, string email);
    #endregion
    #region Assignments
    public IEnumerable<Assignment> GetIncompleteAssignments(string email);
    public IEnumerable<Assignment> GetAllAssignments(string email);
    public void AddAssignment(Assignment assignment, string email);
    public void UpdateAssignment(Assignment assignment, string email);
    public void DeleteAssignment(int id, string email);
    #endregion
}