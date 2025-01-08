using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;

namespace StargateAPI.Business.Commands.Tests
{
    [TestClass()]
    public class UpdatePersonTests
    {
        private DbContextOptions<StargateContext> _options;

        [TestInitialize]
        public void SetUp()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            _options = new DbContextOptionsBuilder<StargateContext>().UseSqlite(connection).Options;

            using var context = new StargateContext(_options);
            context.Database.EnsureCreated();
        }

        [TestMethod]
        public async Task UpdatePersonHandlerTest()
        {
            using (var context = new StargateContext(_options))
            {
                context.People.Add(new Person { Id = 1, Name = "Unit TesterA" });
                await context.SaveChangesAsync();
            }

            using (var context = new StargateContext(_options))
            {
                var handler = new UpdatePersonHandler(context);

                var result = await handler.Handle(new UpdatePerson() { CurrentName = "Unit TesterA", NewName = "Unit TesterB" }, default);

                var expectedResult = new UpdatePersonResult
                {
                    Id = 1,
                    Name = "Unit TesterB"
                };
                result.Should().BeEquivalentTo(expectedResult);
            }
        }

        [TestMethod]
        public async Task CreatePersonPreProcessorTest_CurrentNameDoesNotExist()
        {
            using (var context = new StargateContext(_options))
            {
                context.People.Add(new Person { Id = 1, Name = "Unit TesterA" });
                context.People.Add(new Person { Id = 2, Name = "Unit TesterB" });
                await context.SaveChangesAsync();
            }

            using (var context = new StargateContext(_options))
            {
                var preProcessor = new UpdatePersonPreProcessor(context);

                var param = new UpdatePerson() { CurrentName = "Unit TesterC", NewName = "Unit TesterZ" };
                var act = async () => await preProcessor.Process(param, default);

                await act.Should().ThrowAsync<BadHttpRequestException>();
            }
        }
    }
}