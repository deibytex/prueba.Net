-- =============================================
-- Author:      ygonzalez
-- Create Date: 2022.02.03
-- Description: trae el rav desagregado
-- =============================================

--EXEC FIDELIZACION.SP_Rav 834,-1, '20220101', '20220117', -1, 1
CREATE PROCEDURE FIDELIZACION.SP_RavDesagregado
(
    @ClienteIds INT,
    @SiteId BIGINT = NULL,
    @FechaInicial DATETIME,
    @FechaFinal DATETIME,
    @GerenciaId BIGINT = NULL,
    @Tipo INT = NULL
)
AS
BEGIN

    --DECLARE @ClienteIds INT = 834,
    --        @SiteId BIGINT = -1,
    --        @FechaInicial DATETIME = '20220101',
    --        @FechaFinal DATETIME = '20220117',
    --        @GerenciaId BIGINT = -1,
    --        @Tipo INT = 1;

    DECLARE @SitesGerencias AS TABLE
    (
        name VARCHAR(200),
        SiteId BIGINT
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

    INSERT INTO @SitesGerencias
    (
        name,
        SiteId
    )
    SELECT TDL.Nombre,
           CAST(TDL.Valor AS BIGINT)
    FROM dbo.TB_DetalleListas AS TDL
        INNER JOIN dbo.TB_Listas AS TL
            ON TL.ListaId = TDL.ListaId
    WHERE TL.Sigla = 'GERCOND';



    -- se insertan los sites en segundo nivel de conductor
    INSERT INTO @SitesGerencias
    (
        name,
        SiteId
    )
    SELECT TS.siteName,
           TS.siteId
    FROM dbo.TB_Site AS TS
    WHERE TS.clienteIdS = @ClienteIds
          AND (TS.sitePadreId IN
               (
                   SELECT SG.SiteId FROM @SitesGerencias AS SG
               )
              );



    SELECT Gerencia = TS.siteName,
           Regional = TS2.name,
           TE.EventTypeId,
           TD.DriverId,
           TD.name,
           EventO = TET.descriptionEvent,
           Total = SUM(TE.Ocurrencias),
           Tiempo = SUM(TE.Tiempo),
           Distancia = MAX(TDR.Distancia)
    FROM PORTAL.TotalEventos_834 AS TE
        INNER JOIN PORTAL.TotalDistanciaRecorrida_834 AS TDR
            ON TDR.DriverId = TE.DriverId
        INNER JOIN PORTAL.TB_Drivers AS TD
            ON TD.DriverId = TE.DriverId
        INNER JOIN dbo.TB_Site AS TS
            ON TS.siteIdS = TD.siteIdS
        INNER JOIN @SitesGerencias AS TS2
            ON TS2.SiteId = TS.sitePadreId
        INNER JOIN dbo.TB_EventType AS TET
            ON TET.eventTypeId = TE.EventTypeId
    WHERE TE.FechaHora
          BETWEEN @FechaInicial AND @FechaFinal
          AND TS.siteId IN
              (
                  SELECT SG.SiteId FROM @SitesGerencias AS SG
              )
          AND TET.clienteIdS = @ClienteIds
    GROUP BY TS.sitePadreId,
             TS2.name,
             TS.siteName,
             TD.DriverId,
             TD.name,
             TET.descriptionEvent,
             TE.EventTypeId,
             TE.Distancia;









END;
