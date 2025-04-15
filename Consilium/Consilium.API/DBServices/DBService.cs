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
        if (!CanAdjustCourse(assignment.CourseId, email)) return -1;

        string addAssignment = """"
            INSERT INTO assignment
            (course_id, assignment_name, assignment_description, due_date, mark_started, mark_complete)
            VALUES (@courseid, @name, @description, @duedate, @datestarted, @datecompleted)
            returning id
            """";
        return conn.QuerySingle<int>(addAssignment, assignment);
    }

    public void DeleteAssignment(int id, string email) {
        if (!CanAdjustCourse(id, email)) return;

        string deleteAssignment = """"
            delete from assignment where id = @id
            """";
        conn.Execute(deleteAssignment);
    }

    public IEnumerable<Assignment> GetAllAssignments(string email) {
        string getAssignments = """""
        SELECT 
            a.id,
            a.course_id AS CourseId,
            a.assignment_name AS Name,
            a.assignment_description AS Description,
            a.due_date AS DueDate,
            a.mark_started AS DateStarted,
            a.mark_complete AS DateCompleted
        FROM course c
        INNER JOIN assignment a ON a.course_id = c.id
        WHERE c.account_email = @Email;
        """"";
        return conn.Query<Assignment>(getAssignments, new { email });
    }

    public IEnumerable<Assignment> GetIncompleteAssignments(string email) {
        string getIncompleteAssignments = """""
        SELECT 
            a.id, 
            a.course_id AS CourseId,
            a.assignment_name AS Name,
            a.assignment_description AS Description,
            a.due_date AS DueDate,
            a.mark_started AS DateStarted,
            a.mark_complete AS DateCompleted
        FROM course c
        INNER JOIN assignment a ON a.course_id = c.id
        WHERE c.account_email = @Email
        AND a.mark_complete is null
        """"";
        return conn.Query<Assignment>(getIncompleteAssignments, new { email });
    }

    public void UpdateAssignment(Assignment assignment, string email) {
        if (!CanAdjustCourse(assignment.CourseId, email)) return;

        string updateAssignment = """"
            update assignment a set mark_started = @dateStarted, 
                mark_complete = @dateCompleted
                where a.id = @Id
            """";
        conn.Execute(updateAssignment, assignment);
    }
    #endregion

    private bool CanAdjustCourse(int courseId, string email) {
        string OwnsCourse = """""
          SELECT account_email from course where id = @courseid
        """"";

        try {
            string dbEmail = conn.QuerySingle<string>(OwnsCourse, new { courseId });
            return dbEmail == email;
        } catch {
            return false;
        }
    }
}