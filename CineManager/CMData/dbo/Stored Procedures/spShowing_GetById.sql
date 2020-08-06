CREATE PROCEDURE [dbo].[spShowing_GetById]
	@Id int
AS
begin
	set nocount on;

	select s.Id, sc.Name as Screen, f.Title, s.DateTime
	from dbo.Showing s
	inner join dbo.Film f on f.Id = s.FilmId
	inner join dbo.Screen sc on s.ScreenId = sc.Id
	where f.Id = @Id
	and s.DateTime > GETDATE();
end