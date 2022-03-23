-- author = ygonzalez
-- date = 2021.08.19
-- description = trae los ultimos eventos activos por conductor
--EBUS.GetEventActiverecargaByDayAndClient '122021','914'
CREATE PROCEDURE EBUS.GetEventActiveRecargaByDayAndClient
(
    @Day VARCHAR(10),
    @Clienteids NVARCHAR(10)
)
AS
BEGIN

    --DECLARE @Day VARCHAR(10) = '122021',
    --        @Clienteids NVARCHAR(10) = N'914';
    DECLARE @SQL NVARCHAR(4000) = N'';
    DECLARE @Sufix NVARCHAR(20) = @Day + N'_' + @Clienteids;


    DECLARE @GunConn NVARCHAR(50);
    DECLARE @GunDescConn NVARCHAR(50);


    SELECT @GunConn = P.Valor
    FROM EBUS.Parametrizacion AS P
        INNER JOIN dbo.TB_DetalleListas AS TDL
            ON TDL.DetalleListaId = P.TipoParametroId
    WHERE P.ClienteIds = CAST(@Clienteids AS INT)
          AND TDL.Sigla = 'EBUS_GUNCONN';
    SELECT @GunDescConn = P.Valor
    FROM EBUS.Parametrizacion AS P
        INNER JOIN dbo.TB_DetalleListas AS TDL
            ON TDL.DetalleListaId = P.TipoParametroId
    WHERE P.ClienteIds = CAST(@Clienteids AS INT)
          AND TDL.Sigla = 'EBUS_GUNDESCCONN';


    -- declaramos la tabla temporal para traernos los datos de las tablas marcadas con los periodos
    IF OBJECT_ID('tempdb..#ebus_EventosRecarga') IS NOT NULL
        DROP TABLE #ebus_EventosRecarga;
    CREATE TABLE #ebus_EventosRecarga
    (
        FechaHora DATETIME NOT NULL,
        Consecutivo INT NULL,
        SumConsecutivo INT NULL,
        AssetId BIGINT NOT NULL,
        Soc DECIMAL(18, 4) NULL,
        SocInicial DECIMAL(18, 4) NULL,
        GunConn BIGINT NULL,
        Corriente DECIMAL(18, 4) NULL,
        Voltaje DECIMAL(18, 4) NULL,
        Potencia DECIMAL(18, 4) NULL,
        GunDesConn BIGINT NULL,
		FechaInicioRecarga datetime
    );
	   
    SET @SQL
        = N'
	 insert into #ebus_EventosRecarga (Consecutivo,SumConsecutivo,AssetId,FechaHora,Soc, SocInicial, GunConn,Voltaje,Corriente, Potencia, GunDesConn, FechaInicioRecarga)	

      SELECT AER.Consecutivo,      
       AER.SumConsecutivo,
       AER.AssetId,
       AER.FechaHora,
       AER.Soc,
	   SocInicial = FIRST_VALUE(AER.Soc) OVER(PARTITION BY AER.AssetId ORDER BY AER.FechaHora, ISNULL(AER.Consecutivo, 2)),
	   GunConn = FIRST_VALUE(AER.EventTypeID) OVER(PARTITION BY AER.AssetId ORDER BY AER.FechaHora, ISNULL(AER.Consecutivo, 2)),
       AER.Voltaje,
       AER.Corriente,
       Potencia = ABS(AER.Voltaje * AER.Corriente) / 1000,	 
	   GunDesConn = FIRST_VALUE(AER.EventTypeID) OVER(PARTITION BY AER.AssetId ORDER BY AER.FechaHora desc, ISNULL(AER.Consecutivo, 2) desc),
	   FechaInicioRecarga = FIRST_VALUE(AER.FechaHora) OVER(PARTITION BY AER.AssetId ORDER BY AER.FechaHora, ISNULL(AER.Consecutivo, 2))
        FROM
        (
            SELECT AER.Consecutivo,
					AER.EventTypeID,
                   SumConsecutivo = SUM(AER.Consecutivo) OVER (PARTITION BY AER.AssetId ORDER BY AER.FechaHora),
                   AER.AssetId,
                   AER.FechaHora,
                   Soc = MAX(AER.Soc),                  
                   Voltaje = MAX(AER.Voltaje),
                   Corriente = MAX(AER.Corriente)
            FROM EBUS.ActiveEventsRecarga_' + @Sufix
          + N' AS AER
                INNER JOIN
                (
                    SELECT AER.AssetId,
                           MAX(AER.FechaHora) FechaHora
                    FROM EBUS.ActiveEventsRecarga_' + @Sufix + N' AS AER
                    WHERE AER.EventTypeID = ' + @GunConn
          + N' 
                    GROUP BY AER.AssetId
                ) AS filter
                    ON filter.AssetId = AER.AssetId
                       AND AER.FechaHora >= filter.FechaHora
           
            GROUP BY AER.FechaHora,
                     AER.Consecutivo,
                     AER.AssetId, EventTypeID
        ) AS AER				
		ORDER BY AER.AssetId,
         AER.FechaHora,
         ISNULL(AER.Consecutivo, 2),
         AER.SumConsecutivo
	   ';



    EXEC sp_executesql @SQL;
    SELECT Filtrado.Muestra,
           Filtrado.AssetId,
           Fecha = Filtrado.FechaInicioRecarga ,
           Filtrado.Voltaje,
           Corriente =  ISNULL(Filtrado.Corriente, 0),
           Potencia = ISNULL(Filtrado.Potencia,0),
           Filtrado.Energia,
           Soc = ISNULL(Filtrado.Soc, Filtrado.MaximoSoc),
           Filtrado.TotalTime,
           Filtrado.SocInicial,
           Filtrado.IsDisconected,
           PotenciaProm = Filtrado.PotenciaProm
    FROM
    (
        SELECT Final.Muestra,
               Final.AssetId,
               Fecha = Final.FechaHora,
               Final.Voltaje,
               Final.Corriente,
               Final.Potencia,
               Final.Energia,
               Final.Soc,
               MaximoSoc = MAX(Final.Soc) OVER (PARTITION BY Final.AssetId),
               Final.TotalTime,
               Final.SocInicial,
			   Final.FechaInicioRecarga,
               Final.IsDisconected,
               Final.PotenciaProm
        FROM
        (
            SELECT Consecutivo = Recarga.SumConsecutivo,
                   ConsecutivoFinal = FIRST_VALUE(Recarga.SumConsecutivo) OVER (PARTITION BY Recarga.AssetId ORDER BY Recarga.FechaHora DESC),
                   Muestra = ROW_NUMBER() OVER (PARTITION BY Recarga.SumConsecutivo,
                                                             Recarga.AssetId
                                                ORDER BY Recarga.FechaHora DESC
                                               ),
                   Recarga.AssetId,
                   Recarga.FechaHora,
                   Recarga.Voltaje,
                   Recarga.Corriente,
                   Recarga.Potencia,
                   PotenciaProm = AVG(Recarga.Potencia) OVER (PARTITION BY Recarga.SumConsecutivo,
                                                                           Recarga.AssetId
                                                              ORDER BY Recarga.FechaHora
                                                             ),
                   Energia = SUM(Recarga.Potencia) OVER (PARTITION BY Recarga.SumConsecutivo,
                                                                      Recarga.AssetId
                                                         ORDER BY Recarga.FechaHora
                                                        ) / 60,
                   Recarga.Soc,
                   TotalTime = CONVERT(
                                          VARCHAR(8),
                                          DATEADD(
                                                     SECOND,
                                                     DATEDIFF(
                                                                 SECOND,
                                                                 FIRST_VALUE(Recarga.FechaHora) OVER (PARTITION BY Recarga.SumConsecutivo,
                                                                                                                   Recarga.AssetId
                                                                                                      ORDER BY Recarga.FechaHora
                                                                                                     ),
                                                                 Recarga.FechaHora
                                                             ),
                                                     0
                                                 ),
                                          114
                                      ),
                   Recarga.SocInicial,
				   Recarga.FechaInicioRecarga,
                   IsDisconected = CASE
                                       WHEN Recarga.GunDesConn = @GunDescConn THEN
                                           1
                                       WHEN DATEDIFF(
                                                        MINUTE,
                                                        FIRST_VALUE(Recarga.FechaHora) OVER (PARTITION BY Recarga.SumConsecutivo,
                                                                                                          Recarga.AssetId
                                                                                             ORDER BY Recarga.FechaHora DESC
                                                                                            ),
                                                         DATEADD( HOUR, -5, GETDATE())
                                                    ) > 30 THEN
                                           1
                                       ELSE
                                           0
                                   END
            FROM
            (
                SELECT EER.FechaHora,
                       EER.Consecutivo,
                       EER.SumConsecutivo,
                       EER.AssetId,
                       EER.Soc,
                       EER.SocInicial,
                       EER.GunConn,
                       EER.Corriente,
                       EER.Voltaje,
                       EER.Potencia,
                       EER.GunDesConn,
					   EER.FechaInicioRecarga
                FROM #ebus_EventosRecarga AS EER
            ) AS Recarga
            WHERE Recarga.Consecutivo IS NULL
                  OR Recarga.SumConsecutivo = 0
        ) AS Final
        WHERE Final.Consecutivo = Final.ConsecutivoFinal
    ) AS Filtrado
    WHERE Filtrado.Muestra = 1;
END;

