USE [Practice-truonggiang];
GO

DROP TABLE IF EXISTS Product;
GO

CREATE TABLE Product(
	id BIGINT IDENTITY(1, 1) PRIMARY KEY,
	name NVARCHAR(255),
	price DECIMAL(8,2),
	createdById BIGINT NOT NULL
);
GO

DROP TABLE IF EXISTS [User];
GO

CREATE TABLE [User](
	id BIGINT IDENTITY(1, 1) PRIMARY KEY,
	name NVARCHAR(255)
);
GO

INSERT INTO Product
	(name, price, createdById) 
VALUES
	('Apple', 1.25, 1),
	('Orange', 1.5, 1),
	('Pine apple', 0.75, 2);
GO

INSERT INTO [User]
	(name) 
VALUES
	('James'),
	('Steve'),
	('Bill');
GO