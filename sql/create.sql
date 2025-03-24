CREATE TABLE public.account (
  	id int4 GENERATED ALWAYS AS IDENTITY NOT NULL,
    email varchar(80) NOT NULL, 
    displayName varchar(80)
)