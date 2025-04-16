﻿using Consilium.Shared.Models;
using Consilium.Shared.ViewModels;
using Dapper;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Bcpg.OpenPgp;
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
        string getCourseId = """
            SELECT course_id FROM assignment WHERE id = @id
            """;

        int? courseId = conn.QueryFirstOrDefault<int?>(getCourseId, new { id });

        if (courseId is null) return;
        if (!CanAdjustCourse(courseId.Value, email)) return;

        string deleteAssignment = """"
            delete from assignment where id = @id
            """";
        conn.Execute(deleteAssignment, new { id });
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

    public IEnumerable<string> GetConversations(string username) {
        string getConversations = """"
            SELECT DISTINCT participant_email
            FROM (
                SELECT receiver_account_email AS participant_email
                FROM messages
                WHERE sender_account_email = @username
                
                UNION
                
                SELECT sender_account_email AS participant_email
                FROM messages
                WHERE receiver_account_email = @username
            ) AS conversation_participants;
            """";
        return conn.Query<string>(getConversations, new { username });
    }

    public IEnumerable<Message> GetMessages(string username, string otherUser) {
        string getMessages = """"
            SELECT sender_account_email AS Sender, receiver_account_email AS Receiver, message_text AS Content, time_sent AS TimeSent
            FROM messages
            WHERE (sender_account_email = @username AND receiver_account_email = @otherUser)
                OR (sender_account_email = @otherUser AND receiver_account_email = @username)
            ORDER BY time_sent;
            """";
        return conn.Query<Message>(getMessages, new { username, otherUser });
    }

    public Task<string> AddMessage(Message message) {
        string sender = message.Sender;
        string receiver = message.Receiver;
        string content = message.Content;
        DateTime timeSent = message.TimeSent;
        string addMessage = """"
            INSERT INTO messages (sender_account_email, receiver_account_email, message_text, time_sent)
            VALUES (@sender, @receiver, @content, @timeSent)
            RETURNING 'successfully sent message';
            """";
        return Task.FromResult(conn.QuerySingle<string>(addMessage, new { sender, receiver, content, timeSent }));
    }

    public bool CheckUser(string otherUser) {
        string getUser = """"
            SELECT EXISTS (
                SELECT 1
                FROM "HowlDev.User"
                WHERE email = @otherUser
            );
            """";
        return conn.Query<bool>(getUser, new { otherUser }).First();
    }

    public IEnumerable<Course> GetAllCourses(string username) {
        string getCourses = """
            SELECT id, course_name AS CourseName
            FROM course
            WHERE account_email = @username
            """;
        return conn.Query<Course>(getCourses, new { username });
    }

    public int AddCourse(Course course, string email) {
        string addCourse = """
            INSERT INTO course (account_email, course_name)
            VALUES (@account_email, @course_name)
            RETURNING id
            """;

        return conn.QuerySingle<int>(addCourse, new {
            account_email = email,
            course_name = course.CourseName
        });
    }
    public void DeleteCourse(int id, string email) {
        if (!CanAdjustCourse(id, email)) return;
        string deleteCourse = """"
            DELETE FROM assignment WHERE course_id = @id;
            DELETE FROM course WHERE id = @id
            """";
        conn.Execute(deleteCourse, new { id, email });
    }

}