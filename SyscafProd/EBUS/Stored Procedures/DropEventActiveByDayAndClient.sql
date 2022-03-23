-- author = ygonzalez
-- date = 2021.08.19
-- description = permite eliminar  las tablas stage por dia para los eventos activos
-- EBUS.DropEventActiveByDayAndClient '19082021', '858'
CREATE PROCEDURE EBUS.DropEventActiveByDayAndClient 
(
    @Day NVARCHAR(10),
    @Clienteids NVARCHAR(10)
)
AS
BEGIN
    DECLARE @SQL NVARCHAR(4000) = N'';  
    DECLARE @Sufix NVARCHAR(20) = @Day + N'_' + @Clienteids;



    SET @SQL
        = N'IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''EBUS.[ActiveEvents_' + @Sufix
          + N']'') AND type in (N''U''))
			Drop Table EBUS.[ActiveEvents_' + @Sufix + ']
		';

    EXEC sp_executesql @SQL;

END;
