using AutoMapper;
using MiX.Integrate.Shared.Entities.Assets;
using Syscaf.Common.Helpers;
using Syscaf.Data.Helpers.Auth.DTOs;
using Syscaf.Data.Models.Auth;
using Syscaf.Data.Models.Portal;
using Syscaf.Service.Automaper.MapperDTO;
using System.Collections.Generic;
using System.Linq;

namespace Syscaf.Service.Automaper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AssetBaseData, AssetResult>().ForMember(
                x => x.Resultado,
                dto => dto.MapFrom(MapearAssetDTO)
                );

            CreateMap<ApplicationUser, UsuarioDTO>().ReverseMap().ForMember(
                f => f.UserName, op => op.MapFrom( mp => mp.Email)
                );
            //CreateMap<GeneroCreacionDTO, Genero>();

           // CreateMap<Actor, ActorDTO>().ReverseMap();
            //CreateMap<ActorCreacionDTO, Actor>()
            //    .ForMember(x => x.Foto, options => options.Ignore());



            //CreateMap<IdentityUser, UsuarioDTO>();
        }

        /*Metodo que mapea el listado de assets y su configuración, haciendo un leftjoin de los  assets*/
        private List<AssetDTO> MapearAssetDTO(AssetBaseData AssetBaseData, AssetResult AssetResult)
        {
            var resultado = new List<AssetDTO>();

            if (AssetBaseData.ListaAssets != null && AssetBaseData.ListaConfiguracion != null)
            {
                resultado = (from xEntry in AssetBaseData.ListaAssets
                             join yEntryd in AssetBaseData.ListaConfiguracion on xEntry.AssetId equals yEntryd.AssetId into VehiculosConfiguracion
                             from pco in VehiculosConfiguracion.DefaultIfEmpty()
                             select new AssetDTO()
                             {
                                 AssetId = xEntry.AssetId,
                                 AssetImageUrl = xEntry.AssetImageUrl,
                                 AssetTypeId = xEntry.AssetTypeId,
                                 CreatedBy = xEntry.CreatedBy ?? "",
                                 CreatedDate = Constants.GetFechaServidor(xEntry.CreatedDate.DateTime),
                                 ConfigurationGroup = pco?.ConfigurationGroup ?? "",
                                 Description = xEntry.Description,
                                 DeviceType = pco?.DeviceType ?? "",
                                 DriverCAN = pco?.DriverCAN ?? "",
                                 DriverOBC = pco?.DriverOBC ?? "",
                                 DriverOBCLoadDate = pco?.DriverOBCLoadDate ?? "",
                                 FmVehicleId = xEntry.FmVehicleId,
                                 GPRSContext = pco?.GPRSContext ?? "",
                                 LastConfiguration = pco?.LastConfiguration ?? "",
                                 Odometer = xEntry.Odometer,
                                 RegistrationNumber = xEntry.RegistrationNumber,
                                 SiteId = xEntry.SiteId,
                                 UnitIMEI = pco?.UnitIMEI ?? "",
                                 UnitSCID = pco?.UnitSCID ?? "",
                                 UserState = xEntry.UserState ?? "",
                                 LastTrip = pco?.LastTrip ?? ""
                             }
                ).ToList();

            }

            return resultado;
        }

     
    }
}
