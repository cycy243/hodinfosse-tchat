using Bogus;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Domain;
using Tchat.API.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace Tchat.Api.Data.Repository.Database.Test.Utils
{
    public static class DataCreator
    {
        public static Mock<ApplicationDbContext> GetMockDbContext() => new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());


        public static ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestVacationBd")
                .Options;

            var context = new ApplicationDbContext(options);

            context.Database.EnsureCreated();

            return context;
        }

        private static void Seed(ApplicationDbContext context)
        {
            context.SaveChanges();
        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }


        public static Mock<DbSet<Message>> GetMockedMessageDbSet(int itemCount, string ipTchat)
        {
            return new Faker<Message>()
                .RuleFor(m => m.Id, f => Guid.NewGuid())
                .RuleFor(m => m.Content, f => f.Lorem.Sentence())
                .RuleFor(m => m.SendDateTime, f => f.Date.Past())
                .RuleFor(m => m.TchatIp, f => ipTchat)
                .Generate(itemCount)
                .ToList()
                .CreateMockDbSet();
        }

        //public static Mock<DbSet<User>> GetMockedUserManager(int itemCount)
        //{
        //    return new Faker<Message>()
        //        .RuleFor(m => m.Id, f => Guid.NewGuid())
        //        .Generate(itemCount)
        //        .ToList()
        //        .CreateMockDbSet();
        //}
    }
}
