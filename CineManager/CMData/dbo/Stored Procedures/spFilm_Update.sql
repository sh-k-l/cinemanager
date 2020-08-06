CREATE PROCEDURE [dbo].[spFilm_Update]
	@Id nvarchar(128),
	@Title nvarchar(100),
	@Description nvarchar(MAX),
	@ImageLink nvarchar(MAX),
	@TrailerLink nvarchar(MAX),
	@ReleaseDate datetime2,
	@Runtime nvarchar(8),
	@Language nvarchar(20)

AS
begin 
	set nocount on;

	update dbo.Film 
	set [Title] = @Title, [Description] = @Description, [ImageLink] = @ImageLink, [TrailerLink] = @TrailerLink, [ReleaseDate] = @ReleaseDate, [Runtime] = @Runtime, [Language] = @Language
	OUTPUT inserted.Id
	where [Id] = @Id;
end