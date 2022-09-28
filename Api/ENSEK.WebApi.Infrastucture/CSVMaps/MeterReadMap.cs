using CsvHelper.Configuration;
using ENSEK.WebApi.Infrastucture.Repository.Entities;

namespace ENSEK.WebApi.Infrastucture.CSVMaps;

public class MeterReadMap : ClassMap<MeterRead>
{
    public MeterReadMap()
    { 
        Map(x => x.AccountId).Name("AccountId");
        Map(x => x.MeterReadingDateTime).Name("MeterReadingDateTime");
        Map(x => x.MeterReadValue).Name("MeterReadValue");
    }
}