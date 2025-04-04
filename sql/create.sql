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
    course_name varchar(80)
  );

CREATE TABLE
  assignment (
    id int4 PRIMARY KEY GENERATED ALWAYS AS IDENTITY NOT NULL,
    course_id int references course (id) NOT NULL,
    assignment_name varchar(80)
  );

CREATE TABLE
  todoitem (
    id int4 PRIMARY KEY GENERATED ALWAYS AS IDENTITY NOT NULL,
    account_email varchar(80) references "HowlDev.User" (email) NOT NULL,
    category_name varchar(80) NOT NULL,
    parent_id int references todoitem (id) NULL,
    assignment_id int references assignment (id) NULL,
    todo_name varchar(80) NOT NULL, 
    completion_date timestamp NULL
  );