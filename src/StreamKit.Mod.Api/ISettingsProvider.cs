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

using System.Diagnostics.CodeAnalysis;
using StreamKit.Common.Data.Abstractions;

namespace StreamKit.Mod.Api;

/// <summary>
///     An interface for defining a settings provider for a given <see cref="IComponent" />.
/// </summary>
public interface ISettingsProvider : IIdentifiable
{
    /// <summary>
    ///     Attempts to load the settings from the file path on disk.
    /// </summary>
    /// <param name="path">The path on disk to the serialized settings.</param>
    /// <param name="settings">
    ///     The settings deserialized into an implementation of <see cref="IComponentSettings" />.
    /// </param>
    /// <returns>
    ///     Whether the settings could successfully be loaded and deserialized.
    /// </returns>
    bool TryLoadSettings(string path, [NotNullWhen(true)] out IComponentSettings? settings);

    /// <summary>
    ///     Tries to save the component's settings from the file path on disk.
    /// </summary>
    /// <param name="path">
    ///     The path on disk to save the deserialized settings to.
    /// </param>
    /// <param name="settings">
    ///     A <see cref="IComponentSettings" /> implementation representing the settings being serialized.
    /// </param>
    /// <returns>
    ///     Whether the settings could successfully be serialized and written to disk.
    /// </returns>
    bool TrySaveSettings(string path, IComponentSettings settings);
}
