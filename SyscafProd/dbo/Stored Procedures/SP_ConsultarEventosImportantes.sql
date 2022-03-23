
CREATE procedure SP_ConsultarEventosImportantes  
as  
begin  
select eventTypeId from TB_EventType where SeTieneEnBase=1  
group by eventTypeId  
end