CREATE TABLE
  "HowlDev.User" (
    email varchar(80) UNIQUE PRIMARY KEY NOT NULL,
    displayName varchar(80) NOT NULL, 
    role int4 NOT NULL
  );

CREATE TABLE
  "HowlDev.Key" (
    id int4 PRIMARY KEY GENERATED ALWAYS AS IDENTITY NOT NULL,
    email varchar(80) references "HowlDev.User" (email) NOT NULL,
    apiKey varchar(20) NOT NULL,
    validatorToken varchar(40) NOT NULL, 
    validatedOn timestamp NULL
  );

CREATE TABLE
  course (
    id int4 PRIMARY KEY GENERATED ALWAYS AS IDENTITY NOT NULL,
    account_email varchar(80) references "HowlDev.User" (email) NOT NULL,
    courseName varchar(80)
  );

CREATE TABLE
  assignment (
    id int4 PRIMARY KEY GENERATED ALWAYS AS IDENTITY NOT NULL,
    courseId int references course (id) NOT NULL,
    assignmentName varchar(80)
  );

-- CREATE TABLE
--   category (
--     id int4 PRIMARY KEY GENERATED ALWAYS AS IDENTITY NOT NULL,
--     account_email varchar(80) references "HowlDev.User" (email) NOT NULL,
--     categoryName varchar(80),
--     color varchar(6)
--   );

CREATE TABLE
  todolist (
    id int4 PRIMARY KEY GENERATED ALWAYS AS IDENTITY NOT NULL,
    account_email varchar(80) references "HowlDev.User" (email) NOT NULL,
    listName varchar(80)
  );

CREATE TABLE
  todoitem (
    id int4 PRIMARY KEY GENERATED ALWAYS AS IDENTITY NOT NULL,
    toDoListId int references todolist (id) NOT NULL,
    categoryId varchar(80) NOT NULL,
    parentId int references todoitem (id) NULL,
    assignmentId int references assignment (id) NULL,
    toDoName varchar(80) NOT NULL, 
    completionDate timestamp NULL
  );