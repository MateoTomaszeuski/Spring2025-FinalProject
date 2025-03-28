insert into account (email, displayName) values ('cody@final.codyhowell.dev', 'Cody')

-- Insert mock accounts
INSERT INTO public.account (email, displayName) VALUES
('user1@example.com', 'User One'),
('user2@example.com', 'User Two'),
('user3@example.com', 'User Three'),
('user4@example.com', 'User Four');

-- Insert mock courses
INSERT INTO public.course (accountId, courseName) VALUES
(1, 'Math 101'),
(1, 'History 201'),
(2, 'Science 301'),
(3, 'Literature 401');

-- Insert mock assignments
INSERT INTO public.assignment (courseId, assignmentName) VALUES
(1, 'Algebra Homework'),
(1, 'Geometry Quiz'),
(2, 'WWII Essay'),
(3, 'Physics Experiment'),
(4, 'Shakespeare Analysis');

-- Insert mock categories
INSERT INTO public.category (account_id, categoryName, color) VALUES
(1, 'Homework', 'FF5733'),
(2, 'Exams', '33FF57'),
(3, 'Projects', '3357FF'),
(4, 'Reading', 'FF33A8');

-- Insert mock to-do lists
INSERT INTO public.todolist (account_id, listName) VALUES
(1, 'Daily Tasks'),
(2, 'Weekly Review'),
(3, 'Assignments List'),
(4, 'Personal Goals');

-- Insert mock to-do items
INSERT INTO public.todoitem (toDoListId, categoryId, parentId, assignmentId, toDoName) VALUES
(1, 1, NULL, 1, 'Complete Algebra Homework'),
(1, 1, NULL, 2, 'Prepare for Geometry Quiz'),
(2, 2, NULL, 3, 'Write WWII Essay'),
(3, 3, NULL, 4, 'Finish Physics Experiment'),
(4, 4, NULL, 5, 'Read Shakespeare Play');
