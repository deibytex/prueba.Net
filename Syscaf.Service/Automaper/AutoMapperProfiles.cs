﻿using AutoMapper;
using MiX.Integrate.Shared.Entities.Drivers;
using MiX.Integrate.Shared.Entities.Groups;
using MiX.Integrate.Shared.Entities.LibraryEvents;
using MiX.Integrate.Shared.Entities.Trips;
using Syscaf.Common.Helpers;
using Syscaf.Data.Helpers.Auth.DTOs;
using Syscaf.Data.Models.Auth;
using Syscaf.Data.Models.Portal;
using Syscaf.Service.Automaper.MapperDTO;
using Syscaf.Service.Portal.Models;
using System;
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

            CreateMap<ApplicationUser, UsuarioDTO>().ReverseMap()
                .ForMember(f => f.UserName, op => op.MapFrom(mp => mp.Email));

            CreateMap<GroupSummary, SiteResult>().ForMember(
               x => x.Resultado,
               dto => dto.MapFrom(MapearSites)
               );

            CreateMap<LibraryEvent, EventTypeDTO>();

            CreateMap<Driver, DriverDTO>()
                .ForMember(f => f.aditionalFields, op => op.MapFrom(MapearAditionalDetailsFields));

            CreateMap<Group, ClienteSaveDTO>()
                 .ForMember(f => f.clienteNombre, op => op.MapFrom(mp => mp.Name))
                   .ForMember(f => f.clienteId, op => op.MapFrom(mp => mp.GroupId));

            CreateMap<ClienteSaveDTO, ClienteDTO>();

            CreateMap<Trip,TripsNew>()
                 .ForMember(f => f.StartPositionId, op => op.MapFrom(mp => mp.StartPositionId.ToString()))
                 .ForMember(f => f.EndPositionId, op => op.MapFrom(mp => mp.EndPositionId.ToString()))
                 .ForMember(f => f.TripEnd, op => op.MapFrom(mp => Constants.GetFechaServidor(mp.TripEnd)))
                 .ForMember(f => f.TripStart, op => op.MapFrom(mp => Constants.GetFechaServidor(mp.TripStart)))
                 .ForMember(f => f.Duration, op => op.MapFrom(mp => Decimal.ToInt32(mp.Duration)));

            CreateMap<TripRibasMetrics,MetricsNew>()
                .ForMember(f => f.TripStart, op => op.MapFrom(mp => Constants.GetFechaServidor(mp.TripStart)));

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

        private List<SiteDTO> MapearSites(GroupSummary lstSites, SiteResult sites)
        {
            var resultado = new List<SiteDTO>();

            if (lstSites != null)
            {
                GetSubGroup(resultado, lstSites, null);
            }

            return resultado;
        }
        private void GetSubGroup(List<SiteDTO> bases, GroupSummary Sitios, long? sitePadreId)
        {
            // condicion por la cual el metodo recursivo va a terminar
            if (Sitios.SubGroups.Count > 0)
            {
                bases.AddRange(Sitios.SubGroups.Select(s => new SiteDTO
                {
                    //Cargar los datos del modelo
                    SiteId = s.GroupId,
                    SiteName = s.Name,
                    SitePadreId = sitePadreId
                }));

                foreach (GroupSummary sum in Sitios.SubGroups)
                {
                    // si tiene subgrupos vuelve y ejecuta la acción
                    GetSubGroup(bases, sum, sum.GroupId);
                }

            }
            //bases sale lleno y sitios ya no importa 

        }

        private string MapearAditionalDetailsFields(Driver driver, DriverDTO result)
        {
            return "{" + driver.AdditionalDetailFields?.Select(s => $"\"{s.Label}\":\"{s.Value}\"").Aggregate((i, j) => i + "," + j) + "}";
        }

    }
}
