// MIT License
//
// Copyright (c) 2023 SirRandoo
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NLog;
using StreamKit.Mod.Shared.Logging;

namespace StreamKit.Mod.Api;

/// <summary>
///     Represents a wrapper class around System.Text.Json's <see cref="JsonSerializer" /> that wraps
///     exceptions in a <see cref="ExceptionError" /> instance.
/// </summary>
/// <param name="serializerOptions">
///     The optional serializer options for the
///     <see cref="JsonSerializer" />.
/// </param>
public class JsonDataSerializer(JsonSerializerOptions? serializerOptions = null) : IDataSerializer
{
    private static readonly Logger Logger = KitLogManager.GetLogger<JsonDataSerializer>();
    private static readonly Lazy<JsonDataSerializer> DefaultSerializer = new(CreateDefaultInstance);

    public static JsonDataSerializer Default => DefaultSerializer.Value;

    /// <inheritdoc />
    public T? Deserialize<T>(Stream stream)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(stream)!;
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error deserializing json object from stream");

            return default;
        }
    }

    /// <inheritdoc />
    public void Serialize<T>(Stream stream, [DisallowNull] T data)
    {
        try
        {
            JsonSerializer.Serialize(stream, data, serializerOptions);
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error serializing json object into stream");
        }
    }

    /// <inheritdoc />
    public async Task<T?> DeserializeAsync<T>(Stream stream)
    {
        try
        {
            return (await JsonSerializer.DeserializeAsync<T>(stream))!;
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error deserializing json object from stream");

            return default;
        }
    }

    /// <inheritdoc />
    public async Task SerializeAsync<T>(Stream stream, [DisallowNull] T data)
    {
        try
        {
            await JsonSerializer.SerializeAsync(stream, data);
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error serializing json object into stream");
        }
    }

    private static JsonDataSerializer CreateDefaultInstance() => new(
        new JsonSerializerOptions(JsonSerializerDefaults.General)
        {
            NumberHandling = JsonNumberHandling.Strict,
            ReadCommentHandling = JsonCommentHandling.Skip,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            AllowTrailingCommas = true,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
        }
    );
}
