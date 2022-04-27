using AutoMapper;
using MiX.Integrate.Shared.Entities.Assets;
using Syscaf.Common.Helpers;
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
            //CreateMap<GeneroCreacionDTO, Genero>();

            //CreateMap<Actor, ActorDTO>().ReverseMap();
            //CreateMap<ActorCreacionDTO, Actor>()
            //    .ForMember(x => x.Foto, options => options.Ignore());

            //CreateMap<Cine, CineDTO>()
            //    .ForMember(x => x.Latitud, dto => dto.MapFrom(campo => campo.Ubicacion.Y))
            //    .ForMember(x => x.Longitud, dto => dto.MapFrom(campo => campo.Ubicacion.X));

            //CreateMap<PeliculaCreacionDTO, Pelicula>()
            //    .ForMember(x => x.Poster, opciones => opciones.Ignore())
            //    .ForMember(x => x.PeliculasGeneros, opciones => opciones.MapFrom(MapearPeliculasGeneros))
            //    .ForMember(x => x.PeliculasCines, opciones => opciones.MapFrom(MapearPeliculasCines))
            //    .ForMember(x => x.PeliculasActores, opciones => opciones.MapFrom(MapearPeliculasActores));

            //CreateMap<Pelicula, PeliculaDTO>()
            //   .ForMember(x => x.Generos, options => options.MapFrom(MapearPeliculasGeneros))
            //   .ForMember(x => x.Actores, options => options.MapFrom(MapearPeliculasActores))
            //   .ForMember(x => x.Cines, options => options.MapFrom(MapearPeliculasCines));

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
