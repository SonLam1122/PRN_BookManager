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

INSERT INTO Categories (CategoryName, Description)
VALUES
(N'Khoa học', N'Sách liên quan đến khoa học tự nhiên và công nghệ.'),
(N'Văn học', N'Tác phẩm văn học cổ điển và hiện đại.'),
(N'Kinh tế', N'Sách về kinh doanh, tài chính, quản lý.'),
(N'Lịch sử', N'Sách viết về lịch sử thế giới và Việt Nam.');

INSERT INTO Authors (FirstName, LastName, Nationality)
VALUES
(N'Isaac', N'Newton', N'Anh'),
(N'Nguyễn Nhật', N'Ánh', N'Việt Nam'),
(N'Adam', N'Smith', N'Scotland'),
(N'Howard', N'Zinn', N'Mỹ');

INSERT INTO Books (Title, ISBN, CategoryId, AuthorId, PublishYear, Pages, Price, StockQuantity, Description)
VALUES
(N'Triết học tự nhiên', N'978-1234567890', 1, 1, 1687, 500, 99.99, 10, N'Tác phẩm vật lý kinh điển của Isaac Newton.'),
(N'Mắt biếc', N'978-6042089399', 2, 2, 1990, 200, 5.99, 50, N'Truyện nổi tiếng của Nguyễn Nhật Ánh.'),
(N'Wealth of Nations', N'978-0140432084', 3, 3, 1776, 400, 15.50, 30, N'Tác phẩm kinh tế học cổ điển của Adam Smith.'),
(N'A People''s History of the United States', N'978-0062397348', 4, 4, 1980, 700, 12.00, 20, N'Cái nhìn lịch sử từ góc độ dân thường.');
