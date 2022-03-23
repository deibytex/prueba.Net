
-- =============================================
-- Author:      ygonzalez
-- Create Date: 12/11/2021
-- Description: crea tabla dispcriminado por period  para 
-- los mensajes del modulo ITS
-- =============================================

--EXEC ITS.CreateTableMessageByPeriod '112021'
CREATE PROCEDURE ITS.CreateTableMessageByPeriod
(@Periodo VARCHAR(6))
AS
BEGIN

    DECLARE @SQL NVARCHAR(400);
    SET @SQL
        = N'IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[ITS].[Messages_' + @Periodo
          + N']'') AND type in (N''U''))

			CREATE TABLE [ITS].[Messages_' + @Periodo + N'](
				[FechaHora] [DATETIME] NOT NULL PRIMARY KEY,
				[Mensaje] [VARCHAR](MAX) NULL,
				EsProcesado Bit NOT NULL default(0),
				FechaProcesado Datetime NULL,
				ProfileData VARCHAR(max)
				)
			' ;

    EXEC sp_executesql @SQL;

END;



