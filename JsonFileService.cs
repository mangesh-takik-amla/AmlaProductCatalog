using System.Text.Json;
using AmlaProductCatalog;
using AmlaProductCatalog.Models;

public class JsonFileService
{
    private readonly string _filePath = "data.json";

    public async Task<List<UserRequest>> GetAsync()
    {
        if (!File.Exists(_filePath))
            return new List<UserRequest>();

        var json = await File.ReadAllTextAsync(_filePath);

        return string.IsNullOrWhiteSpace(json)
            ? new List<UserRequest>()
            : JsonSerializer.Deserialize<List<UserRequest>>(json);
    }

    public async Task SaveAsync(List<UserRequest> data)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await File.WriteAllTextAsync(_filePath, json);
    }
}