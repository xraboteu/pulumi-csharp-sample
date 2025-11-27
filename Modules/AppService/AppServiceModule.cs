using System.Collections.Generic;
using Pulumi;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

namespace Infra.Modules.AppService;

public class AppServiceModuleArgs
{
    public Input<string> ResourceGroupName { get; set; } = null!;
    public Input<string> Location { get; set; } = null!;
    public InputMap<string>? Tags { get; set; }
}

public class AppServiceModule : ComponentResource
{
    public Output<string> DefaultHostname { get; }

    public AppServiceModule(string name, AppServiceModuleArgs args, ComponentResourceOptions? options = null)
        : base("infra:module:AppService", name, options)
    {
        var plan = new AppServicePlan($"{name}-plan", new AppServicePlanArgs
        {
            ResourceGroupName = args.ResourceGroupName,
            Location = args.Location,
            Kind = "App",
            Sku = new SkuDescriptionArgs
            {
                Tier = "Basic",
                Name = "B1"
            },
            Tags = args.Tags
        }, new CustomResourceOptions { Parent = this });

        var app = new WebApp(name, new WebAppArgs
        {
            ResourceGroupName = args.ResourceGroupName,
            Location = args.Location,
            ServerFarmId = plan.Id,
            Tags = args.Tags
        }, new CustomResourceOptions { Parent = this });

        DefaultHostname = app.DefaultHostName;

        RegisterOutputs(new Dictionary<string, object?>
        {
            ["hostname"] = DefaultHostname
        });
    }
}