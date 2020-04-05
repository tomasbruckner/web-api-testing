using System.Net.Http;
using Example.Api.Tests.Support.Utils;
using Example.Models.Models;
using JestDotnet;
using Xunit;

namespace Example.Api.Tests.Controllers
{
    [TestCaseOrderer("Example.Api.Tests.Support.Utils.PriorityOrderer", "Example.Api.Tests")]
    public class UserControllerTests : IClassFixture<CustomWebApplicationFactoryWithInMemoryDb<Startup>>
    {
        public UserControllerTests(CustomWebApplicationFactoryWithInMemoryDb<Startup> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
        }

        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactoryWithInMemoryDb<Startup> _factory;
        private const string RootRoute = "api/user";

        [Fact]
        [TestOrder(1)]
        public async void T01Create()
        {
            _factory.SeedData(
                db =>
                {
                    // prepare any data      
                }
            );
            var request = TestUtils.CreatePostMessageAsAdmin(
                $"{RootRoute}",
                new UserCreateDto
                {
                    Age = 12,
                    FirstName = "John",
                    LastName = "Smith"
                }
            );

            var entity = await TestUtils.SendOkRequest(_client, request);
            entity.ShouldMatchSnapshot();
        }

        [Fact]
        [TestOrder(2)]
        public async void T02Get()
        {
            await TestUtils.IterateAllRolesAsync(
                async (addHeader, roleName) =>
                {
                    var request = TestUtils.CreateGetRequest($"{RootRoute}/1");
                    addHeader(request);
                    var entity = await TestUtils.SendOkRequest(_client, request);
                    entity.ShouldMatchSnapshot(roleName);
                }
            );
        }

        [Fact]
        [TestOrder(3)]
        public async void T03Edit()
        {
            var request = TestUtils.CreatePutRequestAsAdmin(
                $"{RootRoute}/1",
                new UserEditDto
                {
                    Age = 22,
                    FirstName = "Michael",
                    LastName = "Fritz"
                }
            );

            var entity = await TestUtils.SendOkRequest(_client, request);
            entity.ShouldMatchSnapshot();
        }

        [Fact]
        [TestOrder(4)]
        public async void T04Delete()
        {
            var request = TestUtils.CreateDeleteRequestAsAdmin($"{RootRoute}/1");
            await TestUtils.SendNoContentRequest(_client, request);
        }

        [Fact]
        [TestOrder(5)]
        public async void T05GetNotFound()
        {
            var request = TestUtils.CreateGetRequestAsAdmin($"{RootRoute}/1");
            await TestUtils.SendNotFoundRequest(_client, request);
        }
    }
}
