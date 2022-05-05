
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

public class RepositoryTests
{
    public static IConfigurationRoot RepositoryTestsConstructor()
    {
        //Using the configuration builder to get the connection string from the appsettings.json
        var myConfiguration = new Dictionary<string, string>
        {

            {"MongoDbConnectionString:ConnectionString",  "mongodb+srv://<username>:<password>@cluster0.rljqx.mongodb.net/<database>?retryWrites=true&w=majority"},
            {"MongoDbConnectionString:MongoDbUserName",   "flights"},
            {"MongoDbConnectionString:MongoDbPassword",   "flights"},
            {"MongoDbConnectionString:MongoDbDatabase",   "FlightsTests"},
            {"MongoDbConnectionString:MongoDbFlightsCollection", "FlightsTests"},
            {"MongoDbConnectionString:MongoDbAirlinesCollection", "AirlinesTests"},
            {"AzureKeyVault:Name", "https://HolidayPlannerKeyVault.vault.azure.net"}
        };

        

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();
        return configuration;
    }

    

}