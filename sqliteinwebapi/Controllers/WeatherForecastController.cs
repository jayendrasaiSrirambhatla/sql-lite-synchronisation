using Dotmim.Sync;
using Dotmim.Sync.Sqlite;
using Dotmim.Sync.SqlServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace sqliteinwebapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
   
    public class WeatherForecastController : ControllerBase
    {
        public CustomerDbContext CustomerDbContext = new CustomerDbContext();
        public CustomerDbContext db = new CustomerDbContext();

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        // sql connection string
        public string serverConnectionString = $"data source=.\\sqlexpress;integrated security=true;database=sqlitedb";
        public string clientConnectionString;


        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            clientConnectionString = db.Database.GetDbConnection().ConnectionString;
            //Customer c = new Customer() { CustomerID = 3, Age = 120, CustomerName = "o" };
            //CustomerDbContext.Cus.Add(c);
            CustomerDbContext.SaveChanges();
            _logger = logger;

        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            List<Customer> c = CustomerDbContext.Customers.ToList();
            foreach(var a in c)
            {
                Console.WriteLine(a.CustomerName);
            }
            Console.WriteLine("saasa");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpPost]
        [Route("api/[controller]/update")]
        public async void syncAsync()
        {
            var serverProvider = new SqlSyncProvider(serverConnectionString);

            // Second provider is using plain old Sql Server provider, relying on triggers and tracking tables to create the sync environment
            var clientProvider = new SqliteSyncProvider(clientConnectionString);

            // Tables involved in the sync process:

            var setup = new SyncSetup("Customers");

            var agent = new SyncAgent(clientProvider, serverProvider);

            var progress = new SynchronousProgress<ProgressArgs>(s =>
                    Console.WriteLine($"{s.Context.SyncStage}:\t{s.Message}"));



            var syncContext = await agent.SynchronizeAsync(setup, progress);

        }

        [HttpPost]
        [Route("api/[controller]/Add")]
        public void Add( Customer c)
        {
            db.Customers.Add(c);
            db.SaveChanges();
        }
    }
}