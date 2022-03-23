
-- author = ygonzalez
-- date = 2021.08.19
-- description = permite adicionar informacion las tablas stage por dia para los eventos activos
CREATE PROCEDURE EBUS.AddEventActiveRecargaByDayAndClient
(
    @Day VARCHAR(10),
    @Clienteids NVARCHAR(10),
    @Eventos EBUS.UDT_ActiveEventsRecarga READONLY
)
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX) = N'';
    DECLARE @Sufix NVARCHAR(20) = @Day + N'_' + @Clienteids;

    IF EXISTS (SELECT 1 FROM @Eventos)
    BEGIN


        --  SET @BeforeDayString = CONVERT(VARCHAR(15), DATEADD(DAY, -1, @Day), 112);
        --n verifica que exista la tabla y la crea
        EXEC EBUS.CreateEventActiveByDayAndClient @Day, @Clienteids;
        -- elimina el dia anterior k
        --EXEC EBUS.DropEventActiveByDayAndClient @BeforeDayString, @Clienteids;

        SET @SQL
            = N'
	INSERT INTO EBUS.ActiveEventsRecarga_' + @Sufix
              + N'
			( EventId,
          FechaHora,
          EventTypeID,
          Consecutivo,
          Carga,
          AssetId,
          DriverId,
          Soc,
          Corriente,
          Voltaje,
          Potencia,
          Energia,
          ETA,
          Odometer,
          fechasistema, EsProcesado
			)
           SELECT  EventId,
          FechaHora,
          EventTypeID,
          Consecutivo,
          Carga,
          AssetId,
          DriverId,
          Soc,
          Corriente,
          Voltaje,
          Potencia,
          Energia,
          ETA,
          Odometer,
          fechasistema , 0
		  FROM @EventosActivos
				where EventId not in (select EventId from EBUS.ActiveEventsViaje_' + @Sufix
              + N')
				
				
		DELETE FROM EBUS.ActiveEventsRecarga_' + @Sufix
              + N'
		WHERE EventId IN (
            SELECT EventId
            FROM EBUS.ActiveEventsRecarga_' + @Sufix
              + N' AS AER
                INNER JOIN
                (
                    SELECT AER.AssetId,
                           MAX(AER.FechaHora) FechaHora
                    FROM EBUS.ActiveEventsRecarga_' + @Sufix
              + N' AS AER
                    WHERE AER.Consecutivo = 1
                    GROUP BY AER.AssetId
                ) AS filter
                    ON filter.AssetId = AER.AssetId
                       AND AER.FechaHora < filter.FechaHora)		
				';

        EXEC sp_executesql @SQL,
                           N'@EventosActivos EBUS.UDT_ActiveEventsRecarga READONLY',
                           @Eventos;

        EXEC EBUS.SetActiveRecargaHistoricalByClient @Day = @Day, -- varchar(10)
                                                     @Clienteids = @Clienteids;

    END;

END;
