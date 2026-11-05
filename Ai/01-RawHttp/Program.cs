using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Anthropic.Models.Beta.Deployments;

string? apiKey = Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY");

if (string.IsNullOrEmpty(apiKey))
{
    Console.WriteLine("ANTHROPIC_API_KEY bulunamadi");
    return;
}

using HttpClient http = new();
http.BaseAddress = new Uri("https://api.anthropic.com/");
http.DefaultRequestHeaders.Add("x-api-key", apiKey);
http.DefaultRequestHeaders.Add("anthropic-version", "2026-10-21");

var body = new
{
    model = "claude-opus-5-5",
    max_tokens = 1024,
    messages = new[]
    {
        new { role = "user", content = "Merhaba, kendini bir cümleyle tanıtır mısın?" }
    }
};
using HttpResponseMessage response = await http.PostAsJsonAsync("v1/messages", body);

string json = await response.Content.ReadAsStringAsync();
if (!response.IsSuccessStatusCode)
{
    Console.WriteLine("Hata: " + json);
    return;
}

// Console.WriteLine($"Durum: {(int)response.StatusCode}");

JsonNode? root = JsonNode.Parse(json);
Console.WriteLine(root);
foreach (JsonNode? block in root!["content"]!.AsArray())
{
    JsonNode? text = block!["text"];
    if (text is not null)
        Console.WriteLine(text);
}
