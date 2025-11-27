using System.Collections.Generic;
using Infra.Stacks;

using Pulumi;
return await Deployment.RunAsync(() =>
{
    var config = new Config();
    var environment = config.Get("environment") ?? "dev";

    Stack stack = environment.ToLowerInvariant() switch
    {
        "dev" => new DevStack(),
        "staging" => new StagingStack(),
        "prod" => new ProdStack(),
        _ => new DevStack()
    };
}).ConfigureAwait(false);