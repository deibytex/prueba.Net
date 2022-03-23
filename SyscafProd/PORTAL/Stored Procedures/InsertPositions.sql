

CREATE PROCEDURE [PORTAL].[InsertPositions] (

 @Period NVARCHAR(10),
 @DataPositions [PORTAL].[UDT_Positions] READONLY
 )
 AS
BEGIN
DECLARE @SQL NVARCHAR(4000) = '';



	SET @SQL = '
Insert into [Portal].[TB_Positions_'+@Period+'](
	[PositionId]
      ,[assetId]
      ,[driverId]
      ,[Timestamp]
      ,[Longitude]
      ,[Latitude]
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
	'
	EXEC sp_executesql @SQL,N'@data [PORTAL].[UDT_Positions] readonly',@DataPositions
	

END

