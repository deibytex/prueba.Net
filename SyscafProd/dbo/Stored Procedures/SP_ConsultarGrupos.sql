  
CREATE procedure SP_ConsultarGrupos  
 @gruposIdS int =NULL  
AS       
if(@gruposIdS=0)    
 begin    
 set @gruposIdS=null    
 end    
begin      
 select     
 tg.grupoIdS,  
 tg.nombre   
 from  TB_Grupos tg     
 where      
 tg.grupoIdS=ISNULL(@gruposIdS,tg.grupoIdS)     
 order by nombre 
end