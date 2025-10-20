using AutoMapper;
using NetLocator.IPLookupService.Business.Models;
using NetLocator.IPLookupService.IpStack.Dtos;

namespace NetLocator.IPLookupService.Business.MapperProfiles;

public class ModelProfile : Profile
{
    public ModelProfile()
    {
        CreateMap<IpStackAddressDto, IpModel>();
    }
}