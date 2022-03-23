
-- =============================================
-- Author:      ygonzalez
-- Create Date: 28.12.2021
-- Description: trae la informacion desagregada por meses, por gerencias,  de coductor y vehiculos
-- Tipo 0 = Informacion agregada total por eventos y por mes Regional
-- Tipo 2 = Informacion agregada total por Regional  por mes
-- Tipo 4 = Informacion agregada total por Regional  por eventos   y mes
-- Tipo 1 = Informacion dividida por Regional  gerencias y  meses 
-- =============================================

--EXEC PORTAL.GetInformacionEventosPorCliente 834,3,-1,'20211201','20220112'
--EXEC PORTAL.GetInformacionEventosPorCliente 834,1,-1,'20220101','20220124'
--EXEC PORTAL.GetInformacionEventosPorCliente 834,2,-1,'20211101','20220112'
--EXEC PORTAL.GetInformacionEventosPorCliente 834,4,-1,'20211101','20220112'

CREATE PROCEDURE PORTAL.GetInformacionEventosPorCliente
(
    @ClienteIds INT = NULL,
    @Tipo INT = NULL,
    @SiteId BIGINT = NULL,
    @FechaInicial DATETIME,
    @FechaFinal DATETIME
)
AS
BEGIN


    DECLARE @EventsFiltados AS TABLE
    (
        EventTypeId BIGINT NOT NULL
    );

    INSERT INTO @EventsFiltados
    (
        EventTypeId
    )
    SELECT CEI.EventTypeId
    FROM PORTAL.ConfiguracionEventosIMG AS CEI
    WHERE CEI.ClienteIds = @ClienteIds;



    DECLARE @SitesGerencias AS TABLE
    (
        Tipo INT,
        SiteId BIGINT,
        Nombre VARCHAR(150),
        SitePadreId BIGINT
    );

    -- traemos la informaci[on de las bases de los vehiculos los cuales se va a realizar la cuenta de los eventos
    -- Gerencias Vehiculo 

    INSERT INTO @SitesGerencias
    (
        Tipo,
        SiteId,
        Nombre
    )
    SELECT 1,
           CAST(TDL.Valor AS BIGINT),
           TDL.Nombre
    FROM dbo.TB_DetalleListas AS TDL
        INNER JOIN dbo.TB_Listas AS TL
            ON TL.ListaId = TDL.ListaId
    WHERE TL.Sigla = 'GERCOND';


    -- se insertan los sites en segundo nivel de conductor
    INSERT INTO @SitesGerencias
    (
        Tipo,
        SiteId,
        Nombre,
        SitePadreId
    )
    SELECT 1,
           TS.siteId,
           TS.siteName,
           TS.sitePadreId
    FROM dbo.TB_Site AS TS
    WHERE TS.clienteIdS = @ClienteIds
          AND (TS.sitePadreId IN
               (
                   SELECT SG.SiteId FROM @SitesGerencias AS SG
               )
              );


    IF (@Tipo = 0) -- agrupa  por mes y por eventos
    BEGIN

        SELECT Periodo = DATEADD(MONTH, DATEDIFF(MONTH, 0, MIN(TE.FechaHora)), 0),
               MONTH(TE.FechaHora) Mes,
               YEAR(TE.FechaHora) Anio,
               TET.descriptionEvent Evento,
               SUM(TE.Ocurrencias) Total
        FROM PORTAL.TotalEventos_834 AS TE
            INNER JOIN
            (
                SELECT eventTypeId,
                       descriptionEvent
                FROM dbo.TB_EventType
                WHERE clienteIdS = 834
            ) AS TET
                ON TET.eventTypeId = TE.EventTypeId
        WHERE TE.FechaHora
              BETWEEN @FechaInicial AND @FechaFinal
              AND TE.EventTypeId IN
                  (
                      SELECT EF.EventTypeId FROM @EventsFiltados AS EF
                  )
        GROUP BY MONTH(TE.FechaHora),
                 YEAR(TE.FechaHora),
                 TET.descriptionEvent
        ORDER BY MONTH(TE.FechaHora);

    END;
    ELSE
    BEGIN
        IF (@Tipo = 1) --divido por regional - gerencia - evento
        BEGIN
            SELECT Periodo = DATEADD(MONTH, DATEDIFF(MONTH, 0, (total.FechaHora)), 0),
                   total.Mes,
                   total.Anio,
                   Regional = TS2.Nombre,
                   Gerencia = TS3.siteName,
                   Evento = TET.descriptionEvent,
                   total.Total,
                   Usuarios =
                   (
                       SELECT COUNT(TD.DriverId)
                       FROM PORTAL.TB_Drivers AS TD
                       WHERE TD.ClienteIds = @ClienteIds
                             AND TD.siteIdS = TS3.siteIdS
                   )
            FROM
            (
                SELECT MIN(TE.FechaHora) FechaHora,
                       MONTH(TE.FechaHora) Mes,
                       YEAR(TE.FechaHora) Anio,
                       GerenciaId = TS.siteId,
                       TS.sitePadreId,
                       TE.EventTypeId,
                       Total = SUM(TE.Ocurrencias)
                FROM PORTAL.TotalEventos_834 AS TE
                    INNER JOIN PORTAL.TB_Drivers AS TA
                        ON TA.DriverId = TE.DriverId
                    INNER JOIN dbo.TB_Site AS TS
                        ON TS.siteIdS = TA.siteIdS
                WHERE TE.FechaHora
                      BETWEEN @FechaInicial AND @FechaFinal
                      AND
                      (
                          @SiteId = -1
                          OR TS.sitePadreId = @SiteId
                      )
                      AND TE.EventTypeId IN
                          (
                              SELECT EF.EventTypeId FROM @EventsFiltados AS EF
                          )
                GROUP BY MONTH(TE.FechaHora),
                         YEAR(TE.FechaHora),
                         TS.sitePadreId,
                         TS.siteId,
                         TE.EventTypeId
            ) AS total
                INNER JOIN @SitesGerencias AS TS2
                    ON TS2.SiteId = total.sitePadreId
                INNER JOIN dbo.TB_Site AS TS3
                    ON TS3.siteId = total.GerenciaId
                INNER JOIN
                (
                    SELECT eventTypeId,
                           descriptionEvent
                    FROM dbo.TB_EventType
                    WHERE clienteIdS = @ClienteIds
                ) AS TET
                    ON TET.eventTypeId = total.EventTypeId
            WHERE (
                      @SiteId = -1
                      OR CAST(TS2.SiteId AS BIGINT) = @SiteId
                  )
            ORDER BY total.Mes;
        END;
        ELSE
        BEGIN
            IF (@Tipo = 2)
            BEGIN
                SELECT Periodo = DATEADD(MONTH, DATEDIFF(MONTH, 0, (total.FechaHora)), 0),
                       total.Mes,
                       total.Anio,
                       Regional = TS2.Nombre,
                       total.Total
                FROM
                (
                    SELECT MIN(TE.FechaHora) FechaHora,
                           MONTH(TE.FechaHora) Mes,
                           YEAR(TE.FechaHora) Anio,
                           TS.sitePadreId,
                           SUM(TE.Ocurrencias) Total
                    FROM PORTAL.TotalEventos_834 AS TE
                        INNER JOIN PORTAL.TB_Drivers AS TA
                            ON TA.DriverId = TE.DriverId
                        INNER JOIN dbo.TB_Site AS TS
                            ON TS.siteIdS = TA.siteIdS
                    WHERE TE.FechaHora
                          BETWEEN @FechaInicial AND @FechaFinal
                          AND TE.EventTypeId IN
                              (
                                  SELECT EF.EventTypeId FROM @EventsFiltados AS EF
                              )
                    GROUP BY MONTH(TE.FechaHora),
                             YEAR(TE.FechaHora),
                             TS.sitePadreId
                ) AS total
                    INNER JOIN @SitesGerencias AS TS2
                        ON TS2.SiteId = total.sitePadreId
                WHERE (
                          @SiteId = -1
                          OR CAST(TS2.SiteId AS BIGINT) = @SiteId
                      )
                ORDER BY total.Mes;

            END;
            ELSE
            BEGIN
                IF (@Tipo = 4) --gerencias vehiculos y eventos 
                BEGIN


                    SELECT Periodo = DATEADD(MONTH, DATEDIFF(MONTH, 0, (total.FechaHora)), 0),
                           total.Mes,
                           total.Anio,
                           Regional = TS2.Nombre,
                           Evento = TET.descriptionEvent,
                           total.Total
                    FROM
                    (
                        SELECT MIN(TE.FechaHora) FechaHora,
                               MONTH(TE.FechaHora) Mes,
                               YEAR(TE.FechaHora) Anio,
                               TS.sitePadreId,
                               TE.EventTypeId,
                               Total = SUM(TE.Ocurrencias)
                        FROM PORTAL.TotalEventos_834 AS TE
                            INNER JOIN PORTAL.TB_Drivers AS TA
                                ON TA.DriverId = TE.DriverId
                            INNER JOIN dbo.TB_Site AS TS
                                ON TS.siteIdS = TA.siteIdS
                        WHERE TE.FechaHora
                              BETWEEN @FechaInicial AND @FechaFinal
                              AND TE.EventTypeId IN
                                  (
                                      SELECT EF.EventTypeId FROM @EventsFiltados AS EF
                                  )
                        GROUP BY MONTH(TE.FechaHora),
                                 YEAR(TE.FechaHora),
                                 TS.sitePadreId,
                                 TE.EventTypeId
                    ) AS total
                        INNER JOIN @SitesGerencias AS TS2
                            ON TS2.SiteId = total.sitePadreId
                        INNER JOIN
                        (
                            SELECT eventTypeId,
                                   descriptionEvent
                            FROM dbo.TB_EventType
                            WHERE clienteIdS = 834
                        ) AS TET
                            ON TET.eventTypeId = total.EventTypeId
                    WHERE (
                              @SiteId = -1
                              OR CAST(TS2.SiteId AS BIGINT) = @SiteId
                          )
                    ORDER BY total.Mes;

                END;
                ELSE IF (@Tipo = 3) -- agrupa  por mes y por eventos
                BEGIN



                    SELECT Periodo = TE.FechaHora,
                           MONTH(TE.FechaHora) Mes,
                           YEAR(TE.FechaHora) Anio,
                           TET.descriptionEvent Evento,
                           SUM(TE.Ocurrencias) Total
                    FROM PORTAL.TotalEventos_834 AS TE
                        INNER JOIN
                        (
                            SELECT eventTypeId,
                                   descriptionEvent
                            FROM dbo.TB_EventType
                            WHERE clienteIdS = 834
                        ) AS TET
                            ON TET.eventTypeId = TE.EventTypeId
                    WHERE TE.FechaHora
                          BETWEEN @FechaInicial AND @FechaFinal
                          AND TE.EventTypeId IN
                              (
                                  SELECT EF.EventTypeId FROM @EventsFiltados AS EF
                              )
                    GROUP BY TE.FechaHora,
                             TET.descriptionEvent;

                END;
                ELSE IF (@Tipo = 5) -- trae informacion por regional, eventos y total conductores, distancia
                BEGIN



                    SELECT Periodo = TE.FechaHora,
						   Regional = sg.Nombre,
                           MONTH(TE.FechaHora) Mes,
                           YEAR(TE.FechaHora) Anio,
                           TET.descriptionEvent Evento,
                           SUM(TE.Ocurrencias) Total
                    FROM PORTAL.TotalEventos_834 AS TE
                        INNER JOIN
                        (
                            SELECT eventTypeId,
                                   descriptionEvent
                            FROM dbo.TB_EventType
                            WHERE clienteIdS = @ClienteIds
                        ) AS TET
                            ON TET.eventTypeId = TE.EventTypeId
                    INNER JOIN PORTAL.TB_Drivers AS TD ON TD.DriverId = TE.DriverId
					INNER JOIN dbo.TB_Site AS TS ON TS.siteIdS = TD.siteIdS
					INNER JOIN @SitesGerencias AS SG ON SG.SiteId = TS.siteId
                    
                    WHERE TE.FechaHora
                          BETWEEN @FechaInicial AND @FechaFinal
                          AND TE.EventTypeId IN
                              (
                                  SELECT EF.EventTypeId FROM @EventsFiltados AS EF
                              )
                    GROUP BY TE.FechaHora,
                             TET.descriptionEvent, SG.Nombre;

                END;;

            END;
        END;
    END;
END;
