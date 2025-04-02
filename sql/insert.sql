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
    INSERT INTO course (account_email, course_name) VALUES
    ('alice@example.com', 'Database Systems'),
    ('bob@example.com', 'Software Engineering'),
    ('charlie@example.com', 'Computer Networks');

    -- Insert mock data into assignment
    INSERT INTO assignment (course_id, assignment_name) VALUES
    (1, 'SQL Queries Homework'),
    (2, 'Design Patterns Report'),
    (3, 'Network Security Presentation');

    -- Insert mock data into todoitem
    INSERT INTO todoitem (account_email, category_name, parent_id, assignment_id, todo_name) VALUES
    ('alice@example.com', 'Something', NULL, 1, 'Finish database schema'),
    ('bob@example.com', 'Something else', NULL, NULL, 'Buy milk'),
    ('charlie@example.com', 'Lorem', NULL, 2, 'Write design patterns summary');
