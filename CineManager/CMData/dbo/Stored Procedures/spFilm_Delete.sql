CREATE PROCEDURE [dbo].[spFilm_Delete]
	@Id nvarchar(128)

AS
begin 
	set nocount on;

	delete from dbo.Film 
	where [Id] = @Id;
end