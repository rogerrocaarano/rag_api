using Application.Dto;
using Domain.Repository;
using Domain.Service;

namespace Application.Service;

public class ContextSeeder(
    IContextRepository repository,
    ITextProcessorService textProcessor,
    string seedDirectory
)
{
    public async Task SeedData()
    {
        var jsonFiles = Directory.GetFiles(seedDirectory, "*.json");
        foreach (var jsonFilePath in jsonFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(jsonFilePath);
            var textFilePath = Path.Combine(seedDirectory, $"{fileName}.txt");
            if (File.Exists(textFilePath))
            {
                await SeedFile(textFilePath, jsonFilePath);
            }
        }
    }

    private async Task SeedFile(string filePath, string metaDataPath)
    {
        var jsonFile = await File.ReadAllTextAsync(metaDataPath);
        try
        {
            var metadata = System.Text.Json.JsonSerializer.Deserialize<MetaDataFile>(jsonFile);
            if (metadata == null)
            {
                throw new Exception($"Error deserializing metadata from {metaDataPath}");
            }

            var contextId = await SaveContext(metadata);
            await SaveFragments(contextId, filePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<Guid> SaveContext(MetaDataFile metadata)
    {
        var contextName = metadata.Name;
        var contextDescription = metadata.Description;
        var tags = await textProcessor.ExtractTagsFromText(contextDescription, 20);
        var embedding = await textProcessor.GenerateEmbeddingFromText(contextDescription);
        return await repository.AddContext(contextName, contextDescription, tags, embedding);
    }

    private async Task SaveFragments(Guid contextId, string filePath)
    {
        var fileFragments = await textProcessor.SplitTextFile(filePath);
        for (var i = 0; i < fileFragments.Count; i++)
        {
            var content = fileFragments[i];
            var fragmentTags = await textProcessor.ExtractTagsFromText(content);
            var fragmentEmbedding = await textProcessor.GenerateEmbeddingFromText(fragmentTags.ToString());
            await repository.AddFragment(content, fragmentTags, i, contextId, fragmentEmbedding);
        }
    }
}