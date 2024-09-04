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

using StreamKit.Common.Data.Abstractions;
using StreamKit.Mod.Api;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Core.Windows;

// TODO: This class requires a system for managing viewers who want pawns.
// TODO: Map out the user data this class requires in order to complete queue tasks.
// TODO: This class requires special wrapper classes for converting pawns into "IAssignableEntity" instances.

// FIXME: This class stores a "read-only" copy of data that isn't periodically updated.

public class PawnQueueWindow : Window
{
    private IUser[] _users = [];

    private PawnQueueWindow()
    {
    }

    /// <inheritdoc />
    public override void DoWindowContents(Rect inRect)
    {
        var previewRegion = new Rect(0f, 0f, inRect.width * 0.2f, inRect.width * 0.2f);
    }

    public static PawnQueueWindow CreateDefaultInstance() => new();

#if DEBUG
    /// <summary>
    ///     Creates a debug instance of a pawn queue window preloaded with generated data.
    /// </summary>
    public static PawnQueueWindow CreateDebugInstance() => new() { _users = PseudoDataGenerator.Instance.GeneratePseudoUsers(50) };
#endif
}
