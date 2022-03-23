
-- =============================================
-- Author:     ygonzalez
-- Create Date: 27.08.2021
-- Description: ingresa y actualiza los tipos de eventos por clientes
-- =============================================


CREATE PROCEDURE PORTAL.InsertEventTypes
(
    @Clienteids INT,
    @Eventos dbo.UDT_TiposEventos READONLY
)
AS
BEGIN


    UPDATE TET
    SET TET.valueName = E.valueName,
        TET.descriptionEvent = E.descriptionEvent,
		TET.formatType = E.formatType
    FROM dbo.TB_EventType AS TET
        INNER JOIN @Eventos AS E
            ON E.eventTypeId = TET.eventTypeId
    WHERE TET.clienteIdS = @Clienteids;


    INSERT INTO dbo.TB_EventType
    (
        eventTypeId,
        clienteIdS,
        descriptionEvent,
        eventType,
        displayUnits,
        formatType,
        valueName,
		SeTieneEnBase,
        fechaSistema
    )
    SELECT CAST(eventTypeId AS VARCHAR(20)),
           clienteIdS,
           descriptionEvent,
           eventType,
           displayUnits,
           formatType,
           valueName,
		   0,
           GETDATE()
    FROM @Eventos AS E
    WHERE eventTypeId NOT IN
          (
              SELECT eventTypeId FROM dbo.TB_EventType WHERE clienteIdS = @Clienteids
          );



END;
