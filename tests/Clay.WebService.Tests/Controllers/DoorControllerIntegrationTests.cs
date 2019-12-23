using Clay.WebService.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Clay.WebService.Tests.Controllers
{
    public class DoorControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;
        private readonly HttpClient httpClient;
        public DoorControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            this.httpClient = factory.CreateClient();
        }

        private HttpContent CreateContent<T>(T obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj),
               Encoding.UTF8,
               "application/json");
        }

        private async Task<string> CreateTokenForAdmin()
        {

            var result = await httpClient.PostAsync("token",
                             CreateContent(new LoginDTO()
                             {
                                 Email = "admin@admin.com",
                                 Password = "admin"
                             }));

            var token = await result.Content.ReadAsStringAsync();
            return $"Bearer {token.Replace("\"", string.Empty)}";
        }

        private async Task<string> CreateTokenForUser()
        {

            var result = await httpClient.PostAsync("token",
                         CreateContent(new LoginDTO()
                         {
                             Email = "test@test.com",
                             Password = "test"
                         }));


            var token = await result.Content.ReadAsStringAsync();
            return $"Bearer {token.Replace("\"", string.Empty)}";
        }

        private async Task<Guid> GetDoorId()
        {
            var response = await httpClient.GetAsync($"doors");
            return JsonConvert.DeserializeObject<List<DoorDTO>>(await response.Content.ReadAsStringAsync()).First().Id;
        }

        [Fact]
        public async Task Create_NonGrantedAccess_ReturnsForbidden()
        {
            var token = await CreateTokenForUser();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await httpClient.PostAsync("doors",
                          CreateContent(new DoorDTO()
                          {
                              Name = "New Door",
                              Description = "I am new"
                          }));

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_NotAuthorizedUser_ReturnsUnauthorized()
        {
            var response = await httpClient.PostAsync("doors",
                          CreateContent(new DoorDTO()
                          {
                              Name = "New Door",
                              Description = "I am new"
                          }));

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Create_InvalidPayload_ReturnsBadRequest()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await httpClient.PostAsync("doors",
                          CreateContent(new DoorDTO()
                          {
                          }));

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_Admin_CreatesANewDoor()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await httpClient.PostAsync("doors",
                          CreateContent(new DoorDTO()
                          {
                              Name = "New Door",
                              Description = "I am new"
                          }));

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Fact]
        public async Task GetById_NonExistingDoor_ReturnsNotFound()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await httpClient.GetAsync($"doors/{Guid.NewGuid()}");

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_EmptyId_ReturnsBadRequest()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await httpClient.GetAsync($"doors/{Guid.Empty}");

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetById_Success_ReturnsOk()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var doorId = await GetDoorId();
            var response = await httpClient.GetAsync($"doors/{doorId}");

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAll_NonExistingDoor_ReturnsNotFound()
        {
            var token = await CreateTokenForUser();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await httpClient.GetAsync($"doors");

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var responseContent = JsonConvert.DeserializeObject<List<DoorDTO>>(await response.Content.ReadAsStringAsync());
            responseContent.Should().NotBeNull();
            responseContent.Should().HaveCountGreaterThan(1);
        }

        [Fact]
        public async Task Update_NonGrantedAccess_ReturnsForbidden()
        {
            var token = await CreateTokenForUser();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var doorId = await GetDoorId();

            var response = await httpClient.PutAsync($"doors/{doorId}",
                CreateContent<DoorDTO>(
                    new DoorDTO()
                    {
                        Name = "New Door",
                        Description = "I am updated"
                    }));

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_EmptyId_ReturnsForbidden()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var response = await httpClient.PutAsync($"doors/{Guid.Empty}",
                CreateContent<DoorDTO>(
                    new DoorDTO()
                    {
                        Name = "New Door",
                        Description = "I am updated"
                    }));

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var doorId = await GetDoorId();

            var response = await httpClient.PutAsync($"doors/{doorId}",
                CreateContent<DoorDTO>(
                    new DoorDTO()
                    {
                        Name = "New Door",
                        Description = "I am updated"
                    }));

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task UpdateState_EmptyId_ReturnsBadRequest()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var response = await httpClient.PutAsync($"doors/{Guid.Empty}/state/1", null);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateState_InvalidState_ReturnsBadRequest()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var doorId = await GetDoorId();

            var response = await httpClient.PutAsync($"doors/{doorId}/state/100", null);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateState_SameState_ReturnsBadRequest()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var doorId = await GetDoorId();

            var response = await httpClient.PutAsync($"doors/{doorId}/state/0", null);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task AuditList_NonGrantedAccess_ReturnsForbidden()
        {
            var token = await CreateTokenForUser();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var doorId = await GetDoorId();

            var response = await httpClient.GetAsync($"doors/{doorId}/audits");

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task AuditList_EmptyId_ReturnsForbidden()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var response = await httpClient.GetAsync($"doors/{Guid.Empty}/audits");

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AuditList_NonExistingLog_ReturnsForbidden()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var response = await httpClient.GetAsync($"doors/{Guid.NewGuid()}/audits");

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task AuditList_Success_ReturnsLogs()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);

            var doorId = await GetDoorId();

            await httpClient.PutAsync($"doors/{doorId}/state/1", null);

            var response = await httpClient.GetAsync($"doors/{doorId}/audits");

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

    }
}