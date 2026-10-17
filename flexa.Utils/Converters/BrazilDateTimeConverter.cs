using System.Text.Json;
using System.Text.Json.Serialization;

namespace flexa.Utils.Converters
{
    /// <summary>
    /// JsonConverter for DateTime that forces values to be interpreted and serialized in Brazil (America/Sao_Paulo) timezone.
    /// </summary>
    public class BrazilDateTimeConverter : JsonConverter<DateTime>
    {
        private static readonly TimeZoneInfo BrazilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Assume incoming string is UTC or unspecified; parse then convert to Brazil timezone.
            var dateTime = reader.GetDateTime();
            // If Kind is Unspecified, treat as UTC.
            if (dateTime.Kind == DateTimeKind.Unspecified)
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            // Convert to Brazil time.
            var brazilTime = TimeZoneInfo.ConvertTime(dateTime, BrazilTimeZone);
            // Return as DateTime with Kind Unspecified (or Local) to avoid double conversion later.
            return DateTime.SpecifyKind(brazilTime, DateTimeKind.Unspecified);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Assume value is in Brazil timezone (Unspecified). Convert to UTC for consistent ISO format.
            var dateTimeUtc = value.Kind == DateTimeKind.Utc
                ? value
                : TimeZoneInfo.ConvertTimeToUtc(value, BrazilTimeZone);
            writer.WriteStringValue(dateTimeUtc.ToString("o")); // ISO 8601 format
        }
    }
}
