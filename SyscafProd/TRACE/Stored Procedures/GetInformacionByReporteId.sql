
-- =============================================
-- Author:     ygonzalez
-- Create Date: 27/07/2021
-- Description: Trae los odometros por periodo
-- =============================================
--TRACE.[GetInformacionByReporteId] '20210401', '20210701', 43,2
CREATE PROCEDURE [TRACE].[GetInformacionByReporteId] (@FechaInicial AS datetime,
@Fechafinal AS datetime,
@ReporteId AS int,
@Tipo AS int = NULL)
AS
BEGIN


  IF (@ReporteId = 41)
  BEGIN

    SELECT
      [Fecha],
      [AssetIds],
      [MaximoOdometro]
    FROM [TRACE].[UltimoOdometro]
    WHERE [Fecha] BETWEEN @FechaInicial AND @Fechafinal

  END
  ELSE
  BEGIN

    IF (@ReporteId = 45)
    BEGIN

      SELECT
        assetids,
        Fecha,
        Hour,
        totalRalenti
      FROM [TRACE].[RalentiFranjaHoraria]
      WHERE [Fecha] BETWEEN @FechaInicial AND @Fechafinal

    END
    ELSE
    BEGIN

      IF (@ReporteId = 44)
      BEGIN

        SELECT
          assetids,
          Fecha,
          CombustibleEvento,
          CombustibleViaje
        FROM [TRACE].[Ralenti]
        WHERE [Fecha] BETWEEN @FechaInicial AND @Fechafinal

      END
      ELSE
      BEGIN

        IF (@ReporteId = 40)
        BEGIN

          SELECT
            Fecha,
            AssetIds,
            FuelUsedLitres,
            HorasMotor,
            DistanciaRecorrida,
            PorRalenti
          FROM [TRACE].Rendimiento
          WHERE [Fecha] BETWEEN @FechaInicial AND @Fechafinal

        END
        ELSE
        BEGIN

          IF (@ReporteId IN (42, 43))
          BEGIN

            SELECT
              Fecha,
              TotalTimeSeconds,
              AssetIds,
              EventTypeId
            FROM [TRACE].VelocidadOperacion
            WHERE [Fecha] BETWEEN @FechaInicial AND @Fechafinal
            AND tipo = @Tipo

          END
          ELSE
          BEGIN

            IF (@ReporteId IN (48, 50))
            BEGIN

              SELECT
                Fecha,
                EventTypeId,
                AssetIds,
                Fechasistema,
                TotalTimeSeconds,
                distanceKilometers,
                TotalOccurances
              FROM [TRACE].Safeti
              WHERE [Fecha] BETWEEN @FechaInicial AND @Fechafinal

            END
            ELSE
            BEGIN
              --// trae la informacion del consolidado de las distancias diarias por vehiculoi
              IF (@ReporteId = -1)
              BEGIN

                SELECT
                  Fecha,
                  AssetIds,
                  distanceKilometers,
                  FuelUsedLitres,
                  EngineSeconds
                FROM [TRACE].ConsolidadoViajes
                WHERE [Fecha] BETWEEN @FechaInicial AND @Fechafinal

              END
              ELSE
              BEGIN
                --// trae la informacion mecanikal skill
                IF (@ReporteId in (46, 47, 49))
                BEGIN

                  SELECT
                    Fecha, EventTypeId, TotalOcurrencias,TotalTimeSeconds, AssetIds
                  FROM [TRACE].TotalEventCount
                  WHERE [Fecha] BETWEEN @FechaInicial AND @Fechafinal
				  and clasificacion  = @ReporteId

                END
               
              END


            END


          END


        END

      END

    END
  END

END


--46	10	Mechanical Skill
--47	10	Pedales
--49	10	Alarmas