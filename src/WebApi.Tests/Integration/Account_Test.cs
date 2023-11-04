using Models.Entities.Users;
using System.Net.Http.Json;

namespace WebApi.Tests.Integration
{
    [Collection("AuthCollection")]
    public class Account_Test
    {
      /*  private readonly AuthFixture _authFixture;
        public Account_Test(AuthFixture authFixture)
        {
            _authFixture = authFixture;
        }


        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("", " ")]
        [InlineData(" ", "")]
        public async Task Register_EmptyValues_Returns400(string email, string password)
        {
            const string URL = "api/account/register";

            using var client = _authFixture.ApplicationFactory.CreateClient();

            var user = new User()
            {
                Email = email,
                Password = password
            };
            client.DefaultRequestHeaders.Add("Api-Key", apiKey);

            var content = JsonContent.Create(user);

            var response = await client.PostAsync(URL, content);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest, new StreamReader(response.Content.ReadAsStream()).ReadToEnd());
        }*/
    }
}
