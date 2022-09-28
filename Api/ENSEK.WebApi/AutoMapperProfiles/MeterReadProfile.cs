using AutoMapper;
using ENSEK.WebApi.DTOs;
using ENSEK.WebApi.Infrastucture.Repository.Entities;

namespace ENSEK.WebApi.AutoMapperProfiles;

public class MeterReadProfile : Profile
{
    public MeterReadProfile()
    {
        CreateMap<MeterRead, MeterReadDto>();
    }
}