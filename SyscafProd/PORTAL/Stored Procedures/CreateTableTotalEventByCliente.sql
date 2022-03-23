-- =============================================
-- Author:      ygonzalez
-- Create Date: 18-12-2021
-- Description: Crea las tablas de total eventos por usuarioie
-- =============================================
CREATE PROCEDURE PORTAL.CreateTableTotalEventByCliente  @ClienteId AS VARCHAR(10)
AS
BEGIN




    DECLARE @SQL NVARCHAR(4000);

    SET @SQL
        = N'IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''PORTAL.TotalEventos_' + @ClienteId
          + N''') AND type in (N''U''))
			BEGIN
			CREATE TABLE PORTAL.TotalEventos_' + @ClienteId
					  + N'
			(
			TotalEventosId INT NOT NULL PRIMARY KEY IDENTITY(1,1),
			FechaHora DATETIME NOT NULL,
			AssetId BIGINT NOT NULL,
			DriverId BIGINT NOT NULL,
			EventTypeId BIGINT NOT NULL,
			Ocurrencias INT NOT NULL,
			Tiempo int NOT NULL,
			Distancia DECIMAL(18,1) NULL,
			FechaSistema DATETIME NOT NULL
			)

			CREATE INDEX portal_TotalEventos_' + @ClienteId + N' ON PORTAL.TotalEventos_' + @ClienteId
					  + N' (FechaHora)
			INCLUDE (AssetId, DriverId, EventTypeId)

			END
			'   ;

    EXEC sp_executesql @SQL;

	 SET @SQL
        = N'IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''PORTAL.TotalDistanciaRecorrida_' + @ClienteId
          + N''') AND type in (N''U''))
			BEGIN
			CREATE TABLE PORTAL.TotalDistanciaRecorrida_' + @ClienteId
					  + N'
			(
			TotalDistanciaId INT NOT NULL PRIMARY KEY IDENTITY(1,1),
			FechaHora DATETIME NOT NULL,
			AssetId BIGINT NOT NULL,
			DriverId BIGINT NOT NULL,			
			Distancia DECIMAL(18,1) NULL,
			FechaSistema DATETIME NOT NULL
			)

			CREATE INDEX portal_TotalDistanciaRecorrida_' + @ClienteId + N' ON PORTAL.TotalDistanciaRecorrida_' + @ClienteId
					  + N' (FechaHora)
			INCLUDE (AssetId, DriverId)

			END
			'   ;

    EXEC sp_executesql @SQL;

END;
