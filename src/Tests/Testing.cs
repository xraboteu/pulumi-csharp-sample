using System.Collections.Immutable;
using System.Threading.Tasks;
using Pulumi;
using Pulumi.Testing;

namespace Tests;

public static class Testing
{
    public static Task<ImmutableArray<Resource>> RunAsync<TStack>()
        where TStack : Stack, new()
    {
        var mocks = new BasicMocks();

        return Deployment.TestAsync<TStack>(
            mocks,
            new TestOptions
            {
                IsPreview = true
            });
    }
}