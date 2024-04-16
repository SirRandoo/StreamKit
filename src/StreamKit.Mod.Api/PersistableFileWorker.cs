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
using Remora.Results;

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
public class PersistableFileWorker<T>(string persistableId, IDataSerializer dataSerializer)
{
    private readonly ConcurrentHashSet<string> _blockedFiles = [];

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
    public bool IsSavingBlocked(string path) => _blockedFiles.Contains(path);

    /// <summary>
    ///     Blocks a given path from being written to on disk by the file worker.
    /// </summary>
    /// <param name="path">The path being blocked.</param>
    /// <returns>Whether the path was blocked.</returns>
    /// <remarks>
    ///     When a file worker encounters a problems loading data from disk, the file worker will forbid
    ///     itself from writing data to a file on disk to prevent data loss.
    /// </remarks>
    public bool BlockSaving(string path) => _blockedFiles.Add(path);

    /// <summary>
    ///     Loads and deserializes the contents of the file at the given path.
    /// </summary>
    /// <param name="path">The path to a file on disk to load data from.</param>
    public Result<T> Load(string path)
    {
        if (!File.Exists(path))
        {
            return new NotFoundError($"The file at {path} was not found.");
        }

        try
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan))
            {
                Result<T> content = DataSerializer.Deserialize<T>(stream);

                if (!content.IsSuccess)
                {
                    BlockSaving(path);
                }

                return content;
            }
        }
        catch (Exception e)
        {
            BlockSaving(path);

            return new ExceptionError(e, $"Could not load file at {path}.");
        }
    }

    /// <summary>
    ///     Loads and deserializes the contents of the file at the given path.
    /// </summary>
    /// <param name="path">The path to a file on disk to load data from.</param>
    public async Task<Result<T>> LoadAsync(string path)
    {
        if (!File.Exists(path))
        {
            return new NotFoundError($"The file at {path} was not found.");
        }

        try
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan | FileOptions.Asynchronous))
            {
                Result<T> content = await DataSerializer.DeserializeAsync<T>(stream);

                if (!content.IsSuccess)
                {
                    BlockSaving(path);
                }

                return content;
            }
        }
        catch (Exception e)
        {
            BlockSaving(path);

            return new ExceptionError(e, $"Could not load file at {path}.");
        }
    }

    /// <summary>
    ///     Atomically saves the data provided into the file at the given path.
    /// </summary>
    /// <param name="path">The path to a file on disk to save data to.</param>
    /// <param name="data">The data to save to disk.</param>
    public Result Save(string path, [DisallowNull] T data)
    {
        if (IsSavingBlocked(path))
        {
            return new InvalidOperationError();
        }

        Result<string> tempFileResult = GetTemporaryFile(path);

        if (!tempFileResult.IsSuccess)
        {
            return Result.FromError(tempFileResult);
        }

        try
        {
            using (var stream = new FileStream(tempFileResult.Entity, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 4096, FileOptions.SequentialScan))
            {
                Result result = DataSerializer.Serialize(stream, data);

                if (!result.IsSuccess)
                {
                    BlockSaving(path);
                }

                return result;
            }
        }
        catch (Exception e)
        {
            return new ExceptionError(e, $"Could not save file at {path}.");
        }
    }

    /// <summary>
    ///     Atomically saves the data provided into the file at the given path.
    /// </summary>
    /// <param name="path">The path to a file on disk to save data to.</param>
    /// <param name="data">The data to save to disk.</param>
    public async Task<Result> SaveAsync(string path, [DisallowNull] T data)
    {
        if (IsSavingBlocked(path))
        {
            return new InvalidOperationError();
        }

        Result<string> tempFileResult = GetTemporaryFile(path);

        if (!tempFileResult.IsSuccess)
        {
            return Result.FromError(tempFileResult);
        }

        try
        {
            using (var stream = new FileStream(
                tempFileResult.Entity,
                FileMode.OpenOrCreate,
                FileAccess.Write,
                FileShare.Read,
                4096,
                FileOptions.SequentialScan | FileOptions.Asynchronous
            ))
            {
                Result result = await DataSerializer.SerializeAsync(stream, data);

                if (!result.IsSuccess)
                {
                    BlockSaving(path);
                }

                return result;
            }
        }
        catch (Exception e)
        {
            return new ExceptionError(e, $"Could not load file at {path}.");
        }
    }

    private static Result<string> GetTemporaryFile(string originalFilePath)
    {
        string? directory = Path.GetDirectoryName(originalFilePath);

        if (string.IsNullOrEmpty(directory))
        {
            return new InvalidOperationError("The path specified does not reside in a directory.");
        }

        string tempFileName = Path.GetRandomFileName();

        return Path.Combine(directory, tempFileName);
    }

    private static Result ReplaceFile(string targetFile, string replacementFile)
    {
        if (!File.Exists(replacementFile))
        {
            return new NotFoundError($"The file at {replacementFile} does not exist.");
        }

        string fileExtension = Path.GetExtension(targetFile);
        string backupFilePath = Path.ChangeExtension(targetFile, $".bck.{fileExtension}");

        try
        {
            File.Replace(replacementFile, targetFile, backupFilePath);

            return Result.Success;
        }
        catch (Exception e)
        {
            return new ExceptionError(e, $"Could not replace file {targetFile} with {replacementFile}");
        }
    }
}
