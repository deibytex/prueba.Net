
  
CREATE procedure [dbo].[SP_ConsultarDriver]  
as   
begin   
select td.driverId,tc.clienteId from TB_Drivers td , TB_Site ts, TB_Cliente tc
where tc.clienteIdS=ts.clienteIdS
and ts.siteIdS=td.siteIdS
and tc.estadoClienteIdS=1
end