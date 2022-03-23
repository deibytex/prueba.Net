CREATE procedure SP_AsignarSite
 @siteIdS int=null,
 @grupoIdS int=null,
 @tipoSitio int=null
as 
begin 
IF (@siteIdS!=0)
	BEGIN
	UPDATE TB_Site 
	set 
	grupoIdS=@grupoIdS,
	tipoSitio=@tipoSitio
	where
	siteIdS=@siteIdS
	END

	SELECT top 1 * from TB_Site where
	siteIdS=@siteIdS
end