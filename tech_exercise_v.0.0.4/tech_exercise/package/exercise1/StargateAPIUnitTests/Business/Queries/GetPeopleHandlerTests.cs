using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;

namespace StargateAPI.Business.Queries.Tests
{
    [TestClass()]
    public class GetPeopleHandlerTests
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
        public async Task GetPeople_NoError()
        {
            using (var context = new StargateContext(_options))
            {
                var persons = new List<Person> { new Person { Id = 1, Name = "Tom" }, new Person { Id = 2, Name = "John" } };
                var person1Detail = new AstronautDetail { Id = 1, PersonId = 1, CurrentRank = "R1", CurrentDutyTitle = "Commander", CareerStartDate = new DateTime(2025, 1, 7) };

                context.People.AddRange(persons);
                context.AstronautDetails.Add(person1Detail);
                context.SaveChanges();
            }

            using (var context = new StargateContext(_options))
            {
                var handler = new GetPeopleHandler(context);

                var result = await handler.Handle(new GetPeople(), default);

                var expectedResult = new GetPeopleResult
                {
                    People = new List<PersonAstronaut>
                {
                    new PersonAstronaut { PersonId = 1, Name = "Tom", CurrentRank = "R1", CurrentDutyTitle = "Commander", CareerStartDate = new DateTime(2025, 1, 7) },
                    new PersonAstronaut { PersonId = 2, Name = "John",  }
                }
                };
                result.Should().BeEquivalentTo(expectedResult);
            }
        }
    }
}