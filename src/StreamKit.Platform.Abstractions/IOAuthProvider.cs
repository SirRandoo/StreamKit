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

namespace StreamKit.Platform.Abstractions;

/// <summary>
///     Defines a special class for authenticating to a platform's
///     through
///     the oauth specification.
/// </summary>
/// <typeparam name="TScope">
///     An enumeration representing the scopes the platform supports.
/// </typeparam>
public interface IOAuthProvider<TScope> where TScope : Enum, IAuthProvider
{
    /// <summary>
    ///     A collection indicating the scopes the mod is authorized for.
    /// </summary>
    TScope[] Scopes { get; }

    /// <summary>
    ///     Includes one or more scopes in the
    ///     <see cref="IAuthProvider.AuthLink"/>.
    /// </summary>
    /// <param name="scopes">The scope(s) being included.</param>
    /// <returns>Whether the scope was included.</returns>
    /// <remarks>
    ///     Including scopes will require users to reauthenticate.
    ///     Re-authentication is optional and developers should ensure their
    ///     code makes no guarantees a scope exists for the given token.
    /// </remarks>
    bool IncludeScopes(params TScope[] scopes);

    /// <summary>
    ///     Excludes one or more scopes in the
    ///     <see cref="IAuthProvider.AuthLink"/>.
    /// </summary>
    /// <param name="scopes">The scope(s) being excluded.</param>
    /// <returns>Whether the scope was excluded.</returns>
    /// <remarks>
    ///     Excluding scopes won't require users to reauthenticate, but the
    ///     scopes will be removed the next time the user authenticates.
    /// </remarks>
    bool ExcludeScopes(params TScope[] scopes);
}
