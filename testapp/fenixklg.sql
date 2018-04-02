CREATE TABLE tblUserTypes
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(100)
)

INSERT INTO tblUserTypes VALUES('Участник')
INSERT INTO tblUserTypes VALUES('Веду МК')
INSERT INTO tblUserTypes VALUES('Участник ярмарки')
INSERT INTO tblUserTypes VALUES('Веду лекцию/ семинар / круглый стол')

CREATE TABLE tblUsers
( 
	Id INT NOT NULL IDENTITY(1,1),
	Email VARCHAR(100) NOT NULL,
	Password VARCHAR(100) NOT NULL,
	Surname VARCHAR(100) NOT NULL,
	Name VARCHAR(100) NOT NULL,
	Nik VARCHAR(100),
	City VARCHAR(100) NOT NULL,
	Comment VARCHAR(100) NOT NULL,
	TypeId INT,
	CONSTRAINT PK_UserTable PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT FK_UserTypes FOREIGN KEY (TypeId) REFERENCES tblUserTypes(Id)
)

ALTER TABLE tblUsers ALTER COLUMN Comment VARCHAR(100) NULL

CREATE TABLE tblBalance
(
	Id INT NOT NULL IDENTITY(1,1),
	UserId INT NOT NULL,
	[IsEnabled] BIT NOT NULL,
	[IsBank] BIT NOT NULL,
	[Code] VARCHAR(100) NOT NULL,
	Balance INT NOT NULL,
	CONSTRAINT PK_BalanceTable PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT FK_Users FOREIGN KEY (UserId) REFERENCES tblUsers(Id)
)

CREATE TABLE tblNews 
(
	Id INT NOT NULL IDENTITY(1,1),
	Title VARCHAR(100),
	Content VARCHAR(MAX),
	[Date] DATETIME,
	CONSTRAINT PK_News PRIMARY KEY CLUSTERED (Id)
)

SELECT Surname, Name FROM tblUsers u JOIN tblBalance b ON b.UserId = u.Id WHERE b.IsEnabled = 1 AND u.RegDate IS NOT NULL 

UPDATE tblBalance Set IsEnabled = 1 WHERE UserId = 15

INSERT INTO tblNews VALUES('Запуск личного кабинета','Уведомляем Вас, что с 1.04 работает личный кабинет! Здесь вы можете оставить Ваши пожелания организаторам в соответствующем разделе, узнать свой счёт бёрнингов и не упустить самое интересное!', GETDATE())