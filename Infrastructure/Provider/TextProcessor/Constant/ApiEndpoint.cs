namespace Infrastructure.Provider.TextProcessor.Constant;

public static class ApiEndpoint
{
    public const string Embedding = "/process/embedding";
    public const string Tags = "/process/tags";
    public const string MostCommonTags = "/process/most-common-tags";
    public const string SplitFile = "/prepare/split-text";
    public const string NormalizeFile = "/prepare/normalize";
}