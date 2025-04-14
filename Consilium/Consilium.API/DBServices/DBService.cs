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
    public int AddToDo(TodoItem Todo, string email) {
        string addItem = @"
            insert into todoitem (account_email, category_name, parent_id, assignment_id, todo_name, completion_date) values 
                (@email, @categoryid, @parentid, @assignmentid, @todoname, @completiondate)
                returning id
            ";
        return conn.QuerySingle<int>(addItem, new {
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

    public void RemoveToDo(int id, string email) {
        string removeItem = """"
            delete from todoitem t where t.id = @id and account_email = @email;
            """";
        conn.Execute(removeItem, new { id, email });
    }

    public void UpdateAssignment(Assignment assignment, string email) {
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
    #region Assignments

    public int AddAssignment(Assignment assignment, string email) {
        if (!CanAdjustCourse(assignment.CourseId, email)) return;

        string addAssignment = """"
            INSERT INTO assignment
            (course_id, assignment_name, assignment_description, due_date, mark_started, mark_complete)
            VALUES (@course_id, @assignment_name, @assignment_description, @due_date, @mark_started, @mark_complete)
            returning id
            """";
        conn.Execute(addAssignment, assignment);


    }

    public void DeleteAssignment(int id, string email) {
        throw new NotImplementedException();
    }

    public IEnumerable<Assignment> GetAllAssignments(string email) {
        throw new NotImplementedException();
    }

    public IEnumerable<Assignment> GetIncompleteAssignments(string email) {
        throw new NotImplementedException();
    }
    #endregion

    private bool CanAdjustCourse(int courseId, string email) {
        string OwnsCourse = """""
          SELECT account_email from course where id = @course_id
        """"";

        string dbEmail = conn.QuerySingle<string>(OwnsCourse, new { courseId });
        return dbEmail == email;
    }
}