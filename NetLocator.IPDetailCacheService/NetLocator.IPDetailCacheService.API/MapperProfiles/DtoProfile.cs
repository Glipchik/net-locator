using AutoMapper;
using NetLocator.IPDetailCacheService.API.Dtos;
using NetLocator.IPDetailCacheService.Business.Models;

namespace NetLocator.IPDetailCacheService.API.MapperProfiles;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateMap<IpModel, IpDto>();
    }
}