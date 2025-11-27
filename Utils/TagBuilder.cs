using System.Collections.Generic;
using Pulumi;

namespace Infra.Utils;

public static class TagBuilder
{
    public static InputMap<string> Default(string environment)
    {
        return new InputMap<string>
        {
            { "env", environment },
            { "owner", "xavier" },
            { "managed-by", "pulumi" }
        };
    }
}