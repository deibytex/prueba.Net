-- =============================================
-- Author:      dlopez
-- Create Date: 21.01.2022
-- Description: trae el RAV

--EXEC FIDELIZACION.SP_Rav 834,-1, '20220101', '20220117', -1, 1
CREATE PROCEDURE FIDELIZACION.SP_Rav
(
    @ClienteIds INT,
    @SiteId BIGINT,
    @FechaInicial DATETIME,
    @FechaFinal DATETIME,
    @GerenciaId BIGINT,
    @Tipo INT
)
AS
BEGIN

IF OBJECT_ID('tempdb..#RAVSitesGerencias') IS NOT NULL
        DROP TABLE #RAVSitesGerencias
    CREATE TABLE #RAVSitesGerencias  
    (
        Tipo INT,
        SiteId BIGINT,
		nombre VARCHAR(200)
    );


   

    IF OBJECT_ID('tempdb..#RAVBaseCond') IS NOT NULL
        DROP TABLE #RAVBaseCond;
    CREATE TABLE #RAVBaseCond
    (
        GerenciaId BIGINT,
        Siteids INT,
        Gerencias VARCHAR(200),
        Regional VARCHAR(200),
        DriverId BIGINT,
        name VARCHAR(MAX),
        Aceleraciones INT,
        Revoluciones INT,
        Velocidad INT,
        Frenadas INT,
        Ralenti INT,
        Total INT
    );

    CREATE INDEX idxtempbasecond ON #RAVBaseCond (DriverId);

    IF OBJECT_ID('tempdb..#RAVfinalCond') IS NOT NULL
        DROP TABLE #RAVfinalCond;
    CREATE TABLE #RAVfinalCond
    (
        GerenciaId BIGINT,
        Siteids INT,
        Gerencias VARCHAR(200),
        Regional VARCHAR(200),
        name VARCHAR(MAX),
        Aceleraciones INT,
        Revoluciones INT,
        Velocidad INT,
        Frenadas INT,
        Ralenti INT,
        Total INT,
        Tiempo INT,
        Distancia DECIMAL(11, 4)
    );

    CREATE INDEX idxtempfinalcond
    ON #RAVfinalCond (GerenciaId)
    INCLUDE (
                Aceleraciones,
                Frenadas,
                Tiempo
            );

    -- traemos la informaci[on de las bases de los vehiculos los cuales se va a realizar la cuenta de los eventos
    -- Gerencias Vehiculo 

    INSERT INTO #RAVSitesGerencias
    (
        Tipo,
        SiteId, nombre
    )
    SELECT 1,
           CAST(TDL.Valor AS BIGINT), TDL.Nombre
    FROM dbo.TB_DetalleListas AS TDL
        INNER JOIN dbo.TB_Listas AS TL
            ON TL.ListaId = TDL.ListaId
    WHERE TL.Sigla = 'GERCOND';

    -- se insertan los sites en segundo nivel de conductor
    INSERT INTO #RAVSitesGerencias
    (
        Tipo,
        SiteId, nombre
    )
    SELECT 1,
           TS.siteId, TS.siteName
    FROM dbo.TB_Site AS TS
    WHERE TS.clienteIdS = @ClienteIds
          AND (TS.sitePadreId IN
               (
                   SELECT SG.SiteId FROM #RAVSitesGerencias AS SG
               )
              );

    DECLARE @SQLScript NVARCHAR(MAX);

    SET @SQLScript
        = N' 
    -- insert a la tabla 
    INSERT INTO #RAVBaseCond
        (
        GerenciaId,
        Siteids,
        Gerencias,
        Regional,
        DriverId,
        name,
        Aceleraciones,
        Revoluciones,
        Velocidad,
        Frenadas,
        Ralenti,
        Total
        )
    SELECT PivotTable.GerenciaId,
        PivotTable.siteIdS,
        Gerencia = TS3.nombre,
        Regional = TS2.nombre,
        PivotTable.DriverId,
        PivotTable.name,
        ISNULL(SUM(PivotTable.[6454149451280645233]), 0) AS Aceleraciones,
        ISNULL(SUM(PivotTable.[-7372181092478897411]), 0) AS Revolucuiones,
        ISNULL(SUM(PivotTable.[-3890646499157906515]), 0) AS Velocidad,
        ISNULL(SUM(PivotTable.[4750800303282680186]), 0) AS Frenadas,
        ISNULL(SUM(PivotTable.[4650840888823746694]), 0) AS Ralentí,
        SUM(ISNULL(PivotTable.[4650840888823746694], 0) + ISNULL(PivotTable.[-3890646499157906515], 0)
                       + ISNULL(PivotTable.[4750800303282680186], 0) + ISNULL(PivotTable.[6454149451280645233], 0)
                       + ISNULL(PivotTable.[-7372181092478897411], 0)
                      ) AS Total
    FROM
        (
            SELECT GerenciaId = TS.siteId,
            TS.siteIdS,
            TS.sitePadreId,
            ta.DriverId,
            TA.name,
            TE.EventTypeId,
            Total = SUM(TE.Ocurrencias)
        FROM PORTAL.TotalEventos_' + CAST(@ClienteIds AS VARCHAR)
          + N' AS TE
            INNER JOIN PORTAL.TB_Drivers AS TA
            ON TA.DriverId = TE.DriverId
            INNER JOIN dbo.TB_Site AS TS
            ON TS.siteIdS = TA.siteIdS
        WHERE TE.FechaHora
                      BETWEEN @FechaI AND @FechaF
            AND (@SiteId = -1 OR TS.sitePadreId = @SiteId)
        GROUP BY 
                         TS.sitePadreId,
                         TS.siteId,
                         TE.EventTypeId,
                         TS.siteIdS,
                         TA.DriverId,
                         TA.name
            ) AS total
            PIVOT
            (
                SUM(Total)
                FOR EventTypeId IN ([4650840888823746694], [4750800303282680186], [6454149451280645233],
                                    [-3890646499157906515], [-7372181092478897411]
                                   )
            ) AS PivotTable
        INNER JOIN #RAVSitesGerencias AS TS2
        ON TS2.siteId = PivotTable.sitePadreId
        INNER JOIN #RAVSitesGerencias AS TS3
        ON TS3.siteId = PivotTable.GerenciaId
    WHERE (
                      @SiteId = -1
        OR CAST(TS2.siteId AS BIGINT) = @SiteId
        AND(@GerenciaId = -1 OR GerenciaId = @GerenciaId)
                  )
    GROUP BY PivotTable.GerenciaId,
                     PivotTable.sitePadreId,
                     TS3.nombre,
                     TS2.nombre,
                     PivotTable.siteIdS,
                     PivotTable.DriverId,
                     PivotTable.name;';

    EXEC sp_executesql @SQLScript,
                       N'@FechaI DATETIME, @FechaF DATETIME, @SiteId BIGINT, @GerenciaId BIGINT',
                       @FechaInicial,
                       @FechaFinal,
                       @SiteId,
                       @GerenciaId;

    DECLARE @FechaDiaInicial DATE = DATEADD(DAY, 1, EOMONTH(@FechaInicial, -1));
    DECLARE @FechaDiaFinal DATE = DATEADD(DAY, 1, EOMONTH(@FechaFinal, -1));

    DECLARE @Periodo VARCHAR(7);


    -- While para sumar todas las distancias necesarias. 
    WHILE (@FechaDiaInicial <= @FechaDiaFinal)
    BEGIN

        --Creamos periodo a conultar en viajes
        SET @Periodo
            = CAST(DATEPART(MONTH, @FechaDiaInicial) AS VARCHAR) + CAST(DATEPART(YEAR, @FechaDiaInicial) AS VARCHAR);

        -- insert tabla final
        SET @SQLScript
            = N' 
             INSERT INTO #RAVfinalCond
        (
        GerenciaId,
        Siteids,
        Gerencias,
        Regional,
        name,
        Aceleraciones,
        Revoluciones,
        Velocidad,
        Frenadas,
        Ralenti,
        Total,
        Tiempo,
        Distancia
        )
    SELECT
        BR.GerenciaId
            , BR.Siteids
            , BR.Gerencias
            , BR.Regional
            , BR.name
            , BR.Aceleraciones
            , BR.Revoluciones
            , BR.Velocidad
            , BR.Frenadas
            , BR.Ralenti
            , BR.Total
            , ISNULL((SELECT SUM(ER.Tiempo)
        FROM PORTAL.TotalEventos_' + CAST(@ClienteIds AS VARCHAR)
              + N' AS ER
        WHERE DriverId = BR.DriverId AND EventTypeId = -3890646499157906515), 0)
            , SUM(TR.distancia)
    FROM #RAVBaseCond AS BR
        INNER JOIN PORTAL.TotalDistanciaRecorrida_' + CAST(@ClienteIds AS VARCHAR)
              + N' (NOLOCK) AS TR
        ON (TR.driverId = BR.DriverId)
    WHERE TR.FechaHora BETWEEN @FechaI AND @FechaF    
    GROUP BY BR.Siteids, BR. GerenciaId, BR.Gerencias, BR.Regional, BR.name , BR.Aceleraciones
            , BR.Revoluciones
            , BR.Velocidad
            , BR.Frenadas
            , BR.Ralenti
            , BR.Total
            , BR.DriverId
            ';
			
			PRINT @SQLScript
        EXEC sp_executesql @SQLScript,
                           N'@FechaI datetime, @FechaF DateTime',
                           @FechaInicial,
                           @FechaFinal;

        SET @FechaDiaInicial = DATEADD(MONTH, 1, @FechaDiaInicial);
    END;


    IF (@Tipo = 0) -- ordena por puntuación
    BEGIN

        SELECT name,
               Gerencias,
               Regional,
               MAX(Aceleraciones) AS Aceleraciones,
               MAX(Revoluciones) AS Revoluciones,
               MAX(Velocidad) AS Velocidad,
               MAX(Frenadas) AS Frenadas,
               MAX(Ralenti) AS Ralenti,
               MAX(Total) AS Total,
               MAX(Tiempo) AS Tiempo,
               SUM(Distancia) AS Distancia,
               Puntuacion = ROUND(
                                     ((MAX(Aceleraciones) / SUM(Distancia)) * 100)
                                     + ((MAX(Frenadas) / SUM(Distancia)) * 100) + ((MAX(Tiempo) / SUM(Distancia)) * 10),
                                     2
                                 )
        FROM #RAVfinalCond
        WHERE Distancia > 0
        GROUP BY name,
                 Gerencias,
                 Regional
        ORDER BY Puntuacion ASC;


    END;
    ELSE IF (@Tipo = 1) -- ordena por evento
    BEGIN
        SELECT name,
               Gerencias,
               Regional,
               MAX(Aceleraciones) AS Aceleraciones,
               MAX(Revoluciones) AS Revoluciones,
               MAX(Velocidad) AS Velocidad,
               MAX(Frenadas) AS Frenadas,
               MAX(Ralenti) AS Ralenti,
               MAX(Total) AS Total,
               MAX(Tiempo) AS Tiempo,
               SUM(Distancia) AS Distancia,
               Puntuacion = ROUND(
                                     ((MAX(Aceleraciones) / SUM(Distancia)) * 100)
                                     + ((MAX(Frenadas) / SUM(Distancia)) * 100) + ((MAX(Tiempo) / SUM(Distancia)) * 10),
                                     2
                                 )
        FROM #RAVfinalCond
        WHERE Distancia > 0
        GROUP BY name,
                 Gerencias,
                 Regional
        ORDER BY MAX(Total) ASC;
    END;
    ELSE IF (@Tipo = 2) -- ordena por top puntuacion 
    BEGIN
        SELECT *
        FROM
        (
            SELECT TOP (5)
                   name,
                   Gerencias,
                   Regional,
                   MAX(Aceleraciones) AS Aceleraciones,
                   MAX(Revoluciones) AS Revoluciones,
                   MAX(Velocidad) AS Velocidad,
                   MAX(Frenadas) AS Frenadas,
                   MAX(Ralenti) AS Ralenti,
                   MAX(Total) AS Total,
                   MAX(Tiempo) AS Tiempo,
                   SUM(Distancia) AS Distancia,
                   Puntuacion = ROUND(
                                         ((MAX(Aceleraciones) / SUM(Distancia)) * 100)
                                         + ((MAX(Frenadas) / SUM(Distancia)) * 100)
                                         + ((MAX(Tiempo) / SUM(Distancia)) * 10),
                                         2
                                     )
            FROM #RAVfinalCond
            WHERE Distancia > 0
            GROUP BY name,
                     Gerencias,
                     Regional
            ORDER BY Puntuacion ASC
        ) AS top5
        UNION
        SELECT *
        FROM
        (
            SELECT TOP (5)
                   name,
                   Gerencias,
                   Regional,
                   MAX(Aceleraciones) AS Aceleraciones,
                   MAX(Revoluciones) AS Revoluciones,
                   MAX(Velocidad) AS Velocidad,
                   MAX(Frenadas) AS Frenadas,
                   MAX(Ralenti) AS Ralenti,
                   MAX(Total) AS Total,
                   MAX(Tiempo) AS Tiempo,
                   SUM(Distancia) AS Distancia,
                   Puntuacion = ROUND(
                                         ((MAX(Aceleraciones) / SUM(Distancia)) * 100)
                                         + ((MAX(Frenadas) / SUM(Distancia)) * 100)
                                         + ((MAX(Tiempo) / SUM(Distancia)) * 10),
                                         2
                                     )
            FROM #RAVfinalCond
            WHERE Distancia > 0
            GROUP BY name,
                     Gerencias,
                     Regional
            ORDER BY Puntuacion DESC
        ) AS down5
        ORDER BY Puntuacion ASC;
    END;
    ELSE -- ordena por top evento
    BEGIN
        SELECT *
        FROM
        (
            SELECT TOP (5)
                   name,
                   Gerencias,
                   Regional,
                   MAX(Aceleraciones) AS Aceleraciones,
                   MAX(Revoluciones) AS Revoluciones,
                   MAX(Velocidad) AS Velocidad,
                   MAX(Frenadas) AS Frenadas,
                   MAX(Ralenti) AS Ralenti,
                   Total = MAX(Total),
                   MAX(Tiempo) AS Tiempo,
                   SUM(Distancia) AS Distancia,
                   Puntuacion = ROUND(
                                         ((MAX(Aceleraciones) / SUM(Distancia)) * 100)
                                         + ((MAX(Frenadas) / SUM(Distancia)) * 100)
                                         + ((MAX(Tiempo) / SUM(Distancia)) * 10),
                                         2
                                     )
            FROM #RAVfinalCond
            WHERE Distancia > 0
            GROUP BY name,
                     Gerencias,
                     Regional
            ORDER BY MAX(Total) ASC
        ) AS top5
        UNION
        SELECT *
        FROM
        (
            SELECT TOP (5)
                   name,
                   Gerencias,
                   Regional,
                   MAX(Aceleraciones) AS Aceleraciones,
                   MAX(Revoluciones) AS Revoluciones,
                   MAX(Velocidad) AS Velocidad,
                   MAX(Frenadas) AS Frenadas,
                   MAX(Ralenti) AS Ralenti,
                   Total = MAX(Total),
                   MAX(Tiempo) AS Tiempo,
                   SUM(Distancia) AS Distancia,
                   Puntuacion = ROUND(
                                         ((MAX(Aceleraciones) / SUM(Distancia)) * 100)
                                         + ((MAX(Frenadas) / SUM(Distancia)) * 100)
                                         + ((MAX(Tiempo) / SUM(Distancia)) * 10),
                                         2
                                     )
            FROM #RAVfinalCond
            WHERE Distancia > 0
            GROUP BY name,
                     Gerencias,
                     Regional
            ORDER BY Total DESC
        ) AS down5
        ORDER BY Total ASC;

    END;
END;
