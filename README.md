# WebProject
Below are the scripts and stored procedures that are used in the project that are needed for its proper functioning:

//Creating the Category table

CREATE TABLE [dbo].[Category] (
    [Id]        INT        IDENTITY (1, 1) NOT NULL,
    [shortName] NCHAR (50) NOT NULL,
    [longName]  NCHAR (60) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

//Creating the Product table

CREATE TABLE [dbo].[Product] (
    [Id]         INT        IDENTITY (1, 1) NOT NULL,
    [CategoryId] INT        NOT NULL,
    [name]       NCHAR (50) NOT NULL,
    [price]      MONEY      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id]) ON DELETE CASCADE
);

//Creating the Identification table (for users)

CREATE TABLE [dbo].[Identification] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [login]    VARCHAR (20)  NOT NULL,
    [password] VARCHAR (100) NOT NULL,
    [role]     VARCHAR (15)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

//Adding a category

CREATE procedure [dbo].[AddCategory]
@shortName varchar(50),
@longName varchar(50)
as
Insert into Category (shortName, longName) Values (@shortName, @longName)

//Adding a product

CREATE procedure [dbo].[AddProduct]
@CategoryId int,
@name varchar(50),
@price money
as
Insert into Product (CategoryId, name, price) Values(@CategoryId, @name, @price)

//Adding a user(to use the program the first user with admin privileges has to be crated manually)

CREATE procedure [dbo].[AddUser]
@login varchar(20),
@password varchar(100),
@role varchar(15)
as
Insert into Identification (login, password, role) Values (@login, @password, @role)

//Deleting a category

Create procedure [dbo].[DeleteCategory]
@Id int
as
Delete from Category
where Id=@Id

//Deleting a product

Create procedure [dbo].[DeleteProduct]
@Id int
as
Delete from Product
where Id=@Id

//Updating a category

Create procedure [dbo].[EditCategory]
@Id int,
@shortName varchar(50),
@longName varchar(50)
as
Update Category
set shortName=@shortName, longName=@longName
where Id=@Id

//Updating a product

Create procedure [dbo].[EditProduct]
@Id int,
@CategoryId int,
@name varchar(50),
@price money
as
Update Product
set CategoryId=@CategoryId, name=@name, price=@price
where Id=@Id

//Getting categories

Create procedure [dbo].[ShowCategory]
as
Select *
From Category

//Getting products

Create procedure [dbo].[ShowProduct]
as
Select *
From Product
