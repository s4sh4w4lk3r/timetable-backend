using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Tests
{
    public class UnitTest1
    {
        private HttpClient _httpClient;
        public UnitTest1()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>();
            _httpClient = webApplicationFactory.CreateDefaultClient();
        }

        [Fact]
        public async void Test1()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>();
            _httpClient = webApplicationFactory.CreateDefaultClient();
            var response = await _httpClient.GetAsync("/api/customer");
            

            Assert.True(response.IsSuccessStatusCode, new StreamReader(response.Content.ReadAsStream()).ReadToEnd());
        }
    }
}