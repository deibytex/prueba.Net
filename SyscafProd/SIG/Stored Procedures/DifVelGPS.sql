-- =============================================
-- Author:      dlopez
-- Create Date: 2021.12.02
-- Description: 
-- =============================================

CREATE  PROCEDURE SIG.DifVelGPS
(
    @CondicionId INT,
    @EventTypeId VARCHAR(MAX),
    @Valor DECIMAL(18, 2),
    @Ocurrencias INT,
    @Distancia DECIMAL(18, 2),
    @OperadorId INT,
    @Clienteids VARCHAR(MAX),
    @CondTrips VARCHAR(MAX),
    @CondEvent VARCHAR(MAX),
    @FallaSenialId INT,
    @Descripcion VARCHAR(MAX)
)
AS
BEGIN

    DECLARE @Sql AS NVARCHAR(MAX);

    IF @CondicionId = 2 --- Diferencia velocidad FM y velocidad GPS
    BEGIN

        SET @Sql
            = N'	  

					INSERT INTO [SIG].[TB_DataSeniales]
						(
							CondicionId,
                            FallaSenialId,	
                            Descripcion,				
							AssetIds,
							ClienteIds,
							FechaInicial,
							FechaFinal,
                            Ocurrencias,
							FechaSistema,
							EsActivo
						)
					  SELECT @CondicionId, @FallaSenialId, @Descripcion, assetId, @ClienteIds,
							 MIN(Tripstart), MAX(TripEnd), Ocurrencias = COUNT(1),
                             GETDATE(), 1
					  FROM 
                      (SELECT * FROM
                        (SELECT TE.assetId, TE.maxSpeedKilometersPerHour,
                            TE.tripStart, TE.tripEnd, MAX(EV.Value) AS maxgps, TE.distanceKilometers 
                        FROM #ViajesSenales AS TE
                          INNER JOIN (SELECT * FROM #EventsSenales AS ES  
                          WHERE ES.EventTypeId IN (' + @EventTypeId
              + N')) AS EV ON (EV.assetId = TE.assetId 
                        AND EV.driverId = TE.driverId 
                        AND EV.StartDateTime BETWEEN TE.TripStart AND TE.TripEnd)
                        GROUP BY TE.assetId, TE.driverId, TE.maxSpeedKilometersPerHour,
                        TE.tripStart, TE.tripEnd, TE.distanceKilometers) AS Final	
                    WHERE ' + @CondEvent + N' @Valor AND ' + @CondTrips
              + N' @Distancia) AS Senal
                    GROUP BY assetId 
                    HAVING COUNT(1) > @Ocurrencias';

    END;

    EXEC sp_executesql @Sql,
                       N'@Ocurrencias INT , @CondicionId INT ,@ClienteIds INT, @Distancia DECIMAL(10,2)
                               , @Valor INT, @FallaSenialId INT, @Descripcion VARCHAR(MAX)',
                       @Ocurrencias,
                       @CondicionId,
                       @Clienteids,
                       @Distancia,
                       @Valor,
                       @FallaSenialId,
                       @Descripcion;

END;
