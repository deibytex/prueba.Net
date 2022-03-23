
-- author = ygonzalez
-- date = 2021.08.19
-- description = permite adicionar informacion las tablas stage por dia para los eventos activos
CREATE PROCEDURE EBUS.AddEventActiveByDayAndClient
(
    @Day VARCHAR(10),
    @Clienteids NVARCHAR(10),
    @Eventos EBUS.UDT_ActiveEventsViaje READONLY
)
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX) = N'';
    DECLARE @Sufix NVARCHAR(20) = @Day + N'_' + @Clienteids;

    --n verifica que exista la tabla y la crea
    EXEC EBUS.CreateEventActiveByDayAndClient @Day, @Clienteids;
    -- elimina el dia anterior k
    --EXEC EBUS.DropEventActiveByDayAndClient @BeforeDayString, @Clienteids;
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
	INSERT INTO EBUS.ActiveEventsViaje_' + @Sufix
          + N'
			(
				EventId,           FechaHora,           EventTypeID,           AssetId,
           DriverId,           Altitud,           EnergiaRegenerada,           EnergiaDescargada,
           Soc,           Energia,           PorRegeneracion,           Distancia,           Localizacion,
           Latitud,           Longitud,           Autonomia,           VelocidadPromedio,           fechasistema, EsProcesado
			)
           SELECT EventId,           FechaHora,           EventTypeID,           AssetId,
           DriverId,           Altitud,           EnergiaRegenerada,           EnergiaDescargada,
           Soc,           Energia,           PorRegeneracion,           Distancia,           Localizacion,
           Latitud,           Longitud,           Autonomia,           VelocidadPromedio,           fechasistema, 0	
		   FROM @EventosActivos
				where EventId not in (select EventId from EBUS.ActiveEventsViaje_' + @Sufix
          + N')

			DELETE FROM EBUS.ActiveEventsViaje_' + @Sufix
          + N'
			WHERE EventId IN (
			SELECT TOP 1000 AER2.EventId FROM 
			EBUS.ActiveEventsViaje_' + @Sufix
          + N' AS AER2
			INNER JOIN 
			(
			SELECT  AEV.AssetId,
								   dateadd(hour, -1 ,  MAX(AEV.FechaHora)) FechaHora
							FROM EBUS.ActiveEventsViaje_' + @Sufix + N'   AS AEV
							WHERE AEV.EventTypeID = ' + @EventSocMAx
          + N'
							GROUP BY AEV.AssetId) del ON del.AssetId = AER2.AssetId 
								AND AER2.FechaHora < del.FechaHora )
	';


    EXEC sp_executesql @SQL,N'@EventosActivos EBUS.UDT_ActiveEventsViaje READONLY',  @Eventos;

END;
