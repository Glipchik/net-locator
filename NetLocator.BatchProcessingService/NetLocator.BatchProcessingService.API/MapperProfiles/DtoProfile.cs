using AutoMapper;
using NetLocator.BatchProcessingService.API.Dtos;
using NetLocator.BatchProcessingService.Business.Models;

namespace NetLocator.BatchProcessingService.API.MapperProfiles;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateMap<BatchModel, BatchResponseDto>()
            .ForMember(dest => dest.BatchId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TotalIpAddresses, opt => opt.MapFrom(src => src.TotalIpAddresses))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<BatchModel, BatchStatusDto>()
            .ForMember(dest => dest.BatchId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.TotalIpAddresses, opt => opt.MapFrom(src => src.TotalIpAddresses))
            .ForMember(dest => dest.ProcessedIpAddresses, opt => opt.MapFrom(src => src.ProcessedIpAddresses))
            .ForMember(dest => dest.SuccessfulIpAddresses, opt => opt.MapFrom(src => src.SuccessfulIpAddresses))
            .ForMember(dest => dest.FailedIpAddresses, opt => opt.MapFrom(src => src.FailedIpAddresses))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.CompletedAt))
            .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors));
    }
}
