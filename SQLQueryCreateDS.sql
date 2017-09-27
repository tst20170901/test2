create procedure [dbo].[RunSQL]
(
@sql nvarchar(max)
)
as
begin
  exec sys.sp_executesql @sql;
end
