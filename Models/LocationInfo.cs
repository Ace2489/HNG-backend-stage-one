using System.Text.Json.Serialization;

namespace HNG_backend_stage_one.Models;

public record class LocationInfo
{
    [JsonPropertyName("city")]
    public string? City { get; set; } = null;

    
    [JsonPropertyName("lon")]
    public decimal Longitude { get; set; }

    [JsonPropertyName("lat")]
    public decimal Latitude { get; set; }

    public string? Error { get; set; } = null;
}
