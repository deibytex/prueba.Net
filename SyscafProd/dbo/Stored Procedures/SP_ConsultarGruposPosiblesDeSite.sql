     
 CREATE procedure SP_ConsultarGruposPosiblesDeSite    
 @clienteIdS int =null    
 as    
 begin    
  select     
  ts.grupoIdS,  
  (select tg.nombre from TB_Grupos tg where tg.grupoIdS=ts.grupoIdS ) AS nombreGrupo  
  from TB_Site ts     
  where ts.clienteIdS=@clienteIdS    
  group by grupoIdS    
  union all
 select 
 tg.grupoIdS,
 tg.nombre as nombreGrupo
 from TB_Grupos tg where tg.grupoIdS not in (select ts.grupoIdS from TB_Site ts group by ts.grupoIdS)

 end