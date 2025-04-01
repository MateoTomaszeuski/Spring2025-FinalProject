using Consilium.Shared.Models;
using Consilium.Shared.ViewModels;
using Dapper;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace Consilium.API.DBServices;

public class DBService(IDbConnection conn) : IDBService {
    public void AddToDo(TodoItem Todo, string username) {
        throw new NotImplementedException();
    }
    public IEnumerable<TodoList> GetToDoLists(string email) {
        string sql = $"SELECT id, listname FROM todolist tl INNER JOIN \"HowlDev.User\" h on (tl.account_email = h.email) WHERE h.email = @email";
        var todolists = conn.Query<TodoList>(sql, new { email });
        return todolists;
    }
    public TodoList GetTodoList(int tableid) {
        string sql = $"SELECT id, listname FROM todolist WHERE id = @tableid";
        var todolist = conn.Query<TodoList>(sql, new { tableid }).FirstOrDefault();

        sql = $"SELECT id, categoryId, parentId, assignmentId,toDoName FROM todoitem WHERE toDoListId = @tableid";
        var todoitems = conn.Query<TodoItem>(sql, new { tableid });

        if (todolist is null)
            throw new SqlNullValueException();
        todolist.TodoItems = new(todoitems);
        return todolist;
    }

    public void RemoveToDo(int index, string username) {
        throw new NotImplementedException();
    }

    public int ToDoCount(string username) {
        throw new NotImplementedException();
    }

    public void UpdateToDo(int index, TodoItem Todo, string username) {
        throw new NotImplementedException();
    }
}