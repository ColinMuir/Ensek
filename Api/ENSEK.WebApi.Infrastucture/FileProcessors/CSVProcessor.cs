using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Reflection;

namespace ENSEK.WebApi.Infrastucture.FileProcessors;

public class CSVProcessor : IFileProcessor
{ 
    public IProcessFileResult<TModel> ProcessFile<TModel>(IFormFile file)
    {
        var result = new ProcessFileResult<TModel>();

        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var csvClassMap = Assembly.GetAssembly(typeof(TModel))!
                                  .GetTypes()
                                  .FirstOrDefault(t => t.IsClass && t.IsSubclassOf(typeof(ClassMap<TModel>)));

        if (csvClassMap != null)
            csv.Context.RegisterClassMap((ClassMap<TModel>?)Activator.CreateInstance(csvClassMap));

        csv.Read();
        csv.ReadHeader();
        while (csv.Read())
        {
            try
            {
                result.TotalRecords++;

                result.Records.Add(csv.GetRecord<TModel>());
            }
            catch (Exception)
            {
                result.InvalidRecords++;
            }
        }

        return result;
    }
}