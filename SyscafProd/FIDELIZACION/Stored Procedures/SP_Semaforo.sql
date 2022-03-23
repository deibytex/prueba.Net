-- =============================================
-- Author:      dlopez
-- Create Date: 21.01.2022
-- Description: trae el RAV

--EXEC FIDELIZACION.SP_Semaforo 834,-1,'20220101', '20220117', 
CREATE PROCEDURE [FIDELIZACION].[SP_Semaforo]
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

    -- DECLARE @ClienteIds INT = 834,
    -- @SiteId BIGINT = 6450340602836511566,
    -- @FechaInicial DATETIME = '20211101',
    -- @FechaFinal DATETIME = '20220112',
    -- @GerenciaId BIGINT = 4209718049609148653
    -- @Tipo INT = 0

    DECLARE @SitesGerencias AS TABLE
    (
        Tipo INT,
        SiteId BIGINT
    );

    IF OBJECT_ID('tempdb..#SemaforoBase') IS NOT NULL
        DROP TABLE #SemaforoBase;
    CREATE TABLE #SemaforoBase
    (
        GerenciaId BIGINT,
        Siteids INT,
        Regional  VARCHAR(200), 
        Gerencias VARCHAR(200),
        Aceleraciones INT,
        Frenadas INT,
        Velocidad INT,
        Ralenti INT,
        Total INT
    );

    CREATE INDEX idxtempbasesem
    ON #SemaforoBase (Siteids);

    IF OBJECT_ID('tempdb..#SemaforoBaseCond') IS NOT NULL
        DROP TABLE #SemaforoBaseCond;
    CREATE TABLE #SemaforoBaseCond
    (
        GerenciaId BIGINT,
        Siteids INT,
        Gerencias VARCHAR(200),
        name VARCHAR(MAX),
        Aceleraciones INT,
        Frenadas INT,
        Velocidad INT,
        Ralenti INT,
        Total INT
    );

    CREATE INDEX idxtempbasesemcond
    ON #SemaforoBaseCond (GerenciaId);

    -- traemos la informaci[on de las bases de los vehiculos los cuales se va a realizar la cuenta de los eventos
    -- Gerencias Vehiculo 

    INSERT INTO @SitesGerencias
    (
        Tipo,
        SiteId
    )
    SELECT 1,
           CAST(TDL.Valor AS BIGINT)
    FROM dbo.TB_DetalleListas AS TDL
        INNER JOIN dbo.TB_Listas AS TL
            ON TL.ListaId = TDL.ListaId
    WHERE TL.Sigla = 'GERCOND';

    -- se insertan los sites en segundo nivel de conductor
    INSERT INTO @SitesGerencias
    (
        Tipo,
        SiteId
    )
    SELECT 1,
           TS.siteId
    FROM dbo.TB_Site AS TS
    WHERE TS.clienteIdS = @ClienteIds
          AND (TS.sitePadreId IN
               (
                   SELECT SG.SiteId FROM @SitesGerencias AS SG
               )
              );

    DECLARE @SQLScript NVARCHAR(MAX);

    IF (@Tipo = 0) -- agrupa  por mes y por eventos
    BEGIN

    SET @SQLScript
            = N' 
        INSERT INTO #SemaforoBase
        (
            GerenciaId,
            Siteids,
            Regional,
            Gerencias,
            Aceleraciones,
            Frenadas,
            Velocidad,
            Ralenti,
            Total
        )
        SELECT PivotTable.GerenciaId,
               PivotTable.siteIdS,
               Regional = ts2.siteName,
               Gerencia = TS3.siteName,
               ISNULL(SUM(PivotTable.[6454149451280645233]), 0) AS Aceleraciones,
               ISNULL(SUM(PivotTable.[4750800303282680186]), 0) AS Frenadas,
               ISNULL(SUM(PivotTable.[-3890646499157906515]), 0) AS EVelocidad,
               ISNULL(SUM(PivotTable.[4650840888823746694]), 0) AS Ralenti,
               SUM(ISNULL(PivotTable.[4650840888823746694], 0) + ISNULL(PivotTable.[-3890646499157906515], 0)
                   + ISNULL(PivotTable.[4750800303282680186], 0) + ISNULL(PivotTable.[6454149451280645233], 0)
                  ) AS TotalEventos
        FROM
        (
            SELECT GerenciaId = TS.siteId,
                   TS.siteIdS,
                   TS.sitePadreId,
                   TE.EventTypeId,
                   Total = SUM(TE.Ocurrencias)
            FROM PORTAL.TotalEventos_' + CAST(@clienteIdS AS VARCHAR)
              + N' AS TE
                INNER JOIN PORTAL.TB_Drivers AS TA
                    ON TA.DriverId = TE.DriverId
                INNER JOIN dbo.TB_Site AS TS
                    ON TS.siteIdS = TA.siteIdS
            WHERE TE.FechaHora
                  BETWEEN @FechaI AND @FechaF
                   AND (@SiteId = -1 OR TS.sitePadreId = @SiteId)
            GROUP BY MONTH(TE.FechaHora),
                     YEAR(TE.FechaHora),
                     TS.sitePadreId,
                     TS.siteId,
                     TE.EventTypeId,
                     TS.siteIdS
        ) AS total
        PIVOT
        (
            SUM(Total)
            FOR EventTypeId IN ([4650840888823746694], [4750800303282680186], [6454149451280645233],
                                [-3890646499157906515]
                               )
        ) AS PivotTable
            INNER JOIN dbo.TB_Site AS TS2
                ON TS2.siteId = PivotTable.sitePadreId
            INNER JOIN dbo.TB_Site AS TS3
                ON TS3.siteId = PivotTable.GerenciaId
        WHERE (
                  @SiteId = -1
                  OR CAST(TS2.siteId AS BIGINT) = @SiteId
              )
        GROUP BY PivotTable.GerenciaId,
                 PivotTable.sitePadreId,
                 TS2.siteName,
                 TS3.siteName,
                 PivotTable.siteIdS;'

        EXEC sp_executesql @SQLScript,
                        N'@FechaI DATETIME, @FechaF DATETIME, @SiteId BIGINT',
                        @FechaInicial,
                        @FechaFinal,
                        @SiteId;         

        SELECT S.Regional,
               S.Gerencias,
               S.Aceleraciones,
               S.Frenadas,
               S.Velocidad,
               S.Ralenti,
               Total,
               COUNT(D.DriverId) AS Personas,
               Puntaje = Total / COUNT(D.DriverId)
        FROM #SemaforoBase AS S
            INNER JOIN PORTAL.TB_Drivers AS D
                ON (D.siteIdS = S.Siteids)
        GROUP BY S.Regional,
                 S.Gerencias,
                 S.Aceleraciones,
                 S.Frenadas,
                 S.Velocidad,
                 S.Ralenti,
                 S.Total
        ORDER BY S.Regional, Puntaje;
    END;
    ELSE
    BEGIN
        IF (@Tipo = 1) --divido por regional - gerencia - evento
        BEGIN
         SET @SQLScript
            = N' 
            INSERT INTO #SemaforoBaseCond
            (
                GerenciaId,
                Siteids,
                Gerencias,
                name,
                Aceleraciones,
                Frenadas,
                Velocidad,
                Ralenti,
                Total
            )
            SELECT PivotTable.GerenciaId,
                   PivotTable.siteIdS,
                   Gerencia = TS3.siteName,
                   PivotTable.name,
                   ISNULL(SUM(PivotTable.[6454149451280645233]), 0) AS Aceleraciones,
                   ISNULL(SUM(PivotTable.[4750800303282680186]), 0) AS Frenadas,
                   ISNULL(SUM(PivotTable.[-3890646499157906515]), 0) AS Velocidad,
                   ISNULL(SUM(PivotTable.[4650840888823746694]), 0) AS Ralenti,
                   SUM(ISNULL(PivotTable.[4650840888823746694], 0) + ISNULL(PivotTable.[-3890646499157906515], 0)
                       + ISNULL(PivotTable.[4750800303282680186], 0) + ISNULL(PivotTable.[6454149451280645233], 0)
                      ) AS TotalEventos
            FROM
            (
                SELECT GerenciaId = TS.siteId,
                       TS.siteIdS,
                       TS.sitePadreId,
                       TA.name,
                       TE.EventTypeId,
                       Total = SUM(TE.Ocurrencias)
                FROM PORTAL.TotalEventos_' + CAST(@clienteIdS AS VARCHAR)
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
                         TA.name
            ) AS total
            PIVOT
            (
                SUM(Total)
                FOR EventTypeId IN ([4650840888823746694], [4750800303282680186], [6454149451280645233],
                                    [-3890646499157906515]
                                   )
            ) AS PivotTable
                INNER JOIN dbo.TB_Site AS TS2
                    ON TS2.siteId = PivotTable.sitePadreId
                INNER JOIN dbo.TB_Site AS TS3
                    ON TS3.siteId = PivotTable.GerenciaId
            WHERE (
                      @SiteId = -1
                      OR CAST(TS2.siteId AS BIGINT) = @SiteId
                         AND(@GerenciaId = -1 or GerenciaId = @GerenciaId)
                  )
            GROUP BY PivotTable.GerenciaId,
                     PivotTable.sitePadreId,
                     TS3.siteName,
                     PivotTable.siteIdS,
                     PivotTable.name;'

            EXEC sp_executesql @SQLScript,
                        N'@FechaI DATETIME, @FechaF DATETIME, @SiteId BIGINT, @GerenciaId BIGINT',
                        @FechaInicial,
                        @FechaFinal,
                        @SiteId,
                        @GerenciaId;


            SELECT 
                   ROW_NUMBER() OVER(ORDER BY [Total] ASC) AS num, 
                   name,
                   Aceleraciones,
                   Frenadas,
                   Velocidad,
                   Ralenti,
                   Total
            FROM #SemaforoBaseCond
            ;
        END;
    END;
END;
