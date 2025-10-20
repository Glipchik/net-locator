using AutoMapper;
using NetLocator.IPLookupService.API.Dtos;
using NetLocator.IPLookupService.Business.Models;

namespace NetLocator.IPLookupService.API.MapperProfiles;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateMap<IpModel, IpDto>();
    }
}