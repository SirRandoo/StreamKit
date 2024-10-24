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
using ConcurrentCollections;
using JetBrains.Annotations;
using NLog;
using StreamKit.Mod.Shared.Logging;

namespace StreamKit.Mod.Api;

/// <summary>
///     A class used to perform file operations on data types within the mod.
/// </summary>
/// <param name="persistableId">A unique id used to identify the data operations being performed.</param>
/// <param name="dataSerializer">The data serializer to used to transform data objects.</param>
/// <typeparam name="T">The type of the data object being (de)serialized by the file worker.</typeparam>
/// <remarks>
///     The purpose of this class is to perform file operations within the mod. These operations are
///     done in a specific way as to ensure any errors that occur during a file operation will
///     subsequently lock the file from further modifications from the same file worker. These locks
///     only apply to the current instance of the file worker, and do not lock the file for other file
///     workers, external programs, or the operating system.
/// </remarks>
[PublicAPI]
public class PersistableFileWorker<T>(string persistableId, IDataSerializer dataSerializer)
{
    private readonly ConcurrentHashSet<string> _blockedFiles = [];
    private readonly Logger _logger = KitLogManager.GetLogger<PersistableFileWorker<T>>();

    /// <summary>A unique id used to identify the data operations being performed.</summary>
    public string PersistableId { get; init; } = persistableId;

    /// <summary>The data serializer to used to transform data objects.</summary>
    public IDataSerializer DataSerializer { get; init; } = dataSerializer;

    /// <summary>
    ///     Returns whether a given path on disk is blocked from being written to.
    /// </summary>
    /// <param name="path">The path being queried about.</param>
    /// <remarks>
    ///     When a file worker encounters a problem loading data from disk, the file worker will forbid
    ///     itself from writing data to the file on disk to prevent data loss.
    /// </remarks>
    public bool IsSavingBlocked(string path) => _blockedFiles.Contains(Path.GetFullPath(path));

    /// <summary>
    ///     Blocks a given path from being written to on disk by the file worker.
    /// </summary>
    /// <param name="path">The path being blocked.</param>
    /// <returns>Whether the path was blocked.</returns>
    /// <remarks>
    ///     When a file worker encounters a problems loading data from disk, the file worker will forbid
    ///     itself from writing data to a file on disk to prevent data loss.
    /// </remarks>
    public bool BlockSaving(string path) => _blockedFiles.Add(Path.GetFullPath(path));

    /// <summary>
    ///     Loads and deserializes the contents of the file at the given path.
    /// </summary>
    /// <param name="path">The path to a file on disk to load data from.</param>
    public T? Load(string path)
    {
        if (!File.Exists(path))
        {
            _logger.Warn("File not found: {Path}", path);

            return default;
        }

        try
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan))
            {
                return DataSerializer.Deserialize<T>(stream);
            }
        }
        catch (Exception e)
        {
            _logger.Error(e, "Could not load file from disk @ {FilePath} ; further save attempts will be blocked", path);

            BlockSaving(path);

            return default;
        }
    }

    /// <summary>
    ///     Loads and deserializes the contents of the file at the given path.
    /// </summary>
    /// <param name="path">The path to a file on disk to load data from.</param>
    public async Task<T?> LoadAsync(string path)
    {
        if (!File.Exists(path))
        {
            _logger.Warn("File not found: {Path}", path);

            return default;
        }

        try
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan | FileOptions.Asynchronous))
            {
                return await DataSerializer.DeserializeAsync<T>(stream);
            }
        }
        catch (Exception e)
        {
            _logger.Error(e, "Could not load file from disk @ {FilePath}", path);

            BlockSaving(path);

            return default;
        }
    }

    /// <summary>
    ///     Atomically saves the data provided into the file at the given path.
    /// </summary>
    /// <param name="path">The path to a file on disk to save data to.</param>
    /// <param name="data">The data to save to disk.</param>
    public void Save(string path, [DisallowNull] T data)
    {
        if (IsSavingBlocked(path))
        {
            _logger.Warn("Save attempts are blocked on the file {Path}", path);

            return;
        }

        string? tempFileResult = GetTemporaryFile(path);

        if (string.IsNullOrEmpty(tempFileResult))
        {
            return;
        }

        try
        {
            using (var stream = new FileStream(tempFileResult, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 4096, FileOptions.SequentialScan))
            {
                DataSerializer.Serialize(stream, data);
            }
        }
        catch (Exception e)
        {
            _logger.Error(e, "Could not save file to disk @ {FilePath}", path);
        }
    }

    /// <summary>
    ///     Atomically saves the data provided into the file at the given path.
    /// </summary>
    /// <param name="path">The path to a file on disk to save data to.</param>
    /// <param name="data">The data to save to disk.</param>
    public async Task SaveAsync(string path, [DisallowNull] T data)
    {
        if (IsSavingBlocked(path))
        {
            _logger.Warn("Save attempts are blocked on the file {Path}", path);
        }

        string? tempFileResult = GetTemporaryFile(path);

        if (string.IsNullOrEmpty(tempFileResult))
        {
            return;
        }

        try
        {
            using (var stream = new FileStream(
                tempFileResult,
                FileMode.OpenOrCreate,
                FileAccess.Write,
                FileShare.Read,
                4096,
                FileOptions.SequentialScan | FileOptions.Asynchronous
            ))
            {
                await DataSerializer.SerializeAsync(stream, data);
            }
        }
        catch (Exception e)
        {
            _logger.Error(e, "Could not load file at path {Path}", path);
        }
    }

    private string? GetTemporaryFile(string originalFilePath)
    {
        string? directory = Path.GetDirectoryName(originalFilePath);

        if (string.IsNullOrEmpty(directory))
        {
            _logger.Warn("The path specified doesn't reside in a directory ({Path})", originalFilePath);

            return null;
        }

        string tempFileName = Path.GetRandomFileName();

        return Path.Combine(directory, tempFileName);
    }

    private void ReplaceFile(string targetFile, string replacementFile)
    {
        if (!File.Exists(replacementFile))
        {
            _logger.Error("The file at {Path} does not exist", replacementFile);
        }

        string fileExtension = Path.GetExtension(targetFile);
        string backupFilePath = Path.ChangeExtension(targetFile, $".bck.{fileExtension}");

        try
        {
            File.Replace(replacementFile, targetFile, backupFilePath);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Could not replace file {Target} with {Replacement}", targetFile, replacementFile);
        }
    }
}
