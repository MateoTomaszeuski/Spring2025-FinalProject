using Consilium.Shared.Models;
using Consilium.Shared.ViewModels;
using Dapper;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace Consilium.API.DBServices;

public class DBService(IDbConnection conn) : IDBService {
    #region ToDos
    public void AddToDo(TodoItem Todo, string email) {
        string addItem = @"
            insert into todoitem (account_email, category_name, parent_id, assignment_id, todo_name, completion_date) values 
                (@email, @categoryid, @parentid, @assignmentid, @todoname, @completiondate)
            ";
        conn.Execute(addItem, new {
            email,
            categoryId = Todo.Category,
            parentId = Todo.ParentId,
            assignmentId = Todo.AssignmentId,
            todoName = Todo.Title,
            completionDate = Todo.CompletionDate
        });
    }

    /// <summary>
    /// Retrieves the filled To-Do list. 
    /// </summary>
    public IEnumerable<TodoItem> GetTodoList(string email) {
        string items = """"
            SELECT id, category_name as category, parent_id as parentId, assignment_id as assignmentId, todo_name as title, completion_date as completionDate
            FROM todoitem t WHERE t.account_email = @email
            """";
        return conn.Query<TodoItem>(items, new { email });
    }

    public void RemoveToDo(TodoItem item, string email) {
        string removeItem = """"
            delete from todoitem t where t.id = @id and account_email = @email;
            """";
        conn.Execute(removeItem, new { id = item.Id, email });
    }

    public int ToDoCount(string email) {
        throw new NotImplementedException();
    }

    public void UpdateToDo(TodoItem Todo, string email) {
        string updateItem = """"
                update todoitem t set completion_date = @time where id = @id and account_email = @email
                """";
        DateTime? now = null;
        if (Todo.IsCompleted) {
            now = DateTime.Now;
        }
        conn.Execute(updateItem, new { time = now, id = Todo.Id, email });
    }
    #endregion 
}