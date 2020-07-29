CREATE PROCEDURE [dbo].[spFilm_GetAll]

AS
begin 
	set nocount on;

	select [Id], [Title], [Description], [ImageLink], [TrailerLink], [ReleaseDate], [Runtime], [Language] 
	from dbo.Film;
end