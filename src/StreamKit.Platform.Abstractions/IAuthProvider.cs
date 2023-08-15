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
using System.ComponentModel;

namespace StreamKit.Platform.Abstractions;

/// <summary>
///     Defines a special class used for authenticating to a platform.
/// </summary>
public interface IAuthProvider : IPlatformSided, INotifyPropertyChanged
{
    /// <summary>
    ///     Returns a url viewers can use to authorize the mod.
    /// </summary>
    Uri AuthLink { get; }

    /// <summary>
    ///     Whether there's been a change that requires users to
    ///     reauthenticate.
    /// </summary>
    /// <remarks>
    ///     This is generally used as a hint to the mod that the user must
    ///     reauthenticate for a new feature to work. However,
    ///     re-authentication is optional and developers of the feature
    ///     should ensure their code works properly when users haven't
    ///     re-authenticated.
    /// </remarks>
    bool RequiresReAuth { get; }
}
