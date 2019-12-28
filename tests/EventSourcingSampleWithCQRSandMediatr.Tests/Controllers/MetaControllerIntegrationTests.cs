using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace EventSourcingSampleWithCQRSandMediatr.Tests.Controllers
{
    public class MetaControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly WebApplicationFactory<Startup> factory;
        public MetaControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task Liveness()
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync("/liveness");

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
        [Fact]
        public async Task ApiCalls_ShouldHave_CorrelationId()
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync("/liveness");
            response.Headers.Any(x => x.Key == "X-Correlation-ID").Should().BeTrue();
        }

        [Fact]
        public async Task MetricCalls_Success_ReturnsOk()
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync("/metrics");
            response.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}
