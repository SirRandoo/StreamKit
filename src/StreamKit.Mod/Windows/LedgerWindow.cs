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

using SirRandoo.CommonLib.Entities;
using SirRandoo.CommonLib.Interfaces;
using StreamKit.Api;
using StreamKit.Data.Abstractions;
using UnityEngine;
using Verse;

namespace StreamKit.Mod;

public class ViewerWindow : Window
{
    private static readonly IRimLogger Logger = new RimLogger("streamkit.windows.viewers");
    private bool _isDebugInstance;
    private ILedger? _ledger;

    // TODO: Replace the right branch with the actual viewer list.
    protected ILedger? ViewerList => _isDebugInstance ? _ledger : null;

    /// <inheritdoc/>
    public override void DoWindowContents(Rect inRect)
    {
    }

    /// <inheritdoc />
    public override void PreOpen()
    {
        if (ViewerList is null)
        {
            Log.Error();
        }
    }

    public static ViewerWindow CreateDebugInstance() => new() { _isDebugInstance = true, _ledger = PseudoDataGenerator.GeneratePseudoLedger(100, 50) };

    public static ViewerWindow CreateDefaultInstance() => new();
}
