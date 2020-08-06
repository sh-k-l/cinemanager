CREATE TABLE [dbo].[Showing]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ScreenId] INT NOT NULL, 
    [FilmId] NVARCHAR(128) NOT NULL, 
    [DateTime] DATETIME2 NOT NULL, 
    CONSTRAINT [FK_Showing_Screen] FOREIGN KEY (ScreenId) REFERENCES Screen(Id),
    CONSTRAINT [FK_Showing_Film] FOREIGN KEY (FilmId) REFERENCES Film(Id)
)
