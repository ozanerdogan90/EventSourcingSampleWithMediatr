using Clay.WebService.DataAccess.Entities;
using Clay.WebService.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Clay.WebService.Tests.Controllers
{
    public class UserControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;
        private readonly HttpClient httpClient;
        public UserControllerIntegrationTests(WebApplicationFactory<Startup> factory)
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

        private async Task<Guid> GetUserId()
        {
            var response = await httpClient.GetAsync("users/email/test@test.com");
            return JsonConvert.DeserializeObject<UserDTO>(await response.Content.ReadAsStringAsync()).Id;

        }

        [Fact]
        public async Task Get_NonGrantedAccess_ReturnsForbidden()
        {
            var token = await CreateTokenForUser();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await httpClient.GetAsync("users/email/IamEmail");

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }


        [Fact]
        public async Task Get_NonExistingUser_ReturnsNotFound()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await httpClient.GetAsync("users/email/IamEmail");

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_Success_ReturnsOk()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await httpClient.GetAsync("users/email/test@test.com");

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }


        [Fact]
        public async Task Create_NonGrantedAccess_ReturnsForbidden()
        {
            var token = await CreateTokenForUser();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await httpClient.PostAsync("users",
                          CreateContent(new UserDTO()
                          {
                              Email = "user@user.com",
                              Password = "pass",
                              Role = Role.User
                          }));

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_UnAuthorizedUser_ReturnsUnauthorized()
        {
            var response = await httpClient.PostAsync("users",
                          CreateContent(new UserDTO()
                          {
                              Email = "user@user.com",
                              Password = "pass",
                              Role = Role.User
                          }));

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Create_InvalidPayload_ReturnsBadRequest()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await httpClient.PostAsync("users",
                          CreateContent(new UserDTO()
                          {
                              Name = "Clay"
                          }));

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task Create_Success_ReturnsCreated()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await httpClient.PostAsync("users",
                        CreateContent(new UserDTO()
                        {
                            Email = "user@user.com",
                            Password = "pass",
                            Role = Role.User
                        }));

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Fact]
        public async Task Update_NonGrantedAccess_ReturnsForbidden()
        {
            var token = await CreateTokenForUser();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var response = await httpClient.PutAsync($"users/{Guid.NewGuid()}",
                          CreateContent(new UserDTO()
                          {
                              Email = "user@user.com",
                              Password = "pass",
                              Role = Role.User
                          }));

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_UnAuthorizedUser_ReturnsUnauthorized()
        {
            var response = await httpClient.PutAsync($"users/{Guid.NewGuid()}",
                          CreateContent(new UserDTO()
                          {
                              Email = "user@user.com",
                              Password = "pass",
                              Role = Role.User
                          }));

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Update_InvalidPayload_ReturnsBadRequest()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var userId = await GetUserId();
            var response = await httpClient.PutAsync($"users/{userId}",
                          CreateContent(new UserDTO()
                          {
                              Name = "Clay"
                          }));

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_InvalidId_ReturnsBadRequest()
        {
            var token = await CreateTokenForAdmin();
            this.httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var userId = await GetUserId();
            var response = await httpClient.PutAsync($"users/{userId}",
                          CreateContent(new UserDTO()
                          {
                              Name = "Clay"
                          }));

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

    }
}