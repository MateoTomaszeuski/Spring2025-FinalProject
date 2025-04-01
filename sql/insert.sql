 -- Insert mock data into HowlDev.User
    INSERT INTO "HowlDev.User" (email, displayName, role) VALUES
    ('alice@example.com', 'Alice Johnson', 1),
    ('bob@example.com', 'Bob Smith', 2),
    ('charlie@example.com', 'Charlie Brown', 1);

    -- Insert mock data into HowlDev.Key
    INSERT INTO "HowlDev.Key" (email, apiKey, validatorToken, validatedOn) VALUES
    ('alice@example.com', 'APIKEY1234567890', 'TOKENVALID12345678901234567890', '2024-03-31 10:00:00'),
    ('bob@example.com', 'APIKEY0987654321', 'TOKENVALID09876543210987654321', NULL);

    -- Insert mock data into course
    INSERT INTO course (account_email, courseName) VALUES
    ('alice@example.com', 'Database Systems'),
    ('bob@example.com', 'Software Engineering'),
    ('charlie@example.com', 'Computer Networks');

    -- Insert mock data into assignment
    INSERT INTO assignment (courseId, assignmentName) VALUES
    (1, 'SQL Queries Homework'),
    (2, 'Design Patterns Report'),
    (3, 'Network Security Presentation');

    -- Insert mock data into category
    INSERT INTO category (account_email, categoryName, color) VALUES
    ('alice@example.com', 'Work', 'FF5733'),
    ('bob@example.com', 'Personal', '33FF57'),
    ('charlie@example.com', 'Urgent', '5733FF');

    -- Insert mock data into todolist
    INSERT INTO todolist (account_email, listName) VALUES
    ('alice@example.com', 'Project Tasks'),
    ('bob@example.com', 'Grocery List'),
    ('charlie@example.com', 'Homework');

    -- Insert mock data into todoitem
    INSERT INTO todoitem (toDoListId, categoryId, parentId, assignmentId, toDoName) VALUES
    (1, 1, NULL, 1, 'Finish database schema'),
    (2, 2, NULL, NULL, 'Buy milk'),
    (3, 3, NULL, 2, 'Write design patterns summary');
