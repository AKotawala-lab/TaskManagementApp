using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using TaskManagementApp.Application.Models;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.UnitTests.Controllers
{
    public class AuthenticationControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthenticationControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_ShouldReturnToken_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var request = new RegisterUserRequest
            {
                Username = "newuser",
                Password = "password123",
                Email = "newuser@test.com",
                AvatarUrl = ""
            };

            // Act
            var response = await _client.PostAsync("/api/authentication/register", new StringContent(
                JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // Deserialize the response to verify the structure
            var jsonResponse = JsonSerializer.Deserialize<AuthResponse>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            jsonResponse.AccessToken.Should().NotBeNullOrEmpty();
            jsonResponse.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
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

            // Deserialize the response to verify the structure
            var jsonResponse = JsonSerializer.Deserialize<AuthResponse>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            jsonResponse.AccessToken.Should().NotBeNullOrEmpty();
            jsonResponse.RefreshToken.Should().NotBeNullOrEmpty();
        }
    }
}
