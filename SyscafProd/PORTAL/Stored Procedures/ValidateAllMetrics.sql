

CREATE PROCEDURE PORTAL.ValidateAllMetrics
(@Period VARCHAR(10))
AS
BEGIN


    IF OBJECT_ID('tempdb..#portal_metricasfaltantes') IS NOT NULL
        DROP TABLE #Viajes;

    CREATE TABLE #portal_metricasfaltantes
    (
        ClienteId INTEGER,
        Fecha DATE
    );

    DECLARE @cmd VARCHAR(4000);
    DECLARE cmds CURSOR FOR
    SELECT '
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''PORTAL.TB_TripsMetrics_' + @Period + '_' + CAST(clienteIdS AS VARCHAR)
              + N''') AND type in (N''U''))
		insert into  #portal_metricasfaltantes 
		SELECT ' + CAST(clienteIdS AS VARCHAR) + ', CAST(TT.tripStart AS DATE) FROM PORTAL.TB_Trips_' + @Period + '_'
				   + CAST(clienteIdS AS VARCHAR) + ' AS TT-- WHERE TT.EsProcesado = 0
		LEFT OUTER JOIN PORTAL.TB_TripsMetrics_' + @Period + '_' + CAST(clienteIdS AS VARCHAR)
				   + ' AS TTM ON TTM.TripId = TT.TripId
		WHERE TT.Esprocesado = 0 and  ttm.TripId IS NULL
		GROUP BY CAST(TT.tripStart AS DATE)
	'
    FROM TB_Cliente
    WHERE GeneraIMG = 1
          AND clienteIdS <> 828;

    OPEN cmds;
    WHILE 1 = 1
    BEGIN
        FETCH cmds
        INTO @cmd;
        IF @@fetch_status != 0
            BREAK;
        EXEC (@cmd);
    END;
    CLOSE cmds;
    DEALLOCATE cmds;



    SELECT ClienteId,
           Fecha
    FROM #portal_metricasfaltantes
    ORDER BY Fecha,
             ClienteId;

END;
