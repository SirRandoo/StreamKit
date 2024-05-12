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
using System.Threading.Tasks;
using Remora.Results;
using Tomlet;

namespace StreamKit.Mod.Api;

/// <summary>
///     An implementation of a <see cref="IDataSerializer" /> for
///     reading and writing toml setting files.
/// </summary>
public class TomlDataSerializer : IDataSerializer
{
    /// <inheritdoc />
    public Result<T> Deserialize<T>(Stream stream)
    {
        string content;

        try
        {
            using (var reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            return new ExceptionError(e, "Could not deserialize toml object from stream");
        }

        try
        {
            return TomletMain.To<T>(content);
        }
        catch (Exception e)
        {
            return new ExceptionError(e, "Could not deserialize toml object into instance of class.");
        }
    }

    /// <inheritdoc />
    public Result Serialize<T>(Stream stream, [DisallowNull] T data)
    {
        try
        {
            using (var writer = new StreamWriter(stream))
            {
                string result = TomletMain.TomlStringFrom(typeof(T), data);

                writer.Write(result);

                return Result.Success;
            }
        }
        catch (Exception e)
        {
            return new ExceptionError(e, "Could not serialize toml object into stream");
        }
    }

    /// <inheritdoc />
    public async Task<Result<T>> DeserializeAsync<T>(Stream stream)
    {
        string content;

        try
        {
            using (var reader = new StreamReader(stream))
            {
                content = await reader.ReadToEndAsync();
            }
        }
        catch (Exception e)
        {
            return new ExceptionError(e, "Could not deserialize toml object from stream");
        }

        try
        {
            return TomletMain.To<T>(content);
        }
        catch (Exception e)
        {
            return new ExceptionError(e, "Could not deserialize toml object into instance of class.");
        }
    }

    /// <inheritdoc />
    public async Task<Result> SerializeAsync<T>(Stream stream, [DisallowNull] T data)
    {
        try
        {
            using (var writer = new StreamWriter(stream))
            {
                string result = TomletMain.TomlStringFrom(typeof(T), data);

                await writer.WriteAsync(result);

                return Result.Success;
            }
        }
        catch (Exception e)
        {
            return new ExceptionError(e, "Could not serialize toml object into stream");
        }
    }
}
