using ENSEK.WebApi.Infrastucture.Repository.Entities;
using Loudwater.Result;
using Microsoft.AspNetCore.Http;

namespace ENSEK.WebApi.Infrastucture.Services
{
    public interface IBatchService
    {
        Task<Batch> CreateAsync(Batch batch);
        IQueryable<Batch> Get();
        Task<Batch?> GetAsync(int id);
        Task<IResult<Batch>> ProcessBatchAsync(IFormFile file);
    }
}