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
using JetBrains.Annotations;
using Tomlyn;
using Tomlyn.Syntax;

namespace StreamKit.Api;

public static class Toml
{
    private static readonly TomlModelOptions Options = new() { IgnoreMissingProperties = true };

    /// <summary>
    ///     Deserializes the JSON file specified into the type specified.
    /// </summary>
    /// <param name="filePath">The path to the json file to load.</param>
    /// <param name="defaultFactory">
    ///     A function that's called if the JSON file could not be
    ///     deserialized into an instance.
    /// </param>
    /// <typeparam name="T">
    ///     A type describing the data that's being loaded from the file.
    /// </typeparam>
    /// <returns>
    ///     The deserialized data, or a default value if the file was empty,
    ///     or the data was malformed.
    /// </returns>
    /// <exception cref="IOException">
    ///     An I/O error occurred while opening the file.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    ///     The file does not have permission to access the file, or the file
    ///     was a directory.
    /// </exception>
    /// <exception cref="PathTooLongException">
    ///     The file name, path, or a combination of both, exceeded the
    ///     underlying system's limits.
    /// </exception>
    public static T? Load<T>(string filePath, Func<T> defaultFactory) where T : class, new()
    {
        if (!File.Exists(filePath))
        {
            return defaultFactory();
        }

        string contents = File.ReadAllText(filePath);

        return !Tomlyn.Toml.TryToModel(contents, out T? model, out DiagnosticsBag _, filePath, Options) ? model : defaultFactory();
    }

    /// <summary>
    ///     Deserializes the JSON file specified into the type specified.
    /// </summary>
    /// <param name="filePath">The path to the json file to load.</param>
    /// <typeparam name="T">
    ///     A type describing the data that's being loaded from the file.
    /// </typeparam>
    /// <returns>
    ///     The deserialized data, or <c>null</c> if the file was empty, or
    ///     the data was malformed.
    /// </returns>
    /// <exception cref="IOException">
    ///     An I/O error occurred while opening the file.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    ///     The file does not have permission to access the file, or the file
    ///     was a directory.
    /// </exception>
    /// <exception cref="PathTooLongException">
    ///     The file name, path, or a combination of both, exceeded the
    ///     underlying system's limits.
    /// </exception>
    public static T? Load<T>(string filePath) where T : class, new()
    {
        if (!File.Exists(filePath))
        {
            return default;
        }

        string contents = File.ReadAllText(filePath);

        return !Tomlyn.Toml.TryToModel(contents, out T? model, out DiagnosticsBag _, filePath, Options) ? model : default;
    }

    /// <summary>
    ///     Serializes a type into the JSON file specified.
    /// </summary>
    /// <param name="filePath">The path to the json file to write to.</param>
    /// <param name="obj">The object to write to file.</param>
    /// <typeparam name="T">
    ///     A type describing the data that's being saved to the file.
    /// </typeparam>
    /// <exception cref="IOException">
    ///     An I/O error occurred while opening the file.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    ///     The file does not have permission to access the file, or the file
    ///     was a directory.
    /// </exception>
    /// <exception cref="PathTooLongException">
    ///     The file name, path, or a combination of both, exceeded the
    ///     underlying system's limits.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     The file name, path, or a combination of both, contained invalid
    ///     characters, or no characters at all.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     <see cref="filePath"/> was null.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    ///     The file specified could not be found.
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     <see cref="filePath"/> is invalid.
    /// </exception>
    public static void Save<T>(string filePath, [DisallowNull] T obj) where T : class
    {
        string directory = Path.GetDirectoryName(filePath)!;

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (Stream stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
        {
            using (TextWriter writer = new StreamWriter(stream))
            {
                Tomlyn.Toml.TryFromModel(obj, writer, out DiagnosticsBag _, Options);
            }
        }
    }
}
