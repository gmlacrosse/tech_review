using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;

namespace StargateAPI.Business.Queries.Tests
{
    [TestClass]
    public class GetAstronautDutiesByNameTests
    {
        private DbContextOptions<StargateContext> _options;
        [TestInitialize]
        public void SetUp()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            _options = new DbContextOptionsBuilder<StargateContext>().UseSqlite(connection).Options;

            using (var context = new StargateContext(_options))
            {
                context.Database.EnsureCreated();
            }
        }

        [TestMethod]
        public async Task GetAstronautDutiesByName_NoError()
        {
            using (var context = new StargateContext(_options))
            {
                var persons = new List<Person> { new Person { Id = 1, Name = "John" }, new Person { Id = 2, Name = "Jane" } };
                var person1Detail = new AstronautDetail { Id = 1, PersonId = 1, CurrentRank = "R1", CurrentDutyTitle = "Commander", CareerStartDate = new DateTime(2023, 4, 5) };
                var astronautDuties = new List<AstronautDuty>
            {
                new AstronautDuty{ Id = 1, PersonId = 1, DutyTitle = "Pilot", Rank = "R2", DutyStartDate = new DateTime(2024, 1, 1), DutyEndDate = new  DateTime(2024, 12, 31)},
                new AstronautDuty{ Id = 2, PersonId = 1, DutyTitle = "Commander", Rank = "R1", DutyStartDate = new DateTime(2025, 2, 26) }
            };
                await context.People.AddRangeAsync(persons);
                await context.AstronautDetails.AddAsync(person1Detail);
                await context.AstronautDuties.AddRangeAsync(astronautDuties);
                await context.SaveChangesAsync();
            }

            
        }
    }
}
