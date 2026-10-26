
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Controllers;
using RaceDay.API.Data;
using RaceDay.API.Models;
using Xunit;

namespace RaceDay.API.Tests
{
    public class RaceDayUnitTests
    {
        private static RaceDayDbContext CreateDatabase()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new RaceDayDbContext(options);
        }

        private static EventsController CreateController(
            RaceDayDbContext db)
        {
            var controller = new EventsController(db);

            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            return controller;
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            using var db = CreateDatabase();
            var controller = CreateController(db);

            var result = await controller.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_WhenEventDoesNotExist_ReturnsNotFound()
        {
            using var db = CreateDatabase();
            var controller = CreateController(db);

            var result = await controller.GetById(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetById_WhenEventExists_ReturnsOk()
        {
            using var db = CreateDatabase();

            db.Events.Add(new Event
            {
                EventID = 1,
                OrganiserID = 1,
                EventTypeID = 1,
                Name = "Soweto Marathon",
                Description = "Running event",
                EventDate = DateTime.UtcNow.AddDays(30),
                Location = "Soweto",
                Distance = 42.2m,
                CreatedAt = DateTime.UtcNow
            });

            await db.SaveChangesAsync();

            var controller = CreateController(db);
            var result = await controller.GetById(1);

            Assert.IsType<OkObjectResult>(result);
        }
    }

    public class TestSession : ISession
    {
        private readonly System.Collections.Generic.Dictionary<string, byte[]>
            _store = new();

        public IEnumerable<string> Keys => _store.Keys;

        public string Id => "test-session";

        public bool IsAvailable => true;

        public void Clear() => _store.Clear();

        public Task CommitAsync(
            CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task LoadAsync(
            CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public void Remove(string key) => _store.Remove(key);

        public void Set(string key, byte[] value)
            => _store[key] = value;

        public bool TryGetValue(string key, out byte[] value)
            => _store.TryGetValue(key, out value!);
    }
}
