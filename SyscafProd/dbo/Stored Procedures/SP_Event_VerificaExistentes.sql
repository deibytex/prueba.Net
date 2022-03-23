
-- =============================================
-- Author:		ygonzalez
-- Create date: 30/03/2021
-- Description:	Devuelve los eventos que no se encuentren en la base de datos 
-- =============================================
CREATE PROCEDURE [dbo].[SP_Event_VerificaExistentes]
(
@ClienteIds as int, 
@tipo as int, -- indica si verifica 1.viajes, 2.eventos o 3.metricas,
@dias as datetime, -- indica cuantos días atrás se realiza la verificación
@Ids [UDT_TableIdentity] READONLY
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Verifica que no se hayan insertado estos eventos en los últimos 2 meses


	if( @tipo = 1)
	begin
		select Id from @Ids
		where cast(Id as varchar) not in (    
		select [EventId] from tb_Event e
		where e.clienteIds = @ClienteIds and e.EventDateTime >= dateadd(day, -30, @dias))
	end
	else
		if (@Tipo = 2)
		begin
			select Id from @Ids
			where cast(Id as varchar) not in (    
			select TripId from [dbo].[TB_SubViaje] e
			where e.clienteIds = @ClienteIds and e.TripStart >=dateadd(day, -30 , @dias)
			)
		end
		else
		if (@Tipo = 3)
		begin
			select Id from @Ids
			where Id  not in (    
			select TripId from [dbo].[TB_TripsMetrics] e
			where e.clienteIds = @ClienteIds and e.TripStart >=dateadd(day,- 30 , @dias))
		end

END