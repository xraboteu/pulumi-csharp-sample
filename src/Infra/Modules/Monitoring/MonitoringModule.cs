using System.Collections.Generic;
using Pulumi;
using Pulumi.AzureNative.Insights;
using Pulumi.AzureNative.Insights.Inputs;

namespace Infra.Modules.Monitoring;

public class MonitoringModuleArgs
{
    public Input<string> ResourceGroupName { get; set; } = null!;
    public Input<string> Location { get; set; } = null!;
    public InputMap<string>? Tags { get; set; }
}

public class MonitoringModule : ComponentResource
{
    public Output<string> InstrumentationKey { get; }

    public MonitoringModule(string name, MonitoringModuleArgs args, ComponentResourceOptions? options = null)
        : base("infra:module:Monitoring", name, options)
    {
        var appInsights = new Component(name, new ComponentArgs
        {
            ResourceGroupName = args.ResourceGroupName,
            Location = args.Location,
            ApplicationType = "web",
            Kind = "web",
            Tags = args.Tags
        }, new CustomResourceOptions
        {
            Parent = this
        });

        InstrumentationKey = appInsights.InstrumentationKey;

        RegisterOutputs(new Dictionary<string, object?>
        {
            ["instrumentationKey"] = InstrumentationKey
        });
    }
}