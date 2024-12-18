CREATE DATABASE Sellingss;

GO
USE Sellingss
GO


CREATE TABLE Employee (
    EmployeeID INT PRIMARY KEY,
    EmployeeCode NVARCHAR(50),
    EmployeeName NVARCHAR(100),
    Position NVARCHAR(50),
    Salary DECIMAL(18, 2),
    AuthorityLevel Varchar(50),
    Username NVARCHAR(50),
    Password NVARCHAR(255),
    PasswordChanged BIT
);
INSERT INTO [dbo].[Employee]
VALUES ('01744', '1234', 'Nguyen Van Anh', 'Manager', 10000, 'Admin', 'admin1', '123456', 1);

INSERT INTO [dbo].[Employee] (EmployeeID, EmployeeCode, EmployeeName, Position, Salary, AuthorityLevel, Username, Password, PasswordChanged)
VALUES 
('01743', '1234', 'Nguyen Van Bố', 'Staff', 8000, 'Warehouse Manager', 'warehousemanager1', '123456', 1),
('01745', '1234', 'Nguyen Van Con', 'Staff', 6000, 'Sale', 'sale1', '123456', 0);


SELECT * FROM [dbo].[Employee];




CREATE TABLE Import (
    ImportID INT PRIMARY KEY,
    ImportDate DATE,
    EmployeeID INT,
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);

-- Tạo bảng Product
CREATE TABLE Product (
    ProductID INT PRIMARY KEY,
    ProductName NVARCHAR(100),
    UnitPrice DECIMAL(18, 2),
    Category VARCHAR(50),
    ProductImage NVARCHAR(255)
);

Insert Into[dbo].[Product]
Values ('123','Trà sữa','30','Trà sữa','1'),
('124','Trà Đào','30','Trà','2'),
('125','Trà Táo','30','Trà','3'),
('126','Trà Nhài','30','Trà','4'),
('127','Trà Bưởi','30','Trà','5');
SELECT * FROM [dbo].[Product];

-- Tạo bảng ImportDetail
CREATE TABLE ImportDetail (
    ImportDetailID INT PRIMARY KEY,
    ImportID INT,
    ProductID INT,
    ImportCost DECIMAL(18, 2),
    QuantityImport INT,
    FOREIGN KEY (ImportID) REFERENCES Import(ImportID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);

-- Tạo bảng Customer


-- Tạo bảng Sales
CREATE TABLE Sales (
    SaleID INT PRIMARY KEY,
    ProductID INT,
    QuantitySold INT,
    SaleDate DATE,
    EmployeeID INT,
	CustomerID INT,
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);

-- Tạo bảng SaleDetail
CREATE TABLE SaleDetail (
    SaleDetailID INT PRIMARY KEY,
    SaleID INT,
    ProductID INT,
    FOREIGN KEY (SaleID) REFERENCES Sales(SaleID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);
CREATE TABLE Customer (
    CustomerID INT PRIMARY KEY,
    CustomerName NVARCHAR(100),
    SaleID INT,
    FOREIGN KEY (SaleID) REFERENCES Sales(SaleID)
);

Create table Category(
CategoryID	int primary key,
CategoryName varchar(100),
);

