using AutoMapper;
using ENSEK.WebApi.DTOs;
using ENSEK.WebApi.Infrastucture.Repository.Entities;

namespace ENSEK.WebApi.AutoMapperProfiles;

public class BatchProfiles : Profile
{
    public BatchProfiles()
    {
        CreateMap<Batch, BatchDto>();
    }
}