
---- author = ygonzalez
---- date = 2021.08.19
---- description = trae los ultimos eventos activos por conductor
----EBUS.GetEventActiveViajeByDayAndClient '102021','914'
CREATE PROCEDURE EBUS.GetEventActiveViajeByDayAndClient
(
    @Day VARCHAR(10),
    @Clienteids NVARCHAR(10)
)
AS
BEGIN


    SET ARITHABORT OFF;
    SET ANSI_WARNINGS OFF;

    --DECLARE @Day VARCHAR(10) = '122021',
    --      @Clienteids NVARCHAR(10) = N'914';
    DECLARE @SQL NVARCHAR(4000) = N'';
    DECLARE @Sufix NVARCHAR(20) = @Day + N'_' + @Clienteids;




    IF OBJECT_ID('tempdb..#EBUS_TempEventosActivos') IS NOT NULL
        DROP TABLE #EBUS_TempEventosActivos;
    CREATE TABLE #EBUS_TempEventosActivos
    (
        FechaHora DATETIME NOT NULL,
        EventTypeID BIGINT NULL,
        AssetId BIGINT NOT NULL,
        DriverId BIGINT NOT NULL,
        Mark INT NOT NULL,
        Consecutivo INT NULL,
        EnergiaRegenerada DECIMAL(18, 4) NULL,
        EnergiaDescargada DECIMAL(18, 4) NULL,
        Soc DECIMAL(18, 4) NULL,
        Energia DECIMAL(18, 4) NULL,
        PorRegeneracion DECIMAL(18, 4) NULL,
        Distancia DECIMAL(18, 4) NULL,
        Localizacion VARCHAR(250) NULL,
        Latitud FLOAT NULL,
        Longitud FLOAT NULL
    );

    DECLARE @EventSocMAx NVARCHAR(30) = N'8816511971478625922';

    SELECT @EventSocMAx = P.Valor
    FROM EBUS.Parametrizacion AS P
    WHERE P.TipoParametroId IN
          (
              SELECT TDL.DetalleListaId
              FROM dbo.TB_DetalleListas AS TDL
              WHERE TDL.Sigla = 'EBUS_SOCMAX'
          )
          AND P.ClienteIds = CAST(@Clienteids AS INT);
    SET @SQL
        = N'	         
insert into #EBUS_TempEventosActivos(FechaHora, Mark, Consecutivo, AssetId,  EnergiaRegenerada,EnergiaDescargada,Soc,Localizacion,Latitud,Longitud,Distancia, DriverId)
  SELECT  AEV.FechaHora,
               Mark = CASE AEV.EventTypeID
                          WHEN ' + @EventSocMAx
          + N' THEN
                              1
                          ELSE
                              0
                      END,
               Consecutivo = SUM(   CASE AEV.EventTypeID
                                        WHEN ' + @EventSocMAx
          + N' THEN
                                            1
                                        ELSE
                                            0
                                    END
                                ) OVER (PARTITION BY AEV.AssetId ORDER BY AEV.FechaHora),
               AEV.AssetId,             
               AEV.EnergiaRegenerada,
               AEV.EnergiaDescargada,
               AEV.Soc,        
               AEV.Localizacion,
               AEV.Latitud,
               AEV.Longitud,
			   AEV.Distancia, AEV.DriverId
        FROM EBUS.ActiveEventsViaje_' + @Sufix
          + N' AS AEV
            INNER JOIN
            (
                SELECT AEV.AssetId,
                       MAX(AEV.FechaHora) FechaHora
                FROM EBUS.ActiveEventsViaje_' + @Sufix + N' AS AEV
                WHERE AEV.EventTypeID = ' + @EventSocMAx
          + N'
                GROUP BY AEV.AssetId
            ) AS Filter
                ON Filter.AssetId = AEV.AssetId
                   AND AEV.FechaHora >= Filter.FechaHora';



    EXEC sp_executesql @SQL;


    SELECT Fecha = Final.FechaHora,
           FechaString = CONVERT(VARCHAR(20), Final.FechaHora, 103) + ' ' + CONVERT(VARCHAR(20), Final.FechaHora, 108),
           Final.AssetId,
           Placa = TA.assetsDescription,
           Driver = TD.name,
           Kms = Final.Diferencia,
           Final.Localizacion,
           Soc = CASE WHEN (PARSENAME(Final.Soc,1) > 0 AND @Clienteids <> 910) THEN Final.Soc * 10 ELSE Final.Soc END ,
           Energia = Final.EnergiaDescargada - Final.EnergiaRegenerada,
           PorRegeneracion = Final.EnergiaRegenerada / CASE (Final.EnergiaDescargada)
                                                                   WHEN 0 THEN
                                                                       1
                                                                   ELSE
                                                                       Final.EnergiaDescargada
                                                               END * 100,
           Final.EnergiaRegenerada,
           Final.EnergiaDescargada,
           Final.Latitud,
           Final.Longitud,
           Altitud = 0.0,
           Eficiencia = Final.Diferencia / (Final.EnergiaDescargada - Final.EnergiaRegenerada),
           Odometro,
           Final.Mark
    FROM
    (
        SELECT Position = ROW_NUMBER() OVER (PARTITION BY Procesado.AssetId ORDER BY Procesado.FechaHora DESC),
               Procesado.FechaHora,
               Procesado.Consecutivo,
               Procesado.AssetId,
               Procesado.DriverId,
               Odometro = Procesado.Distancia,
               Diferencia = Procesado.Distancia
                            - FIRST_VALUE(Procesado.Distancia) OVER (PARTITION BY Procesado.Consecutivo,
                                                                                  Procesado.AssetId
                                                                     ORDER BY Procesado.FechaHora
                                                                    ),
               Procesado.Localizacion,
               Procesado.Soc,
               EnergiaRegenerada = CASE  WHEN (PARSENAME(Procesado.Soc,1) > 0 AND @Clienteids <> 910)  THEN Procesado.EnergiaRegenerada / 10 ELSE Procesado.EnergiaRegenerada END ,
               Procesado.EnergiaDescargada,
               Procesado.Latitud,
               Procesado.Longitud,               
               Procesado.Mark
        FROM
        (
            SELECT ETEA.FechaHora,                   
                   ETEA.AssetId,
                   ETEA.DriverId,
                   ETEA.Mark,
                   ETEA.Consecutivo,
                   EnergiaRegenerada = MAX(ETEA.EnergiaRegenerada),
                   EnergiaDescargada = MAX(ETEA.EnergiaDescargada),
                   Soc = MAX(ETEA.Soc),
                   Energia = MAX(ETEA.Energia),
                   Distancia = MAX(ETEA.Distancia),
                   Localizacion = MAX(ETEA.Localizacion),
                   Latitud = MAX(ETEA.Latitud),
                   Longitud = MAX(ETEA.Longitud)
            FROM #EBUS_TempEventosActivos AS ETEA
            GROUP BY ETEA.FechaHora,
                     ETEA.AssetId,
                     ETEA.Consecutivo,
                     ETEA.DriverId,
                     ETEA.Mark
        ) AS Procesado
        WHERE Procesado.Mark = 1
              OR
              (
                  (Procesado.Soc IS NOT NULL
                  AND Procesado.EnergiaDescargada IS NOT NULL
                  AND Procesado.EnergiaRegenerada IS NOT NULL ) OR @Clienteids = 910
              ) 
    ) AS Final
        INNER JOIN PORTAL.TB_Drivers AS TD
            ON TD.DriverId = Final.DriverId
        INNER JOIN dbo.TB_Assets AS TA
            ON TA.assetId = Final.AssetId
    WHERE Final.Position = 1;

END;




