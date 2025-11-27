using System.Collections.Generic;
using Pulumi;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

namespace Infra.Modules.FunctionApp;

public class FunctionModuleArgs
{
    public Input<string> ResourceGroupName { get; set; } = null!;
    public Input<string> Location { get; set; } = null!;
    public InputMap<string>? Tags { get; set; }
    public Input<string> StorageAccountName { get; set; } = null!;
    public Input<string> AppInsightsInstrumentationKey { get; set; } = null!;
}

public class FunctionModule : ComponentResource
{
    public Output<string> DefaultHostname { get; }

    public FunctionModule(string name, FunctionModuleArgs args, ComponentResourceOptions? options = null)
        : base("infra:module:FunctionApp", name, options)
    {
        var plan = new AppServicePlan($"{name}-plan", new AppServicePlanArgs
        {
            ResourceGroupName = args.ResourceGroupName,
            Location = args.Location,
            Kind = "FunctionApp",
            Sku = new SkuDescriptionArgs
            {
                Tier = "Dynamic",
                Name = "Y1"
            },
            Tags = args.Tags
        }, new CustomResourceOptions
        {
            Parent = this
        });

        var app = new WebApp(name, new WebAppArgs
        {
            ResourceGroupName = args.ResourceGroupName,
            Location = args.Location,
            ServerFarmId = plan.Id,
            Kind = "functionapp",
            Tags = args.Tags,
            SiteConfig = new SiteConfigArgs
            {
                AppSettings =
                {
                    new NameValuePairArgs { Name = "FUNCTIONS_EXTENSION_VERSION", Value = "~4" },
                    new NameValuePairArgs { Name = "FUNCTIONS_WORKER_RUNTIME", Value = "dotnet-isolated" },
                    new NameValuePairArgs
                    {
                        Name = "APPINSIGHTS_INSTRUMENTATIONKEY",
                        Value = args.AppInsightsInstrumentationKey
                    },
                    // Dans un vrai projet, utiliser une connection string complète
                    new NameValuePairArgs
                    {
                        Name = "AzureWebJobsStorage",
                        Value = Output.Format($"DefaultEndpointsProtocol=https;AccountName={args.StorageAccountName};EndpointSuffix=core.windows.net")
                    }
                }
            }
        }, new CustomResourceOptions
        {
            Parent = this
        });

        DefaultHostname = app.DefaultHostName;

        RegisterOutputs(new Dictionary<string, object?>
        {
            ["hostname"] = DefaultHostname
        });
    }
}