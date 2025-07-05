-- Tạo Database
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'BookStoreManagement')
BEGIN
    ALTER DATABASE BookStoreManagement SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE BookStoreManagement;
END
GO

CREATE DATABASE BookStoreManagement;
GO

USE BookStoreManagement;
GO

-- Tạo bảng Categories (Danh mục)
CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    CreatedDate DATETIME2 DEFAULT GETDATE()
);

-- Tạo bảng Authors (Tác giả)
CREATE TABLE Authors (
    AuthorId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Nationality NVARCHAR(50),
    CreatedDate DATETIME2 DEFAULT GETDATE()
);

-- Tạo bảng Books (Sách)
CREATE TABLE Books (
    BookId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(300) NOT NULL,
    ISBN NVARCHAR(20),
    CategoryId INT,
    AuthorId INT,
    PublishYear INT,
    Pages INT,
    Price DECIMAL(18,2) NOT NULL DEFAULT 0,
    StockQuantity INT NOT NULL DEFAULT 0,
    Description NVARCHAR(1000),
    CreatedDate DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
    FOREIGN KEY (AuthorId) REFERENCES Authors(AuthorId)
);

-- Tạo Index
CREATE INDEX IX_Books_CategoryId ON Books(CategoryId);
CREATE INDEX IX_Books_AuthorId ON Books(AuthorId);
CREATE INDEX IX_Books_Title ON Books(Title);

PRINT 'Database và các bảng đã được tạo thành công!';
