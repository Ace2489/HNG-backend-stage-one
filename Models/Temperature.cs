using System.Text.Json.Serialization;

namespace HNG_backend_stage_one;

public record class Root
{
    [JsonPropertyName("main")]
    public Temperature? Temperature { get; set; }
}
public record class Temperature
{
    [JsonPropertyName("temp")]
    public decimal Temp { get; set; }

    public string? Error { get; set; } = null;
}
