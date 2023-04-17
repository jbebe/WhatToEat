using System.Text.Json;
using System.Text.Json.Serialization;

namespace WhatToEat.App.Common;

public record Id<T>(string Value)
{
	public override int GetHashCode() => Value.GetHashCode();
}

public class JsonIdConverter<T> : JsonConverter<Id<T>>
{
	public override Id<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value != null ? new Id<T>(value) : null;
	}

	public override void Write(Utf8JsonWriter writer, Id<T> value, JsonSerializerOptions options) => 
		writer.WriteStringValue(value.Value);
}