CREATE PROCEDURE SP_FiltroPorClienteDia3p                          
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
				  usuarioNombre Varchar(50) null,
				  clienteIdS int null,
				  clienteNombre Varchar(50) null,
				  total int null)

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
			,fnt.usuarioNombre
			,fnt.clienteIdS
			,fnt.clienteNombre
			,COUNT(fnt.assetId) as total 
			from dbo.FN_ConteoTransmisionDiaCliente (@dia) fnt 
			where fnt.clienteIdS=ISNULL(@clienteIdS, fnt.clienteIdS) 
			group by 
			fnt.usuarioIdS
			,fnt.usuarioNombre
			,fnt.clienteIdS
			,fnt.clienteNombre
			FETCH FECHASVALIDAS INTO @dia
			
		END
	CLOSE FECHASVALIDAS
	DEALLOCATE FECHASVALIDAS
	IF EXISTS (select top 1 * from @TABLA where usuarioIdS=@USUARIOIDS )
	BEGIN
	select 
	clienteNombre
	,clienteIdS
	from @TABLA
	where usuarioIdS=@USUARIOIDS
	group by clienteNombre, clienteIdS
	END
	ELSE
	BEGIN
	select 
	clienteNombre
	,clienteIdS
	from @TABLA
	group by clienteNombre, clienteIdS
	END	
END