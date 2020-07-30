CREATE PROCEDURE [dbo].[spFilm_GetByDate]
	@Date nvarchar(128)
AS
begin
	set nocount on;

	select distinct f.*
	from dbo.Film f
	inner join dbo.Showing s
	on s.FilmId = f.Id
	where CONVERT(date, s.[DateTime]) = @Date
	;
end
