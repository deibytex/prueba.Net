CREATE PROCEDURE SP_ConsultarTipoIntervalos
@tipo int = null
AS      
BEGIN      
SELECT * FROM TB_Intervalos WHERE intervaloIdS = @tipo
END