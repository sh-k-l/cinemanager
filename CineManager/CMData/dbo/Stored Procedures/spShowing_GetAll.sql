CREATE PROCEDURE [dbo].[spShowing_GetAll]
AS
begin
	set nocount on;

	select s.Id, sc.Name as Screen, f.Title, s.DateTime
	from dbo.Showing s
	inner join dbo.Film f on f.Id = s.FilmId
	inner join dbo.Screen sc on s.ScreenId = sc.Id
	where s.DateTime > GETDATE();
end