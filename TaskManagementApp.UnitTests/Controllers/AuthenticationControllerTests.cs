using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using TaskManagementApp.Application.Models;

namespace TaskManagementApp.UnitTests.Controllers
{
    public class AuthenticationControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthenticationControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact, Trait("Category", "Exclude")]
        public async Task Register_ShouldReturnToken_WhenRegistrationIsSuccessful()
        {
            var request = new RegisterUserRequest
            {
                Username = "newuser",
                Password = "password123",
                Email = "newuser@test.com",
                AvatarUrl = ""
            };

            var response = await _client.PostAsync("/api/authentication/register", new StringContent(
                JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("token");
        }

        [Fact, Trait("Category", "Exclude")]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Register user first
            var registerRequest = new RegisterUserRequest
            {
                Username = "existinguser",
                Password = "password123",
                Email = "existinguser@test.com",
                AvatarUrl = ""
            };

            await _client.PostAsync("/api/authentication/register", new StringContent(
                JsonSerializer.Serialize(registerRequest), Encoding.UTF8, "application/json"));

            // Login with the same credentials
            var loginRequest = new LoginUserRequest
            {
                Username = "existinguser",
                Password = "password123"
            };

            var response = await _client.PostAsync("/api/authentication/login", new StringContent(
                JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("token");
        }
    }
}
