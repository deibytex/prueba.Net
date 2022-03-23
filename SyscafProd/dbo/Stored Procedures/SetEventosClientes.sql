


create procedure [dbo].[SetEventosClientes](
@ClienteIds int,
@Eventos  [UDT_EventType] readonly
)
as
begin	

--begin try

--begin transaction

 insert into [dbo].[TB_Event] (
      [EventId]
      ,[DriverId]
      ,[AssetId]
      ,[EventDateTime]
      ,[ReceivedDateTime]
      ,[EventTypeId]
      ,[OdometerKilometres]
      ,[ValueUnits]
      ,[ValueType]
      ,[ValueEvent]
      ,[SpeedLimit]
      ,[PositionId]
      ,[PriorityEvent]
      ,[Armed]
      ,[assetIdS]
      ,[driverIdS]
      ,[eventTypeIdS]
      ,[grupoIdS]
      ,[positionIdS]
      ,[fechaSistema]
      ,[estadoBase]
      ,[TotalOccurances]
      ,[TotalTimeSeconds], ClienteIds

 )


 SELECT [EventId]
      ,[DriverId]
      ,[AssetId]
      ,[EventDateTime]
      ,[ReceivedDateTime]
      ,[EventTypeId]
      ,[OdometerKilometres]
      ,[ValueUnits]
      ,[ValueType]
      ,[ValueEvent]
      ,[SpeedLimit]
      ,[PositionId]
      ,[PriorityEvent]
      ,[Armed]
      ,[assetIdS]
      ,[driverIdS]
      ,[eventTypeIdS]
      ,null
      ,null
      ,getdate()
      ,1
      ,[TotalOccurances]
      ,[TotalTimeSeconds], @clienteIds FROM @Eventos

--commit  transaction
--end try
--begin catch

--rollback transaction

--end catch

end