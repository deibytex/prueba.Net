/****** Object:  StoredProcedure [EBUS].[CreateEventActiveByDayAndClient]    Script Date: 30/08/2021 11:14:19 a. m. ******/

-- author = ygonzalez
-- date = 2021.08.19
-- description = permite crear las tablas stage por dia para los eventos activos
-- EBUS.CreateEventActiveByDayAndClient '82021', '858'
CREATE PROCEDURE [EBUS].[CreateEventActiveByDayAndClient] 
(
    @Day NVARCHAR(10),
    @Clienteids NVARCHAR(10)
)
AS
BEGIN
    DECLARE @SQL NVARCHAR(4000) = N'';  
	DECLARE @SQLViaje NVARCHAR(4000) = N'';  
    DECLARE @Sufix NVARCHAR(20) = @Day + N'_' + @Clienteids;



    SET @SQL
        = N'IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''EBUS.[ActiveEventsViaje_' + @Sufix
          + N']'') AND type in (N''U''))
		BEGIN
		CREATE TABLE EBUS.[ActiveEventsViaje_' + @Sufix
				  + N'](
			[EventId] bigint NOT NULL primary key,		
			FechaHora datetime not null,
			EventTypeID bigint null,	
			[AssetId] [bigint] NOT NULL,	
			[DriverId] [bigint] NOT NULL,	
			[Altitud] [decimal](18, 4)  NULL,	
			[EnergiaRegenerada] [decimal](18, 4)  NULL,
			[EnergiaDescargada] [decimal](18, 4)  NULL,
			[Soc] [decimal](18, 4)  NULL,	
			[Energia] [decimal](18, 4)  NULL,
			PorRegeneracion  [decimal](18, 4)  NULL,
			[Distancia] [decimal](18, 4)  NULL,
			[Localizacion] varchar(250)  NULL,
			[Latitud] float NULL,	
			[Longitud] float NULL,
			[Autonomia] [decimal](18, 4)  NULL,	
			[VelocidadPromedio] [decimal](18, 4) NULL,	
			EsProcesado bit not null,
			fechasistema datetime not null
			)
	
			CREATE INDEX FechaHoraEvento_ActiveEventsViaje_' + @Sufix + N'  ON EBUS.[ActiveEventsViaje_' + @Sufix + N'] (FechaHora, EventTypeID) 

			END
			';

	
	 EXEC sp_executesql @SQL;
	   SET @SQLViaje
        = N'IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''EBUS.[ActiveEventsRecarga_' + @Sufix
          + N']'') AND type in (N''U''))
			begin
			CREATE TABLE EBUS.[ActiveEventsRecarga_' + @Sufix + N'](
				[EventId] bigint NOT NULL primary key,		
				FechaHora datetime not null,
				EventTypeID bigint null,
				Consecutivo int null,
				Carga int null,
				[AssetId] [bigint] NOT NULL,	
				[DriverId] [bigint] NOT NULL,		
				[Soc] [decimal](18, 4)  NULL,	
				Corriente decimal(18,4) null,
				Voltaje decimal(18,4)  null,
				Potencia decimal(18,4)  null,
				Energia  decimal(18,4)  null,
				ETA decimal(18,4)  null, 
				Odometer decimal(18,2) null,
				EsProcesado bit not null,
				fechasistema datetime not null
				)
	
			CREATE INDEX FechaHoraEvento_ActiveEventsRecarga_' + @Sufix + N'  ON EBUS.[ActiveEventsViaje_' + @Sufix + N'] (FechaHora, EventTypeID) 
	
			end	';

    EXEC sp_executesql @SQLViaje;

END;
