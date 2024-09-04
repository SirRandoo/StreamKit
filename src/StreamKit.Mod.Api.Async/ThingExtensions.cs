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

using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace StreamKit.Mod.Api.Async;

/// <summary>
///     A set of wrappers around the synchronous, unsafe methods within
///     the <see cref="Thing" /> class.
/// </summary>
/// <remarks>
///     Usage of the extensions provided by this class are to be used
///     with care as RimWorld was not designed with asynchronous code in
///     mind.
/// </remarks>
public static class ThingExtensions
{
    /// <inheritdoc cref="ThingCompUtility.TryGetComp{T}(Thing)" />
    public static async Task<T> GetCompAsync<T>(this Thing thing) where T : ThingComp => await MainThreadExtensions.OnMainAsync(thing.TryGetComp<T>);

    /// <inheritdoc cref="QualityUtility.TryGetQuality" />
    public static async Task<QualityCategory?> GetQualityAsync(this Thing thing)
    {
        return await MainThreadExtensions.OnMainAsync(GetQuality, thing);

        QualityCategory? GetQuality(Thing t)
        {
            if (t.TryGetQuality(out QualityCategory category))
            {
                return category;
            }

            return null;
        }
    }
}
