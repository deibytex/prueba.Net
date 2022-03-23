
create PROCEDURE [dbo].[SP_AgrupControlSyscafDinamico]
AS
BEGIN
	SELECT 
		YEAR(fechareporte) AS [Year],
		MONTH(fechareporte) AS [Month]
	FROM 
		[dbo].[TB_ReporteDashboard]
	GROUP BY 
		YEAR(fechareporte), 
		MONTH(fechareporte)
END