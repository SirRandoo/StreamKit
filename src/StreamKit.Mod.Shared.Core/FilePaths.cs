// MIT License
//
// Copyright (c) 2024 SirRandoo
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
using System.IO;
using StreamKit.Mod.Api;
using Verse;

namespace StreamKit.Mod.Shared.Core;

/// <summary>
///     A static class for obtaining paths to certain resources on disk in a predictable way.
/// </summary>
public static class FilePaths
{
    private static readonly IRimLogger Logger = new RimLogger("StreamKit.FileSystem");
    private static readonly string BaseDirectory = GetDirectory(GenFilePaths.SaveDataFolderPath, "StreamKit", true);
    private static readonly string SettingsBase = GetDirectory(BaseDirectory, "settings", true);
    private static readonly string DataBase = GetDirectory(BaseDirectory, "data", true);

    /// <summary>
    ///     The path to the Sqlite3 database containing all the mod's data.
    /// </summary>
    public static readonly string DatabaseFile = GetDataPath("data.db");

    /// <summary>
    ///     Returns a file path within the root StreamKit directory.
    /// </summary>
    /// <param name="fileName">
    ///     The name of the file within the directory. This must include the extension of the file.
    /// </param>
    public static string GetRootFile(string fileName) => Path.Combine(BaseDirectory, fileName);

    /// <summary>
    ///     Returns a directory stitched from a parent and child directories.
    /// </summary>
    /// <param name="parent">
    ///     The directory <see cref="directory" /> should be created in.
    /// </param>
    /// <param name="directory">
    ///     The name of the directory within <see cref="parent" />.
    /// </param>
    /// <param name="ensureExists">
    ///     When true, <see cref="directory" /> will attempt to be created within <see cref="parent" />. If
    ///     the mod wasn't able to create the directory at the given location, only a message to RimWorld's
    ///     dev log will be emitted.
    /// </param>
    public static string GetDirectory(string parent, string directory, bool ensureExists)
    {
        string path = Path.Combine(parent, directory);

        if (!ensureExists || Directory.Exists(path))
        {
            return path;
        }

        try
        {
            Directory.CreateDirectory(path);
        }
        catch (Exception e)
        {
            Logger.Error($"{path} could not be created -- Things wil not work correctly.", e);
        }

        return path;
    }

    /// <summary>
    ///     Returns a toml file appropriate for storing settings for a given component.
    /// </summary>
    /// <param name="component">
    ///     The component whose settings file is being obtained.
    /// </param>
    public static string GetSettingsPath(IComponent component) => Path.Combine(SettingsBase, $"{component.Id}.toml");

    /// <summary>
    ///     Returns a file appropriate for storing arbitrary data according to the caller's specifications.
    /// </summary>
    /// <param name="component">
    ///     The component whose data file is being obtained.
    /// </param>
    /// <param name="dataType">The data type of the file that was obtained.</param>
    public static string GetDataPath(IComponent component, string dataType) => GetDataPath($"{component.Id}.{dataType}");

    /// <summary>
    ///     Returns a file appropriate for storing arbitrary data.
    /// </summary>
    /// <param name="fileName">The name of the file, optionally including its suffix.</param>
    public static string GetDataPath(string fileName) => Path.Combine(DataBase, fileName);
}
