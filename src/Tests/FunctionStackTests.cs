using System.Linq;
using System.Threading.Tasks;
using Pulumi;
using Pulumi.Testing;
using Infra.Stacks;
using Xunit;

namespace Tests;

public class FunctionStackTests
{
    /// <summary>
    /// Devs the stack should expose function endpoint.
    /// Exemple : vérifier qu'on a bien au moins une WebApp
    /// </summary>
    [Fact]
    public async Task DevStack_Should_Expose_FunctionEndpoint()
    {
        var resources = await Testing.RunAsync<DevStack>();

        var hasWebApp = resources.Any(r => r is Pulumi.AzureNative.Web.WebApp);
        Assert.True(hasWebApp);
    }
}