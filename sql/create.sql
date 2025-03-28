CREATE TABLE
  public.account (
    id int4 PRIMARY KEY GENERATED ALWAYS AS IDENTITY NOT NULL,
    email varchar(80) NOT NULL,
    displayName varchar(80)
  );

CREATE TABLE
  public.course (
    id int4 PRIMARY KEY GENERATED ALWAYS AS IDENTITY NOT NULL,
    accountId int references account (id) NOT NULL,
    courseName varchar(80)
  );

CREATE TABLE
  public.assignment (
    id int4 PRIMARY KEY GENERATED ALWAYS AS IDENTITY NOT NULL,
    courseId int references course (id) NOT NULL,
    assignmentName varchar(80)
  );

CREATE TABLE
  public.category (
    id int4 PRIMARY KEY GENERATED ALWAYS AS IDENTITY NOT NULL,
    account_id int references account (id) NOT NULL,
    categoryName varchar(80),
    color varchar(6)
  );

CREATE TABLE
  public.todolist (
    id int4 PRIMARY KEY GENERATED ALWAYS AS IDENTITY NOT NULL,
    account_id int references account (id) NOT NULL,
    listName varchar(80)
  );

CREATE TABLE
  public.todoitem (
    id int4 PRIMARY KEY GENERATED ALWAYS AS IDENTITY NOT NULL,
    toDoListId int references todolist (id) NOT NULL,
    categoryId int references category (id) NOT NULL,
    parentId int references todoitem (id) NULL,
    assignmentId int references assignment (id) NULL,
    toDoName varchar(80)
  );