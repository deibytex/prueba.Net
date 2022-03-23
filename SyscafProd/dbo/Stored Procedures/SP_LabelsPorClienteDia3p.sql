CREATE PROCEDURE [dbo].[SP_LabelsPorClienteDia3p]                         
 @DESDE DATETIME =null,                        
 @HASTA DATETIME =null,                     
 @INTERVALOS int =NULL,
 @USUARIOIDS INT = NULL,
 @clienteIdS int =null
AS                                
BEGIN
	DECLARE @dia VARCHAR(50)
	DECLARE 
	@TABLA TABLE (fecha Varchar(10) null,
				  usuarioIdS int null,
				  clienteNombre Varchar(50) null)
	DECLARE FECHASVALIDAS CURSOR 
	FOR  (select fechas from FN_FechasIntervalos(@DESDE,@HASTA,@INTERVALOS))
	OPEN FECHASVALIDAS
	FETCH FECHASVALIDAS INTO @dia
	WHILE @@FETCH_STATUS=0
		BEGIN 
		insert into @TABLA
			select 
			@dia
			,fnt.usuarioIdS			
			,fnt.clienteNombre			
			from dbo.FN_ConteoTransmisionDiaCliente (@dia) fnt 
			where fnt.clienteIdS=ISNULL(@clienteIdS, fnt.clienteIdS) 
			group by 
			 fnt.usuarioIdS
			,fnt.clienteNombre
			FETCH FECHASVALIDAS INTO @dia			
		END
	CLOSE FECHASVALIDAS
	DEALLOCATE FECHASVALIDAS
	IF (@clienteIdS is null)
	begin
	IF EXISTS (select top 1 * from @TABLA where usuarioIdS=@USUARIOIDS )
	BEGIN	
	select
	clienteNombre	
	from @TABLA
	where usuarioIdS=@USUARIOIDS
	group by clienteNombre
	END
	ELSE
	BEGIN
	select 
	clienteNombre
	from @TABLA
	group by clienteNombre
	END	
	end
	else
	begin
	IF EXISTS (select top 1 * from @TABLA where usuarioIdS=@USUARIOIDS )
	BEGIN
	select
	fecha as clienteNombre	
	from @TABLA
	where usuarioIdS=@USUARIOIDS
	group by fecha
	END	
	ELSE
	BEGIN
	select 
	fecha as clienteNombre
	from @TABLA
	group by fecha
	END
	end
END