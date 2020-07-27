CREATE TABLE [dbo].[Film]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] NVARCHAR(100) NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    [ImageLink] NVARCHAR(MAX) NULL, 
    [TrailerLink] NVARCHAR(MAX) NULL, 
    [ReleaseDate] DATETIME2 NOT NULL, 
    [Runtime] TIME NOT NULL, 
    [Language] NCHAR(10) NOT NULL
)
