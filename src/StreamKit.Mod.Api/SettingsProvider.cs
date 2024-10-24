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
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using HarmonyLib;
using NLog;
using StreamKit.Mod.Shared.Logging;

namespace StreamKit.Mod.Api;

public class SettingsProvider<T>(string id, string name) : ISettingsProvider<T> where T : class, IComponentSettings
{
    private readonly IDataSerializer _dataSerializer = JsonDataSerializer.Default;
    private readonly Logger _logger = KitLogManager.GetLogger($"StreamKit.Providers:Settings.{id}");

    /// <inheritdoc />
    public string Id
    {
        get => id;
        set => throw new ReadOnlyException("Cannot modify the settings provider id.");
    }

    /// <inheritdoc />
    public string Name { get; set; } = name;

    /// <inheritdoc />
    public bool TryLoadSettings(string path, [NotNullWhen(true)] out T? settings)
    {
        if (!File.Exists(path))
        {
            _logger.Trace("File {Path} does not exist :: Failing early...", path);

            settings = default;

            return false;
        }

        using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            var result = _dataSerializer.Deserialize<T?>(stream);

            if (result == default)
            {
                _logger.Warn("Could not load settings from path {Path} :: Continuing may result in your settings being lost", path);

                settings = null;

                return false;
            }

            _logger.Trace("Successfully loaded settings from path {Path}", path);
            settings = result;

            return true;
        }
    }

    /// <inheritdoc />
    public T GenerateDefaultSettings()
    {
        _logger.Debug("Generating default settings for type {QualifiedName} (constructor invocation)", typeof(T).FullDescription());

        return (T)Activator.CreateInstance(typeof(T), []);
    }

    /// <inheritdoc />
    public bool TrySaveSettings(string path, T settings)
    {
        using (FileStream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
        {
            try
            {
                _dataSerializer.Serialize(stream, settings);

                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Could not save settings to file {Path}", path);

                return false;
            }
        }
    }
}
