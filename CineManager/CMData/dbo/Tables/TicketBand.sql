CREATE TABLE [dbo].[TicketBand]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Student] MONEY NULL, 
    [Standard] MONEY NULL, 
    [OAP] MONEY NULL, 
    [Child] MONEY NULL, 
    [Name] NCHAR(20) NOT NULL
)
