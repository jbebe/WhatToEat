using System.Text.Json;
using System.Text.Json.Serialization;

namespace WhatToEat.App.Common;

public class Id<T>
{
	public string Value { get; set; }

	public Id(string? value = null)
    {
		Value = value ?? ModelHelpers.GenerateId();
	}

    public override int GetHashCode() => Value.GetHashCode();

	public override string ToString() => $"Id<{typeof(T).Name}>(\"{Value}\")";

	public override bool Equals(object? obj) => Value.Equals(obj);
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