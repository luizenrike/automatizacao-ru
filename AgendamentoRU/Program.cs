
using AgendamentoRU.Connection;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) 
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) 
            .Build();
var driver = new DriverConnection(configuration);

driver.AgendarRefeicao(2);


