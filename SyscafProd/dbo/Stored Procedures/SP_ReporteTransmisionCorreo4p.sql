--/****** Object:  StoredProcedure [dbo].[SP_ReporteTransmision3p]    Script Date: 22/11/2020 1:08:48 p. m. ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
---- ygonzalez mod = los vehiculos que vuelven a transmitir se les cambia el estado a activo

CREATE PROCEDURE [dbo].[SP_ReporteTransmisionCorreo4p] 
  -- DECLARE
    @usuarioIdS INT=null, @clienteIdS INT=NULL
AS
    BEGIN
        DECLARE @TABLE TABLE
        (
		Id int not null identity(1,1) primary key ,
        assetCodigo        INT
      , registrationNumber VARCHAR(200) COLLATE Modern_Spanish_CI_AS
      , assetsDescription  VARCHAR(200)
      , diffAVL            INT
      , AVL                DATETIME
      , clientenNombre     VARCHAR(200)
      , clienteIdS         INT
      , Sitio              VARCHAR(200)
      , nombre             VARCHAR(200)
      , estado             VARCHAR(200)
        ) ;

		DECLARE @VehiculosSinTX TABLE
        (
          AssetIdS        INT not null
        , diffAVL            INT not null
        , AVL                DATETIME not null
        ) ;


        -- DANGER-- HAY QUE CAMBIARLO
        DECLARE @FechaActual DATETIME=CONVERT(
                                          DATETIME
                                        , SWITCHOFFSET(
                                          CONVERT(
                                          DATETIMEOFFSET, GETUTCDATE())
                                          , '-05:00' )) ;

        DECLARE @minuteDia DECIMAL(18, 2) =1440 ;

		--- YGONZALEZ 22/11/2020
		--- REALIZA LA CONSULTA DE LOS VEHICULOS QUE SE ENCUENTRAN SIN TRANSMISION Y CAMBIA LOS ESTADOS DE CADA /UNO
				insert into @VehiculosSinTX (  AssetIdS    , diffAVL       , AVL      ) 
				select tp.assetIdS,  DATEDIFF( DAY, MAX( tp.Timestamp ), @FechaActual ) DiasSinTx, MAx(tp.Timestamp) UltimoAvl   from dbo.TB_Positions TP
				Left join dbo.TB_Senales TS on Ts.assetsIdS = TP.assetIdS
				group by tp.assetIdS, TS.diasTransmision
				HAVING(( DATEDIFF( MINUTE, MAX( tp.Timestamp ), @FechaActual )
								   / @minuteDia )>=ISNULL( TS.diasTransmision, 1 ))


			 	-- vehiculos que ya transmite y deben cambiar estado activo
				update TA set TA.estadoSyscafIdS=3
				from dbo.TB_Assets TA
				inner join dbo.TB_Estados TE on Te.estadoIdS = Ta.estadoSyscafIdS
				where TA.estadoClienteIdS = 1 and  TE.tipoIds =3 
				and TA.assetIdS not IN(select assetIdS from @VehiculosSinTX )

				-- vehiculos que estan activos y pasaron sin tx

						UPDATE
							dbo.TB_Assets
							SET
							estadoSyscafIdS=8
						  WHERE assetIdS IN( select assetIdS from @VehiculosSinTX )
								AND estadoSyscafIdS=3 ;

				--- FIN RE VALIDACION

				INSERT INTO @TABLE
				 SELECT distinct
					 ta.assetCodigo, ta.registrationNumber, ta.assetsDescription
				   , TP.diffAVL
				   , TP.AVL, tc.clienteNombre, tc.clienteIdS
				   , ts.siteName AS 'Sitio'
				   , ( SELECT
					 TOP 1
						   TU.usuario
						  FROM dbo.TB_Empleados AS TEs
							   INNER JOIN dbo.TB_Usuarios AS TU
								   ON TU.usuarioIdS=TEs.usuarioIdS AND TEs.cargoIdS=1
							   INNER JOIN dbo.TB_GruposDeSeguridad AS TGDS
								   ON TGDS.usuarioIdS=TU.usuarioIdS
						 WHERE TEs.usuarioIdS=TU.usuarioIdS
							   AND TGDS.siteIdS=ts.siteIdS )
				   , te.estado AS estadoSyscaf
					FROM dbo.TB_Assets AS ta WITH( NOLOCK )
						 INNER JOIN  @VehiculosSinTX AS tp
							 ON( ta.assetIdS=tp.assetIdS )              
						 INNER JOIN  dbo.TB_Cliente AS tc WITH( NOLOCK )
							 ON( tc.clienteIdS=ta.clienteIdS )
						 INNER  JOIN  dbo.TB_Site AS ts WITH( NOLOCK )
							 ON( ts.siteIdS=ta.siteIdS )
						 INNER JOIN  dbo.TB_GruposDeSeguridad AS tg WITH( NOLOCK )
							 ON( tg.siteIdS=ts.siteIdS )
						 INNER JOIN  dbo.TB_Usuarios AS tu WITH( NOLOCK )
							 ON( tu.usuarioIdS=tg.usuarioIdS )
						 INNER JOIN  dbo.TB_Estados AS te WITH( NOLOCK )
							 ON( te.estadoIdS=ta.estadoSyscafIdS )
				   WHERE ta.estadoClienteIdS=1 AND ts.tipoSitio = 10
						 AND( @usuarioIdS IS NULL OR tu.usuarioIdS=@usuarioIdS )
						AND tc.clienteIdS not in (856,842) 
         
				   ORDER BY
					 tc.clienteNombre,  ts.siteName, TP.diffAVL  ;
		


				SELECT
				Id, clienteIdS,  assetCodigo, registrationNumber, assetsDescription, diffAVL, AVL
				  , REPLACE(
						REPLACE(
							REPLACE(
								REPLACE( clientenNombre, 'Col FV Itaú ', '' )
							  , 'Col FV Syscaf ', '' ), 'Col FV ', '' ), 'Itaú ', '' ) AS clientenNombre
				  , clientenNombre  Cliente, Sitio, nombre
				  , estado  estadoSyscaf
				   FROM @TABLE
				  WHERE( @clienteIdS IS NULL OR clienteIdS=@clienteIdS )
				 
				  ;
				

  END ;