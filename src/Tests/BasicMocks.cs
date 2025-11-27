using System.Threading.Tasks;
using Pulumi.Testing;

namespace Tests;

public class BasicMocks : IMocks
{
    // Simule la création d'une ressource
    public Task<(string id, object state)> NewResourceAsync(MockResourceArgs args)
    {
        // id factice
        var id = $"{args.Name}_id";

        // On renvoie les inputs comme état par défaut
        return Task.FromResult((id, (object)args.Inputs));
    }

    // Simule un appel data source / fonction
    public Task<object> CallAsync(MockCallArgs args)
    {
        // On renvoie juste les args tels quels
        return Task.FromResult((object)args.Args);
    }
}