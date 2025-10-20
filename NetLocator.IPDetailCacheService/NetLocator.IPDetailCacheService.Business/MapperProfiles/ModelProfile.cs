using AutoMapper;
using NetLocator.IPDetailCacheService.Business.Models;
using NetLocator.IPDetailCacheService.External.Dtos;

namespace NetLocator.IPDetailCacheService.Business.MapperProfiles;

public class ModelProfile : Profile
{
    public ModelProfile()
    {
        CreateMap<IpLookupDto, IpModel>();
    }
}