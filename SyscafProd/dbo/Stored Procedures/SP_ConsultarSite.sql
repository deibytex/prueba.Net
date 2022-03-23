CREATE procedure SP_ConsultarSite     
  @siteIdS int =NULL     
 as     
 begin    
 select top 10     
 ts.siteIdS,    
 ts.siteId,    
 ts.siteName,    
 ts.clienteIdS,    
 ts.sitePadreId,    
 ts.tipoSitio,    
 ts.grupoIdS,    
 (select COUNT(ta.assetIdS) from TB_Assets ta where ta.siteIdS=ts.siteIdS) as cantidadVehiculos,    
 (SELECT tc.clienteNombre FROM TB_Cliente tc where tc.clienteIdS=ts.clienteIdS) as clienteNombre,    
 (SELECT tss.siteName FROM TB_Site tss where tss.siteIdS=ts.siteIdS ) as sitePadreNombre,    
 (SELECT te.estado FROM  TB_Estados te where te.estadoIdS=ts.tipoSitio) as tipoSitioNombre,    
 (SELECT tg.nombre FROM  TB_Grupos tg where tg.grupoIdS=ts.grupoIdS)  as grupoNombre    
 from TB_site ts    
 where ts.siteIdS=ISNULL(@siteIdS, ts.siteIdS)       
 and ts.tipoSitio!=9  
 order by siteName,tipoSitio,clienteNombre
 end