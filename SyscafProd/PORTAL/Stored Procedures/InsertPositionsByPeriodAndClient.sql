
-- author = ygonzalez
-- date = 2021.06.22
-- description = guarda información final dee las tablas

CREATE PROCEDURE PORTAL.InsertPositionsByPeriodAndClient
(
    @Period NVARCHAR(10),
    @Clienteids NVARCHAR(10),
    @DataPositions PORTAL.UDT_Positions READONLY
)
AS
BEGIN
    DECLARE @SQL NVARCHAR(4000) = N'';

    --SET @SQL = N' TRUNCATE TABLE PORTAL.TB_Positions_' + @Period + N'_' + @Clienteids;



    SET @SQL
        = N' 	

Insert into [Portal].[TB_Positions_' + @Period + N'_' + @Clienteids
          + N'](
	[PositionId]
      ,[assetId]
      ,[driverId]
      ,[Timestamp]
      ,[Longitud]
      ,[Latitud]
      ,[FormattedAddress]
      ,[AltitudeMetres]
      ,[NumberOfSatellites]
      ,[ClienteIds]
      ,[fechasistema]
	)
	select [PositionId]
      ,[assetId]
      ,[driverId]
      ,[Timestamp]
      ,[Longitude]
      ,[Latitude]
      ,[FormattedAddress]
      ,[AltitudeMetres]
      ,[NumberOfSatellites]
      ,[ClienteIds]
      ,[fechasistema] from @data
	';
    EXEC sp_executesql @SQL,
                       N'@data [PORTAL].[UDT_Positions] readonly',
                       @DataPositions;


    SET @SQL
        = N' DELETE FROM PORTAL.TB_Positions_' + @Period + N'_' + @Clienteids
          + N'
			WHERE PositionId NOT IN (
			SELECT MAX(TP.PositionId) FROM PORTAL.TB_Positions_' + @Period + N'_' + @Clienteids
          + N' AS TP
			GROUP BY TP.assetId)';
    EXEC sp_executesql @SQL;

END;

