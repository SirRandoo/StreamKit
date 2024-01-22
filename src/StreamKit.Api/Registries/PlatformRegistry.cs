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

using System.Collections.Generic;
using UnityEngine;

namespace StreamKit.Api;

public record PlatformRegistry(IList<IPlatform> AllRegistrants) : FrozenRegistry<IPlatform>(AllRegistrants)
{
    public IPlatform RandomPlatform => AllRegistrants[Random.Range(0, AllRegistrants.Count - 1)];

    public static PlatformRegistry CreateDefaultInstance()
    {
        // TODO: Make this method return a list of platforms that exist within the current runtime.
        // Alternatively, forego this method since the api shouldn't inherently know about implementation details.
        return new PlatformRegistry([]);
    }

    public static PlatformRegistry CreateDebugInstance()
    {
        return new PlatformRegistry(
            [
                new Platform("twitch") { Name = "Twitch", Icon = Icons.Twitch },
                new Platform("trovo") { Name = "Trovo", Icon = Icons.Trovo },
                new Platform("youtube") { Name = "YouTube", Icon = Icons.YouTube },
                new Platform("kick") { Name = "Kick", Icon = Icons.Kick }
            ]
        );
    }
}

public record Platform(string Id) : IPlatform
{
    public required string Name { get; set; }
    public Texture2D? Icon { get; set; }
}
