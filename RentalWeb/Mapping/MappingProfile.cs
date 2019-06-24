using AutoMapper;
using RentalWeb.Models;
using SharedComponents.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalWeb.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EquipmentItemDto, EquipmentItemViewModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<EquipmentItemViewModel, OrderItemDto>();
        }
    }
}
