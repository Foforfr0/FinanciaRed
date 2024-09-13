USE [master];
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'FinanciaRed')
BEGIN
    ALTER DATABASE FinanciaRed SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE FinanciaRed;
END;

CREATE DATABASE FinanciaRed;
GO
USE FinanciaRed;
GO



--CREATION DATABASE
CREATE TABLE Bank (
	IdBank INT NOT NULL PRIMARY KEY,
	Name VARCHAR(30) UNIQUE
);

CREATE TABLE BankCardTypes (
	IdBankCardType INT NOT NULL PRIMARY KEY,
	Type VARCHAR(15) UNIQUE
);

CREATE TABLE BankAccounts (
	IdBankAccount INT NOT NULL PRIMARY KEY,
	NameBank INT NOT NULL,
	CardNumber VARCHAR(16) NOT NULL UNIQUE,
	CardType INT NOT NULL,

	CONSTRAINT fk_Bank_BankAccount FOREIGN KEY (NameBank) REFERENCES Bank (IdBank),
	CONSTRAINT fk_CardType_BankAccount FOREIGN KEY (CardType) REFERENCES BankCardTypes (IdBankCardType)
);

CREATE TABLE AddressesTypes (
    IdAddressType INT NOT NULL PRIMARY KEY,
    Type VARCHAR(15) NOT NULL UNIQUE
);

CREATE TABLE ClientsAddresses (
    IdClientAddress INT NOT NULL PRIMARY KEY,
    ExteriorNumber VARCHAR(10) NOT NULL,
    InteriorNumber VARCHAR(10),
    Street VARCHAR(80) NOT NULL,
    Colony VARCHAR(80) NOT NULL,
    PostalCode VARCHAR(10) NOT NULL,
    State VARCHAR(80) NOT NULL,
    IdType INT NOT NULL,

	CONSTRAINT fk_AddressType_ClientAddress FOREIGN KEY (IdType) REFERENCES AddressesTypes (IdAddressType)
);

CREATE TABLE WorkAreaTypes (
    IdWorkAreaType INT NOT NULL PRIMARY KEY,
    Type VARCHAR(50) NOT NULL
);

CREATE TABLE WorkAreas (
    IdWorkArea INT NOT NULL PRIMARY KEY,
    WorkArea VARCHAR(50) NOT NULL,
    IdType INT NOT NULL,

	CONSTRAINT fk_WorkAreaType_WorkArea FOREIGN KEY (IdType) REFERENCES WorkAreaTypes (IdWorkAreaType)
);

CREATE TABLE MaritalStatuses (
	IdMaritalStatus INT NOT NULL PRIMARY KEY,
	Status VARCHAR(20) NOT NULL UNIQUE

);
CREATE TABLE Clients (
    IdClient INT NOT NULL PRIMARY KEY,
    FirstName VARCHAR(80) NOT NULL,
    MiddleName VARCHAR(80) NOT NULL,
    LastName VARCHAR(80) NOT NULL,
    DateBirth DATE NOT NULL,
    Gender CHAR NOT NULL,
    MaritalStatus INT NOT NULL,
    PhoneNumber1 VARCHAR(15) NOT NULL UNIQUE,
    PhoneNumber2 VARCHAR(15) NOT NULL UNIQUE,
    Email1 VARCHAR(80) NOT NULL UNIQUE,
    Email2 VARCHAR(80) NOT NULL UNIQUE,
    CodeRFC VARCHAR(13) NOT NULL UNIQUE,
    CodeCURP VARCHAR(18) NOT NULL UNIQUE,
    BankAccount1 INT NOT NULL,
    BankAccount2 INT,
    IdAddress INT NOT NULL,
    IdWorkArea INT NOT NULL,

	CONSTRAINT fk_MaritalStatus_Client FOREIGN KEY (MaritalStatus) REFERENCES MaritalStatuses (IdMaritalStatus),
	CONSTRAINT fk_BankAccount1_Client FOREIGN KEY (BankAccount1) REFERENCES BankAccounts (IdBankAccount),
	CONSTRAINT fk_BankAccount2_Client FOREIGN KEY (BankAccount2) REFERENCES BankAccounts (IdBankAccount),
	CONSTRAINT fk_AddressClient_Client FOREIGN KEY (IdAddress) REFERENCES ClientsAddresses (IdClientAddress),
	CONSTRAINT fk_WorkArea_Client FOREIGN KEY (IdWorkArea) REFERENCES WorkAreas (IdWorkArea)
);

CREATE TABLE StatesCredits (
	IdStateCredit INT NOT NULL PRIMARY KEY,
	State VARCHAR(15) NOT NULL UNIQUE
);

CREATE TABLE RolesEmployees (
    IdRoleEmployee INT NOT NULL PRIMARY KEY,
    Role VARCHAR(15) NOT NULL
);

CREATE TABLE Employees (
    IdEmployee INT NOT NULL PRIMARY KEY,
    FirstName VARCHAR(80) NOT NULL,
    MiddleName VARCHAR(80) NOT NULL,
    LastName VARCHAR(80) NOT NULL,
    Email VARCHAR(80) NOT NULL,
    Password VARCHAR(80) NOT NULL,
    IdRole INT NOT NULL,

	CONSTRAINT fk_RolesEmployee_Employee FOREIGN KEY (IdRole) REFERENCES RolesEmployees (IdRoleEmployee)
);

CREATE TABLE Credits (
    IdCredit INT NOT NULL PRIMARY KEY,
    Amount BIGINT NOT NULL,
    AmountLeft BIGINT,
    IdClient INT NOT NULL,
    IdStateCredit INT NOT NULL,
    DocumentaAplication BIT NOT NULL,
    Interest FLOAT NOT NULL,
    StartDATE DATETIME NOT NULL,
    EndDATE DATETIME NOT NULL,
    IdEmployee INT NOT NULL,

	CONSTRAINT fk_Client_Credit FOREIGN KEY (IdClient) REFERENCES Clients (IdClient),
	CONSTRAINT fk_StateCredit_Credit FOREIGN KEY (IdStateCredit) REFERENCES StatesCredits (IdStateCredit),
	CONSTRAINT fk_Employee_Credit FOREIGN KEY (IdEmployee) REFERENCES Employees (IdEmployee)
);

--INSERTIONS-CONSTANTS------------------------------------------------
INSERT INTO Bank (Name) 
			VALUES ('Banamex'),
				   ('HSBC México'),
				   ('Santander México'),
				   ('Banorte'),
				   ('Scotiabank México'),
				   ('Bancomer'), 
				   ('Mercado Pago'),
				   ('NU México');

INSERT INTO BankCardTypes (Type) VALUES ('Débito'), ('Crédito');

INSERT INTO AddressesTypes (Type) VALUES ('Cuarto'), ('Departamento'), ('Casa Residencial');

INSERT INTO WorkAreaTypes (Type ) VALUES ('');

--INSERTIONS----------------------------------------------------------
INSERT INTO BankAccounts (NameBank, CardNumber, CardType) VALUES ();
INSERT INTO Clients (FirstName, MiddleName, LastName, DateBirth, Gender, MaritalStatus, 
					 PhoneNumber1, PhoneNumber2, Email1, Email2, CodeRFC, CodeCURP, 
					 BankAccount1, BankAccount2, IdAddress, IdWorkArea) 
			VALUES  ('Rodolfo', 'Fernández', 'Rodríguez', '2003-10-26', 'M', 'Soltero', 
					 '2281856845', '2281856846', 'foforfr007@gmail.com', 'foforfr07@gmail.com', 
					 'FERR031026TG6', 'FERR031026HVZRDDA2', 1, 2, 1, 1);