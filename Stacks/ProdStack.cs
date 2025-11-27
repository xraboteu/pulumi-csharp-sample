using Pulumi;
using Pulumi.AzureNative.Resources;
using Infra.Modules.Storage;
using Infra.Modules.FunctionApp;
using Infra.Modules.Monitoring;
using Infra.Utils;

namespace Infra.Stacks;

public class ProdStack : Stack
{
    public ProdStack()
    {
        var cfg = new Config();
        var env = cfg.Get("environment") ?? "prod";
        var location = cfg.Get("location") ?? "westeurope";
        var baseName = cfg.Get("baseName") ?? "xra-prod";

        var rg = new ResourceGroup(NameFactory.ResourceGroupName(baseName, env), new ResourceGroupArgs
        {
            Location = location,
            Tags =
            {
                { "env", env },
                { "owner", "xavier" },
                { "criticality", "high" }
            }
        });
        var tags = TagBuilder.Default(env);

        var storage = new StorageModule($"{baseName}-storage", new StorageModuleArgs
        {
            ResourceGroupName = rg.Name,
            Location = rg.Location,
            Tags = tags
        });

        var monitoring = new MonitoringModule($"{baseName}-mon", new MonitoringModuleArgs
        {
            ResourceGroupName = rg.Name,
            Location = rg.Location,
            Tags = tags
        });

        var func = new FunctionModule($"{baseName}-fa", new FunctionModuleArgs
        {
            ResourceGroupName = rg.Name,
            Location = rg.Location,
            Tags = tags,
            StorageAccountName = storage.AccountName,
            AppInsightsInstrumentationKey = monitoring.InstrumentationKey
        });

        Environment = env;
        ResourceGroupName = rg.Name;
        FunctionEndpoint = func.DefaultHostname;
    }

    [Output] public string Environment { get; set; }
    [Output] public Output<string> ResourceGroupName { get; set; }
    [Output] public Output<string> FunctionEndpoint { get; set; }
}
