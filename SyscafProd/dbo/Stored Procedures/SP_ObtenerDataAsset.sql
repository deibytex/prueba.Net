CREATE PROCEDURE [dbo].[SP_ObtenerDataAsset]
(
    -- Add the parameters for the stored procedure here
    @assetId varchar(50) = NULL
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    SELECT assetCodigo, assetsDescription, groupId, siteIdS FROM TB_Assets
	WHERE assetId = @assetId
END