using ENSEK.WebApi.Infrastucture.Repository.Entities;

namespace ENSEK.WebApi.Infrastucture.Services;

public interface IMeterReadingService
{
    Task<MeterRead?> GetReadByAccountIdAsync(int accountId);

    Task<ProcessReadsResult> ProcesReadsAsync(IEnumerable<MeterRead> reads);

    IQueryable<MeterRead> Get();
}