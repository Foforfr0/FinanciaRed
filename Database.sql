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
CREATE TABLE MaritalStatuses (
	IdMaritalStatus INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Status VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE Banks (
	IdBank INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(30) UNIQUE
);

CREATE TABLE BankCardTypes (
	IdBankCardType INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Type VARCHAR(15) NOT NULL UNIQUE
);

CREATE TABLE BankAccounts (
	IdBankAccount INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdNameBank INT NOT NULL,
	CardNumber VARCHAR(19) NOT NULL UNIQUE,
	CLABE VARCHAR(23) NOT NULL UNIQUE,
	IdCardType INT NOT NULL,

	CONSTRAINT fk_Bank_BankAccount FOREIGN KEY (IdNameBank) REFERENCES Banks (IdBank),
	CONSTRAINT fk_CardType_BankAccount FOREIGN KEY (IdCardType) REFERENCES BankCardTypes (IdBankCardType)
);

CREATE TABLE RelationshipsClientsTypes (
	IdRelationshipClient INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Type VARCHAR(15) NOT NULL UNIQUE
);

CREATE TABLE ContactsReferencesClients (
	IdContactReference INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	FirstName VARCHAR(80) NOT NULL,
    MiddleName VARCHAR(80) NOT NULL,
    LastName VARCHAR(80) NOT NULL,
    PhoneNumber VARCHAR(80) NOT NULL,
    Email VARCHAR(80) NOT NULL,
	IdRelationshipType INT NOT NULL,

	CONSTRAINT fk_RelationshipClientType_ContactReferenceClient FOREIGN KEY (IdRelationshipType) REFERENCES RelationshipsClientsTypes (IdRelationshipClient),
);

CREATE TABLE AddressesTypes (
    IdAddressType INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Type VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE StatesAddresses(
    IdStateAddress INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE ClientsAddresses (
    IdClientAddress INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    ExteriorNumber VARCHAR(30) NOT NULL,
    InteriorNumber VARCHAR(30),
    Street VARCHAR(80) NOT NULL,
    Colony VARCHAR(80) NOT NULL,
    PostalCode VARCHAR(10) NOT NULL,
    Municipality VARCHAR(150) NOT NULL,
    IdState INT NOT NULL,
    IdAddressType INT NOT NULL,

	CONSTRAINT fk_AddressType_ClientAddress FOREIGN KEY (IdAddressType) REFERENCES AddressesTypes (IdAddressType),
	CONSTRAINT fk_StateAddress_ClientAddress FOREIGN KEY (IdState) REFERENCES StatesAddresses (IdStateAddress)
);

CREATE TABLE WorkAreaTypes (
    IdWorkAreaType INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Type VARCHAR(50) NOT NULL
);

CREATE TABLE WorkAreas (
    IdWorkArea INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    WorkArea VARCHAR(50) NOT NULL,
    IdWorkAreaType INT NOT NULL,
    MonthlySalary FLOAT NOT NULL,

	CONSTRAINT fk_WorkAreaType_WorkArea FOREIGN KEY (IdWorkAreaType) REFERENCES WorkAreaTypes (IdWorkAreaType)
);

CREATE TABLE Clients (
    IdClient INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(80) NOT NULL,
    MiddleName VARCHAR(80) NOT NULL,
    LastName VARCHAR(80) NOT NULL,
    DateBirth DATE NOT NULL,
    Gender CHAR NOT NULL,
    IdMaritalStatus INT NOT NULL,
    PhoneNumber1 VARCHAR(15) NOT NULL UNIQUE,
    PhoneNumber2 VARCHAR(15) NOT NULL UNIQUE,
    Email1 VARCHAR(80) NOT NULL UNIQUE,
    Email2 VARCHAR(80) NOT NULL UNIQUE,
    CodeRFC VARCHAR(13) NOT NULL UNIQUE,
    CodeCURP VARCHAR(18) NOT NULL UNIQUE,
    IdContactReference1 INT NOT NULL,
	IdContactReference2 INT,
    IdBankAccount1 INT NOT NULL,
    IdBankAccount2 INT,
    IdAddress INT NOT NULL,
    IdWorkArea INT NOT NULL,
    StatusActive BIT NOT NULL,

	CONSTRAINT fk_MaritalStatus_Client FOREIGN KEY (IdMaritalStatus) REFERENCES MaritalStatuses (IdMaritalStatus),
	CONSTRAINT fk_ContactReference1_Client FOREIGN KEY (IdContactReference1) REFERENCES ContactsReferencesClients (IdContactReference),
	CONSTRAINT fk_ContactReference2_Client FOREIGN KEY (IdContactReference2) REFERENCES ContactsReferencesClients (IdContactReference),
	CONSTRAINT fk_BankAccount1_Client FOREIGN KEY (IdBankAccount1) REFERENCES BankAccounts (IdBankAccount),
	CONSTRAINT fk_BankAccount2_Client FOREIGN KEY (IdBankAccount2) REFERENCES BankAccounts (IdBankAccount),
	CONSTRAINT fk_AddressClient_Client FOREIGN KEY (IdAddress) REFERENCES ClientsAddresses (IdClientAddress),
	CONSTRAINT fk_WorkArea_Client FOREIGN KEY (IdWorkArea) REFERENCES WorkAreas (IdWorkArea)
);

CREATE TABLE StatesCredits (
	IdStateCredit INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	State VARCHAR(15) NOT NULL UNIQUE
);

CREATE TABLE RolesEmployees (
    IdRoleEmployee INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Role VARCHAR(30) NOT NULL
);

CREATE TABLE Employees (
    IdEmployee INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(80) NOT NULL,
    MiddleName VARCHAR(80) NOT NULL,
    LastName VARCHAR(80) NOT NULL,
    ProfilePhoto VARBINARY(MAX),
    Email VARCHAR(80) NOT NULL,
    Password VARCHAR(80) NOT NULL,
    IdRole INT NOT NULL,

	CONSTRAINT fk_RolesEmployee_Employee FOREIGN KEY (IdRole) REFERENCES RolesEmployees (IdRoleEmployee)
);

CREATE TABLE Credits (
    IdCredit INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Amount BIGINT NOT NULL,
    AmountLeft BIGINT,
    IdClient INT NOT NULL,
    IdStateCredit INT NOT NULL,
    SignedDocument VARBINARY(MAX),
    InterestRate FLOAT NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    IdEmployee INT NOT NULL,

	CONSTRAINT fk_Client_Credit FOREIGN KEY (IdClient) REFERENCES Clients (IdClient),
	CONSTRAINT fk_StateCredit_Credit FOREIGN KEY (IdStateCredit) REFERENCES StatesCredits (IdStateCredit),
	CONSTRAINT fk_Employee_Credit FOREIGN KEY (IdEmployee) REFERENCES Employees (IdEmployee)
);
--INSERTIONS-CONSTANTS------------------------------------------------
INSERT INTO MaritalStatuses (Status) 
			VALUES ('Soltero/a'),
				   ('Casado/a'),
				   ('Divorciado/a'),
				   ('Viudo/a'),
				   ('Separado/a'),
				   ('Concubinato');

INSERT INTO Banks (Name) 
			VALUES ('Banamex'),
				   ('HSBC México'),
				   ('Santander México'),
				   ('Banorte'),
				   ('Scotiabank México'),
				   ('Bancomer'), 
				   ('Mercado Pago'),
				   ('NU México');

INSERT INTO BankCardTypes (Type) VALUES ('Débito'), ('Crédito');

INSERT INTO RelationshipsClientsTypes (Type)
            VALUES ('Amigo/a'), ('Familiar'), ('Pareja');

INSERT INTO AddressesTypes (Type) VALUES ('Cuarto'), ('Departamento'), ('Casa');
INSERT INTO StatesAddresses (Name) VALUES ('Aguascalientes'), ('Baja california'), ('Baja california sur'), ('Campeche'),
                                          ('Chiapas'), ('Chihuahua'), ('Coahuila'), ('Colima'), ('Durango'), ('Guanajuato'), 
                                          ('Guerrero'), ('Hidalgo'), ('Jalisco'), ('Estado de méxico'), ('Michoacán'), ('Morelos'),
                                          ('Nayarit'), ('Nuevo León'), ('Oaxaca'), ('Puebla'), ('Querétaro'), ('Quintana Roo'),
                                          ('San Luis Potosí'), ('Sinaloa'), ('Sonora'), ('Tabasco'), ('Tamaulipas'), ('Tlaxcala'),
                                          ('Veracruz'), ('Yucatán'), ('Zacatecas');

INSERT INTO WorkAreaTypes (Type) VALUES ('Desarrollo tecnológico');

INSERT INTO StatesCredits (State) VALUES ('Cobrable'), ('Incobrale');

INSERT INTO RolesEmployees (Role) VALUES ('Asesor de crédito'), ('Analista de crédito'), ('Gestor de cobranza'), ('Administrador');

--INSERTIONS----------------------------------------------------------
INSERT INTO BankAccounts (IdNameBank, CardNumber, CLABE, IdCardType)
            VALUES (1, '4234-5678-9012-3456', '123-456-789-012-345-678', 1);
INSERT INTO BankAccounts (IdNameBank, CardNumber, CLABE, IdCardType)
            VALUES (2, '4543-2109-8765-4321', '876-543-210-987-654-321', 2);

INSERT INTO ContactsReferencesClients (FirstName, MiddleName, LastName,
                                      Email, PhoneNumber, IdRelationshipType) 
            VALUES ('César', 'Basilio', 'Gómez', 'basilios@gmail.com', '922-191-4346', 1);
INSERT INTO ContactsReferencesClients (FirstName, MiddleName, LastName,
                                      Email, PhoneNumber, IdRelationshipType) 
            VALUES ('Andrés', 'Arellano', 'García', 'and_are@gmail.com', '228-219-6472', 1);

INSERT INTO ClientsAddresses (ExteriorNumber, InteriorNumber, Street, Colony, 
                             PostalCode, Municipality, IdState, IdAddressType)
            VALUES ('Azahares 3', '308', 'Circuito Primavera', 'Nuevo Xalapa', 
                   '91097', 'Xalapa', 29, 2);

INSERT INTO WorkAreas (WorkArea, IdWorkAreaType, MonthlySalary)
            VALUES ('Ingeniero de software', 1, 20000);

INSERT INTO Clients (FirstName, MiddleName, LastName, DateBirth, Gender, IdMaritalStatus, 
					 PhoneNumber1, PhoneNumber2, Email1, Email2, CodeRFC, CodeCURP, 
					 IdBankAccount1, IdBankAccount2, IdContactReference1, IdContactReference2, IdAddress, IdWorkArea, StatusActive) 
			VALUES  ('Pedro', 'Pica', 'Piedra', '2003-10-26', 'M', 1, 
					 '228-123-1234', '228-321-4321', 'p_p_p@gmail.com', 'pedro_loco@gmail.com', 
					 'PIPP031026TGK', 'PIPP031026HVZRDDA2', 1, 2, 1, 2, 1, 1, 1);

INSERT INTO Employees (FirstName, MiddleName, LastName, Email, Password, IdRole)
            VALUES ('Rodolfo', 'Fernández', 'Rodríguez', 'foforfr007@gmail.com', '1234', 1);
INSERT INTO Employees (FirstName, MiddleName, LastName, Email, Password, IdRole)
            VALUES ('Martin Emmanuel', 'Cruz', 'Carmona', 'lecape_27@gmail.com', '4321', 2);
INSERT INTO Employees (FirstName, MiddleName, LastName, Email, Password, IdRole)
            VALUES ('Mario Alberto', 'Hernández', 'Pérez', 'mariohernandez2@gmail.com', 'admin', 4);

INSERT INTO Credits (Amount, AmountLeft, IdClient, IdStateCredit, SignedDocument, InterestRate, 
                    StartDate, EndDate, IdEmployee) 
            VALUES (100000, 100000, 1, 1, NULL, 10.0, '2024-11-12 11:50:00', '2025-11-12 11:50:00', 1);

SELECT * FROM Clients;