using ENSEK.WebApi.Infrastucture.FileProcessors;
using ENSEK.WebApi.Infrastucture.Repository;
using ENSEK.WebApi.Infrastucture.Repository.Entities;
using Loudwater.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ENSEK.WebApi.Infrastucture.Services;

public class BatchService : IBatchService
{
    private readonly MeterReadingDbContext context;
    private readonly IFileProcessor fileProcessor;
    private readonly IMeterReadingService meterReadingService;

    public BatchService(MeterReadingDbContext context, IFileProcessor fileProcessor, IMeterReadingService meterReadingService)
    {
        this.context = context;
        this.fileProcessor = fileProcessor;
        this.meterReadingService = meterReadingService;
    }

    public async Task<IResult<Batch>> ProcessBatchAsync(IFormFile file)
    {
        if (file == null)
            throw new ArgumentNullException(nameof(file));

        try
        {
            var processFileResult = fileProcessor.ProcessFile<MeterRead>(file);

            //the file processor returns the total number of records.
            var newBatch = new Batch { TotalRecords = processFileResult.TotalRecords };

            var processReadsResult = await meterReadingService.ProcesReadsAsync(processFileResult.Records);

            newBatch.Successful = processReadsResult.Successful;

            await CreateAsync(newBatch);

            return Result.Success(newBatch);
        }
        catch (Exception ex)
        {
            return Result.Fail<Batch>(ex.Message);
        }
    }

    public IQueryable<Batch> Get() =>
        context.Batches;

    public async Task<Batch?> GetAsync(int id) =>
        await context.Batches.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<Batch> CreateAsync(Batch batch)
    {
        await context.Batches.AddAsync(batch);

        await context.SaveChangesAsync();

        return batch;
    }
}