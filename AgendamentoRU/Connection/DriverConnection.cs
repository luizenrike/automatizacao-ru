using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace AgendamentoRU.Connection
{
    public class DriverConnection
    {
        private readonly IConfiguration _configuration;

        public DriverConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AgendarRefeicao(int refeicao)
        {

            ChromeOptions optionsWeb = new ChromeOptions();
            //optionsWeb.AddArgument("--headless");
            optionsWeb.AddArgument("--disable-gpu");
            var driver = new ChromeDriver(optionsWeb);

            try
            {
                // visited URL UFC:
                driver.Navigate().GoToUrl(_configuration["Urls:loginPage"]);

                var login = driver.FindElement(By.Name("user.login"));
                var password = driver.FindElement(By.Name("user.senha"));

                login.SendKeys(_configuration["Credentials:login"]);
                password.SendKeys(_configuration["Credentials:password"]);

                var loginButton = driver.FindElement(By.Name("entrar"));
                loginButton.Click();

                Thread.Sleep(TimeSpan.FromSeconds(2));


                var linkDiscente = driver.FindElement(By.LinkText("Portal do Discente"));
                linkDiscente.Click();

                Thread.Sleep(TimeSpan.FromSeconds(2));


                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement restaurantElement = wait.Until(driver => driver
                    .FindElement(By.XPath("//span[@class='ThemeOfficeMainFolderText' and text()='Restaurante Universitário']")));


                Actions action = new Actions(driver);
                action.MoveToElement(restaurantElement).Perform();

                IWebElement agendarRefeicao = driver.FindElement(By.CssSelector("#cmAction-97"));
                agendarRefeicao.Click();
                Thread.Sleep(TimeSpan.FromSeconds(2)); // wait loading page


                Agendamento(driver, 2);
                Agendamento(driver, 3);

                Console.ReadLine();


                //IWebElement listaErros = driver.FindElement(By.CssSelector("ul.erros"));
                //var erros = listaErros.FindElements(By.TagName("li"));
                //bool erroEncontrado = erros.Any(erro => erro.Text.Contains("Existe agendamento para o dia e tipo de refeição selecionados."));

                //if (erroEncontrado)
                //{
                //    Console.ForegroundColor = ConsoleColor.Red;
                //    Console.WriteLine("Erro: Existe agendamento para o dia e tipo de refeição selecionados, tente novamente amanhã");
                //    Console.ResetColor();
                //    Console.ReadLine();
                    
                //}

            }
            catch(HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine("Uma exceção ocorreu: " + e.Message);
            }
            finally
            {
                driver.Quit();
            }
        }

        private void Agendamento(ChromeDriver driver, int refeicao)
        {
            try
            {
                var dateAgendamento = driver.FindElement(By.Id("formulario:data_agendamento"));

                var date = DateTime.Now;
                var proxDate = date.AddDays(2);
                dateAgendamento.SendKeys(proxDate.Date.ToString());

                var tipoRefeicao = driver.FindElement(By.Id("formulario:tipo_refeicao"));
                var selectedTipoRefeicao = new SelectElement(tipoRefeicao);

                if (refeicao == 2)
                {

                    selectedTipoRefeicao.SelectByValue("2");
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    var horario = driver.FindElement(By.Id("formulario:horario_agendado"));
                    var selectedHorario = new SelectElement(horario);
                    selectedHorario.SelectByValue("53");
                }
                else if (refeicao == 3)
                {
                    selectedTipoRefeicao.SelectByValue("3");
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    var horario = driver.FindElement(By.Id("formulario:horario_agendado"));
                    var selectedHorario = new SelectElement(horario);
                    selectedHorario.SelectByValue("54");
                }

                var buttonCadastrarAgendamento = driver.FindElement(By.Id("formulario:cadastrar_agendamento_bt"));
                buttonCadastrarAgendamento.Click();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
