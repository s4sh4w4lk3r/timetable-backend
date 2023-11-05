using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebApi.Types.Configuration;

namespace WebApi.Tests.Integration;

public class AuthFixture
{
    public string ApiKey { get; init; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public WebApplicationFactory<Program> ApplicationFactory { get; init; }

    public AuthFixture()
    {
        ApplicationFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        ApiKey = ApplicationFactory.Services.GetRequiredService<IOptions<ApiSettings>>().Value.ApiKey;


    }

}

[CollectionDefinition("AuthCollection")]
public class AuthCollection : ICollectionFixture<AuthFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
#warning написать юнит тест для проверки проверки таймтейблов на дубли