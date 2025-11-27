namespace Infra.Utils;

public static class NameFactory
{
    public static string ResourceGroupName(string baseName, string environment)
        => $"{baseName}-{environment}-rg";

    public static string Suffix(string baseName, string postfix)
        => $"{baseName}-{postfix}";
}