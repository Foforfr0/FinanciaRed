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
CREATE TABLE StatusesMarital (
	IdStatusMarital INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Status VARCHAR(20) NOT NULL UNIQUE
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

CREATE TABLE Banks (
	IdBank INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(50) UNIQUE
);

CREATE TABLE BankCardTypes (
	IdBankCardType INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Type VARCHAR(15) NOT NULL UNIQUE
);

CREATE TABLE BankAccounts (
	IdBankAccount INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdNameBank INT NOT NULL,
	CardNumber VARCHAR(19) NOT NULL UNIQUE,
	CodeCLABE VARCHAR(23) NOT NULL UNIQUE,
	IdCardType INT NOT NULL,

	CONSTRAINT fk_Bank_BankAccount FOREIGN KEY (IdNameBank) REFERENCES Banks (IdBank),
	CONSTRAINT fk_CardType_BankAccount FOREIGN KEY (IdCardType) REFERENCES BankCardTypes (IdBankCardType)
);

CREATE TABLE StatesAddresses(
    IdStateAddress INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE AddressesTypes (
    IdAddressType INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Type VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE ClientsAddresses (
    IdClientAddress INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdState INT NOT NULL,
    Municipality VARCHAR(150) NOT NULL,
    PostalCode VARCHAR(10) NOT NULL,
    Colony VARCHAR(80) NOT NULL,
    Street VARCHAR(80) NOT NULL,
    ExteriorNumber VARCHAR(30) NOT NULL,
    InteriorNumber VARCHAR(30),
    IdAddressType INT NOT NULL,

	CONSTRAINT fk_AddressType_ClientAddress FOREIGN KEY (IdAddressType) REFERENCES AddressesTypes (IdAddressType),
	CONSTRAINT fk_StateAddress_ClientAddress FOREIGN KEY (IdState) REFERENCES StatesAddresses (IdStateAddress)
);

CREATE TABLE WorkTypes (
    IdWorkType INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Type VARCHAR(50) NOT NULL
);

CREATE TABLE WorkClients (
    IdWorkClient INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    WorkArea VARCHAR(50) NOT NULL,
    IdWorkType INT NOT NULL,
    MonthlySalary FLOAT NOT NULL,

	CONSTRAINT fk_WorkType_WorkClient FOREIGN KEY (IdWorkType) REFERENCES WorkTypes (IdWorkType)
);

CREATE TABLE StatusesClient (
    IdStatusClient INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Status VARCHAR(20) NOT NULL UNIQUE,
);

CREATE TABLE Clients (
    IdClient INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(80) NOT NULL,
    MiddleName VARCHAR(80) NOT NULL,
    LastName VARCHAR(80) NOT NULL,
    DateBirth DATE NOT NULL,
    Gender CHAR NOT NULL,
    IdStatusMarital INT NOT NULL,
    PhoneNumber1 VARCHAR(15) NOT NULL UNIQUE,
    PhoneNumber2 VARCHAR(15),
    Email1 VARCHAR(80) NOT NULL UNIQUE,
    Email2 VARCHAR(80),
    CodeRFC VARCHAR(13) NOT NULL UNIQUE,
    CodeCURP VARCHAR(18) NOT NULL UNIQUE,
    IdContactReference1 INT NOT NULL,
	IdContactReference2 INT,
    IdBankAccount1 INT NOT NULL,
    IdBankAccount2 INT,
    IdAddress INT NOT NULL,
    IdWorkClient INT NOT NULL,
    IdStatusClient INT NOT NULL,

	CONSTRAINT fk_StatusMarital_Client FOREIGN KEY (IdStatusMarital) REFERENCES StatusesMarital (IdStatusMarital),
	CONSTRAINT fk_ContactReference1_Client FOREIGN KEY (IdContactReference1) REFERENCES ContactsReferencesClients (IdContactReference),
	CONSTRAINT fk_ContactReference2_Client FOREIGN KEY (IdContactReference2) REFERENCES ContactsReferencesClients (IdContactReference),
	CONSTRAINT fk_BankAccount1_Client FOREIGN KEY (IdBankAccount1) REFERENCES BankAccounts (IdBankAccount),
	CONSTRAINT fk_BankAccount2_Client FOREIGN KEY (IdBankAccount2) REFERENCES BankAccounts (IdBankAccount),
	CONSTRAINT fk_AddressClient_Client FOREIGN KEY (IdAddress) REFERENCES ClientsAddresses (IdClientAddress),
	CONSTRAINT fk_WorkClient_Client FOREIGN KEY (IdWorkClient) REFERENCES WorkClients (IdWorkClient),
	CONSTRAINT fk_StatusClient_Client FOREIGN KEY (IdStatusClient) REFERENCES StatusesClient (IdStatusClient)
);

CREATE TABLE RolesEmployees (
    IdRoleEmployee INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Role VARCHAR(30) NOT NULL
);

CREATE TABLE StatusesEmployee (
    IdStatusEmployee INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Status VARCHAR(20) NOT NULL UNIQUE,
);

CREATE TABLE Employees (
    IdEmployee INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(80) NOT NULL,
    MiddleName VARCHAR(80) NOT NULL,
    LastName VARCHAR(80) NOT NULL,
    DateBirth Date NOT NULL,
    Gender CHAR NOT NULL,
    CodeCURP VARCHAR(18) UNIQUE NOT NULL,
    CodeRFC VARCHAR(13) UNIQUE NOT NULL,
    ProfilePhoto VARBINARY(MAX),
    Email VARCHAR(80) NOT NULL,
    Password VARCHAR(80) NOT NULL,
    IdRole INT NOT NULL,
    IdStatusEmployee INT NOT NULL,

	CONSTRAINT fk_RolesEmployee_Employee FOREIGN KEY (IdRole) REFERENCES RolesEmployees (IdRoleEmployee),
    CONSTRAINT fk_StatusEmployee_Employee FOREIGN KEY (IdStatusEmployee) REFERENCES StatusesEmployee (IdStatusEmployee)
);

CREATE TABLE StatusesCreditApplication (
    StatusCreditApplication INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Status VARCHAR(20) NOT NULL
);

CREATE TABLE Promotions (
    IdPromotion INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(40) NOT NULL,
    InterestRate REAL NOT NULL,
    NumberFortnights INT NOT NULL,
    DateStart DATE NOT NULL,
    DateEnd DATE NOT NULL
);

CREATE TABLE CreditApplications (
    IdCreditApplication INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    DateApplication DATE NOT NULL,
    DateAcepted DATE,
    AmountTotal INT NOT NULL,
    InteresRate REAL,
    NumberFortnights INT,
    ProofAddress VARBINARY(MAX),
    ProofINE VARBINARY(MAX),
    IdEmployeeApplication INT NOT NULL,
    IdStatusCreditApplication INT NOT NULL,
    IdPromotion INT,
    IdClient INT NOT NULL,

    CONSTRAINT fk_Employee_CreditApplication FOREIGN KEY (IdEmployeeApplication) REFERENCES Employees (IdEmployee),
    CONSTRAINT fk_StatusCreditApplication_CreditApplication FOREIGN KEY (IdStatusCreditApplication) REFERENCES StatusesCreditApplication (StatusCreditApplication),
    CONSTRAINT fk_Promotion_CreditApplication FOREIGN KEY (IdPromotion) REFERENCES Promotions (IdPromotion),
    CONSTRAINT fk_Client_CreditApplication FOREIGN KEY (IdClient) REFERENCES Clients (IdClient)
);

CREATE TABLE Policies (
    IdPolicy INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(40) NOT NULL,
    Description TEXT NOT NULL,
    DateStart DATE NOT NULL,
    DateEnd DATE
);

CREATE TABLE CreditApplications_Policies (
    IdCreditApplication_Policy INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdCreditApplication INT NOT NULL,
    IdPolicy INT NOT NULL,

    CONSTRAINT fk_CreditApplication_CreditPolicy FOREIGN KEY (IdCreditApplication) REFERENCES CreditApplications (IdCreditApplication),
    CONSTRAINT fk_Policy_CreditPolicy FOREIGN KEY (IdPolicy) REFERENCES Policies (IdPolicy)
);

CREATE TABLE StatusesCredit (
	IdStatusCredit INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Status VARCHAR(15) NOT NULL UNIQUE
);

CREATE TABLE Credits (
    IdCredit INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    AmountLeft INT,
    IdStatusCredit INT NOT NULL,
    SignedDocument VARBINARY(MAX),
    PaymentLayout VARBINARY(MAX),
    DateStart DATETIME NOT NULL,
    DateEnd DATETIME NOT NULL,
    IdCreditApplication INT NOT NULL UNIQUE,

	CONSTRAINT fk_StatusCredit_Credit FOREIGN KEY (IdStatusCredit) REFERENCES StatusesCredit (IdStatusCredit),
	CONSTRAINT fk_CreditApplication_Credit FOREIGN KEY (IdCreditApplication) REFERENCES CreditApplications (IdCreditApplication)
);
--INSERTIONS CONSTANTS------------------------------------------------
INSERT INTO StatusesMarital (Status) 
			VALUES ('Soltero/a'),
				   ('Casado/a'),
				   ('Divorciado/a'),
				   ('Viudo/a'),
				   ('Separado/a'),
				   ('Concubinato');

INSERT INTO StatusesClient (Status) 
            VALUES ('Activo'),
                   ('Inactivo'),
                   ('Muerto');

INSERT INTO StatusesEmployee (Status) 
            VALUES ('Activo'),
                   ('Inactivo'),
                   ('Muerto');

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
INSERT INTO StatesAddresses (Name) 
            VALUES ('Aguascalientes'), ('Baja california'), ('Baja california sur'), ('Campeche'),
                   ('Chiapas'), ('Chihuahua'), ('Coahuila'), ('Colima'), ('Durango'), ('Guanajuato'), 
                   ('Guerrero'), ('Hidalgo'), ('Jalisco'), ('Estado de méxico'), ('Michoacán'), ('Morelos'),
                   ('Nayarit'), ('Nuevo León'), ('Oaxaca'), ('Puebla'), ('Querétaro'), ('Quintana Roo'),
                   ('San Luis Potosí'), ('Sinaloa'), ('Sonora'), ('Tabasco'), ('Tamaulipas'), ('Tlaxcala'),
                   ('Veracruz'), ('Yucatán'), ('Zacatecas');

INSERT INTO WorkTypes (Type) 
            VALUES ('Salubridad'), 
                   ('Tecnológico'), 
                   ('Financiero'), 
                   ('Educativo'), 
                   ('Limpieza');

INSERT INTO StatusesCreditApplication VALUES ('Aplicado'), ('Aceptado'), ('Rechazado');

INSERT INTO StatusesCredit (Status) VALUES ('Cobrable'), ('Incobrale');

INSERT INTO RolesEmployees (Role) VALUES ('Administrador'), ('Gestor de cobranza'), ('Analista de crédito'), ('Asesor de crédito');
--INSERTIONS----------------------------------------------------------
INSERT INTO BankAccounts (IdNameBank, CardNumber, CodeCLABE, IdCardType)
            VALUES (1, '4152-3139-1162-1724', '123-456-789-012-345-678', 1),
                   (2, '4152-3139-0028-1625', '876-543-210-987-654-321', 2),
                   (3, '4152-9182-0182-1882', '182-922-252-864-992-283', 1),
                   (4, '4152-1234-5678-9012', '345-678-901-234-567-890', 2);

INSERT INTO ContactsReferencesClients (FirstName, MiddleName, LastName, Email, PhoneNumber, IdRelationshipType) 
            VALUES ('César', 'Basilio', 'Gómez', 'basilios@gmail.com', '922-191-4346', 1),
                   ('Andrés', 'Arellano', 'García', 'and_are@gmail.com', '228-219-6472', 1),
                   ('Karen', 'Álvarez', 'Gómez', 'karenag@gmail.com', '228-881-0091', 1),
                   ('Lluvia', 'Pérez', 'Jácome', 'lluviapj@gmail.com', '228-918-8921', 1);

INSERT INTO ClientsAddresses (IdState, Municipality, PostalCode, Colony, Street, ExteriorNumber, InteriorNumber, IdAddressType)
            VALUES (29, 'Xalapa', '91000', 'Centro', 'Revolución', '300', NULL, 3), 
                   (29, 'Xalapa', '91091', 'Dique', 'Av. Venustiano Carranza', '129', NULL, 3);

INSERT INTO WorkClients (WorkArea, IdWorkType, MonthlySalary)
            VALUES ('Ingeniero de software', 2, 20000),
                   ('Ingeniero tecnologías de la información', 2, 25000);

INSERT INTO Clients (FirstName, MiddleName, LastName, DateBirth, Gender, IdStatusMarital, PhoneNumber1, PhoneNumber2, Email1, Email2, CodeRFC, CodeCURP, 
					 IdContactReference1, IdContactReference2, IdBankAccount1, IdBankAccount2, IdAddress, IdWorkClient, IdStatusClient) 
			VALUES  ('Pedro', 'Pica', 'Piedra', '2003-10-26', 'M', 1, 
					 '228-123-1234', '228-321-4321', 'p_p_p@gmail.com', 'pedro_loco@gmail.com', 
					 'PIPP031026TGK', 'PIPP031026HVZRDDA2', 1, 2, 1, 2, 1, 1, 1),
                    ('Erick', 'Utrera', 'Cornejo', '2002-09-25', 'M', 1, 
					 '228-132-1324', NULL, 'eutrera@gmail.com', NULL, 
					 'UTCE0209359DJ', 'UTCE020935HVZRDGA1', 3, 4, 3, NULL, 2, 2, 1),
                    ('Ana', 'Jácome', 'Gómez', '1995-05-15', 'F', 2, 
                     '228-555-5555', '228-666-6666', 'ana.gomez@example.com', NULL, 
                     'GOMA950515MDF', 'GOMA950515HDFGNA01', 4, NULL, 4, NULL, 1, 2, 1);

INSERT INTO Employees (FirstName, MiddleName, LastName, DateBirth, Gender, CodeRFC, CodeCURP, Email, Password, IdRole, IdStatusEmployee)
            VALUES ('Rodolfo', 'Fernández', 'Rodríguez', '2003-10-26', 'M', 'FERR031026TG6', 'FERR031026HVZRDDA2', 'foforfr007@gmail.com', '1234', 1, 1),
                   ('Martin Emmanuel', 'Cruz', 'Carmona', '2004-11-27', 'M', 'CRCM041127X4G', 'CACM041127HVZGEHE7', 'lecape_27@gmail.com', '1234', 2, 1),
                   ('Sara', 'Hernández', 'Roldán', '2004-08-03', 'M', 'HERS040803HAI', 'HERS040803MVZGJKF9', 'lecape_27@gmail.com', '1234', 3, 1),
                   ('Mario Alberto', 'Hernández', 'Pérez', '1990-05-13', 'M', 'HEPM900513KJS', 'HEPM900513HVZHDHG0', 'mariohernandez2@gmail.com', 'admin', 4, 1);

INSERT INTO Promotions (Name, InterestRate, NumberFortnights, DateStart, DateEnd)
            VALUES ('Plazoz chiquitoz', 0.05, 2, '2024-06-01', '2024-12-31'),
                   ('Para pacientes', 0.15, 4, '2024-06-01', '2024-12-31');

INSERT INTO CreditApplications (DateApplication, DateAcepted, AmountTotal, InteresRate, NumberFortnights, IdPromotion,
                               IdEmployeeApplication, IdStatusCreditApplication, IdClient)
            VALUES ('2024-10-23', '2024-10-25', 10000, 0.20, 6,     NULL,   4, 1, 1),
                   ('2024-10-24', '2024-10-25', 35000, NULL, NULL,  1,      4, 1, 2);

INSERT INTO Policies (Name, Description, DateStart, DateEnd)
            VALUES ('Crédito máximo', 'El monto total de un crédito que solicita un cliente no debe superar el 30% de su salario mensual total.', '2000-01-01', NULL),
                   ('Crédito mínimo', 'El monto minimo de un crédito que solicita un cliente es de $5,000 MXN.', '2000-01-01', NULL),
                   ('Credito superior', 'En créditos mayores de $100,000 MXN, se debe tener en cuenta la zona y tipo de vivienda del cliente.', '2000-01-01', NULL);

INSERT INTO Credits (AmountLeft, IdStatusCredit, SignedDocument, PaymentLayout, DateStart, DateEnd, IdCreditApplication) 
            VALUES (10000, 1, NULL, NULL, '2024-10-26 11:50:00', '2025-01-18 11:50:00', 1),
                   (35000, 1, NULL, NULL, '2024-10-26 18:30:00', '2025-12-21 18:30:00', 2);

INSERT INTO CreditApplications_Policies 
            VALUES (1, 1), (1, 2), (1, 3),
                   (2, 1), (2, 2), (2, 3);
--TESTS---------------------------------------------------------------
SELECT * FROM Credits 
RIGHT JOIN CreditApplications ON Credits.IdCreditApplication = CreditApplications.IdCreditApplication
RIGHT JOIN Clients ON CreditApplications.IdClient = Clients.IdClient
WHERE Clients.IdClient = 1;

SELECT * FROM Policies;

SELECT * FROM Credits;

SELECT * FROM CreditApplications;