CREATE PROCEDURE [dbo].[spFilm_Insert]
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

	insert into dbo.Film ([Id], [Title], [Description], [ImageLink], [TrailerLink], [ReleaseDate], [Runtime], [Language])
	values (@Id, @Title, @Description, @ImageLink, @TrailerLink, @ReleaseDate, @Runtime, @Language);

end