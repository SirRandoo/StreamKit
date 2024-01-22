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
using FluentResults;
using Tomlyn;
using Tomlyn.Syntax;

namespace StreamKit.Api;

/// <summary>
///     An implementation of an <see cref="IDataSerializer{T}" /> for
///     reading and writing toml setting files.
/// </summary>
/// <typeparam name="T">
///     The type of the object that will be loaded, or saved, from this
///     loader.
/// </typeparam>
public class TomlDataSerializer<T> : IDataSerializer<T> where T : class, new()
{
    /// <inheritdoc />
    public Result<T> Deserialize(Stream stream)
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
            return new ExceptionalError("Could not deserialize toml object from stream", e);
        }

        if (Toml.TryToModel(content, out T? data, out DiagnosticsBag? bag))
        {
            return data!;
        }

        var errorMessages = new TomlError[bag!.Count];

        for (var i = 0; i < bag.Count; i++)
        {
            DiagnosticMessage error = bag[i];

            errorMessages[i] = new TomlError(error.Kind, error.Span, error.Message);
        }

        return Result.Fail(errorMessages);
    }

    /// <inheritdoc />
    public Result Serialize(Stream stream, [DisallowNull] T data)
    {
        try
        {
            using (var writer = new StreamWriter(stream))
            {
                if (Toml.TryFromModel(data, writer, out DiagnosticsBag bag))
                {
                    return Result.Ok();
                }

                var messages = new TomlError[bag.Count];

                for (var i = 0; i < bag.Count; i++)
                {
                    DiagnosticMessage error = bag[i];

                    messages[i] = new TomlError(error.Kind, error.Span, error.Message);
                }

                return Result.Fail(messages);
            }
        }
        catch (Exception e)
        {
            return new ExceptionalError("Could not serialize toml object into stream", e);
        }
    }

    /// <inheritdoc />
    public async Task<Result<T>> DeserializeAsync(Stream stream)
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
            return new ExceptionalError("Could not deserialize toml object from stream", e);
        }

        if (Toml.TryToModel(content, out T? data, out DiagnosticsBag? bag))
        {
            return data!;
        }

        var messages = new TomlError[bag!.Count];

        for (var i = 0; i < bag.Count; i++)
        {
            DiagnosticMessage error = bag[i];

            messages[i] = new TomlError(error.Kind, error.Span, error.Message);
        }

        return Result.Fail(messages);
    }

    /// <inheritdoc />
    public Task<Result> SerializeAsync(Stream stream, [DisallowNull] T data)
    {
        try
        {
            using (var writer = new StreamWriter(stream))
            {
                if (Toml.TryFromModel(data, writer, out DiagnosticsBag bag))
                {
                    return Task.FromResult(Result.Ok());
                }

                var messages = new TomlError[bag.Count];

                for (var i = 0; i < bag.Count; i++)
                {
                    DiagnosticMessage error = bag[i];

                    messages[i] = new TomlError(error.Kind, error.Span, error.Message);
                }

                return Task.FromResult(Result.Fail(messages));
            }
        }
        catch (Exception e)
        {
            return Task.FromResult((Result)new ExceptionalError("Could not serialize toml object into stream", e));
        }
    }
}

public class TomlError : Error
{
    public TomlError(DiagnosticMessageKind kind, SourceSpan span, string message) : base(message)
    {
        Kind = kind;
        Span = span;
    }

    public TomlError(DiagnosticMessageKind kind, SourceSpan span, string message, IError causedBy) : base(message, causedBy)
    {
        Kind = kind;
        Span = span;
    }

    public SourceSpan Span { get; }
    public DiagnosticMessageKind Kind { get; }
}
