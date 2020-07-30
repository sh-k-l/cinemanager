CREATE PROCEDURE [dbo].[spShowing_GetByIdAndDate]
	@Id int,
	@Date nvarchar(10)
AS
begin
	set nocount on;

	select s.Id, sc.Name as Screen, f.Title, s.DateTime
	from dbo.Showing s
	inner join dbo.Film f on f.Id = s.FilmId
	inner join dbo.Screen sc on s.ScreenId = sc.Id
	where CONVERT(date, s.DateTime) = @Date
	and f.Id = @Id;
end