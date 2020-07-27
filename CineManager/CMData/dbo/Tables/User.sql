CREATE TABLE [dbo].[User]
(
	[Id] NVARCHAR(450) NOT NULL PRIMARY KEY, 
    [Firstname] NVARCHAR(50) NULL, 
    [Surname] NVARCHAR(50) NULL,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [Email] NVARCHAR(50) NOT NULL
)
