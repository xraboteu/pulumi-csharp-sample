using System.Collections.Generic;
using Pulumi;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;

namespace Infra.Modules.Storage;

public class StorageModuleArgs
{
    public Input<string> ResourceGroupName { get; set; } = null!;
    public Input<string> Location { get; set; } = null!;
    public InputMap<string>? Tags { get; set; }
}

public class StorageModule : ComponentResource
{
    public Output<string> AccountName { get; }

    public StorageModule(string name, StorageModuleArgs args, ComponentResourceOptions? options = null)
        : base("infra:module:Storage", name, options)
    {
        var account = new StorageAccount(name, new StorageAccountArgs
        {
            ResourceGroupName = args.ResourceGroupName,
            Location = args.Location,
            Sku = new SkuArgs
            {
                Name = SkuName.Standard_LRS
            },
            Kind = Kind.StorageV2,
            Tags = args.Tags
        }, new CustomResourceOptions
        {
            Parent = this
        });

        AccountName = account.Name;

        RegisterOutputs(new Dictionary<string, object?>
        {
            ["accountName"] = AccountName
        });
    }
}