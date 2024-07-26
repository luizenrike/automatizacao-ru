
using AgendamentoRU.Connection;
using Microsoft.Extensions.Configuration;





IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Define o diretório base para o arquivo de configuração
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Carrega o arquivo appsettings.json
            .Build();
var driver = new DriverConnection(configuration);

driver.AgendarRefeicao(2);


