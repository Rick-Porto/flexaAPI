using AutoMapper;
using flexa.App.DTOs.Screen;
using flexa.App.DTOs.Component;
using flexa.Domain.Entities;

namespace flexa.App.Mapping;

public class ScreenProfile : Profile
{
    public ScreenProfile()
    {
        CreateMap<ScreenRequest, Screen>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()); // ignore Id for new entities

        CreateMap<Screen, ScreenResponse>();

        CreateMap<ComponentRequest, Component>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ScreenId, opt => opt.Ignore());

        CreateMap<Component, ComponentResponse>();
    }
}