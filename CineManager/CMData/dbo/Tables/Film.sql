CREATE TABLE [dbo].[Film]
(
	[Id] NVARCHAR(128) NOT NULL PRIMARY KEY, 
    [Title] NVARCHAR(100) NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL, 
    [ImageLink] NVARCHAR(MAX) NULL, 
    [TrailerLink] NVARCHAR(MAX) NULL, 
    [ReleaseDate] DATETIME2 NOT NULL, 
    [Runtime] NVARCHAR(8) NOT NULL, 
    [Language] NVARCHAR(20) NOT NULL
)
