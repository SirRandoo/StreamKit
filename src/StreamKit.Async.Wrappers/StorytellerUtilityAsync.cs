// MIT License
//
// Copyright (c) 2022 SirRandoo
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

namespace StreamKit.Async.Wrappers;

/// <summary>
///     A set of wrappers around the synchronous, unsafe methods within
///     the <see cref="StorytellerUtility"/> class.
/// </summary>
/// <remarks>
///     Usage of the extensions provided by this class are to be used
///     with care as RimWorld was not designed with asynchronous code in
///     mind.
/// </remarks>
public static class StorytellerUtilityAsync
{
    /// <inheritdoc cref="StorytellerUtility.DefaultParmsNow"/>
    public static async ValueTask<IncidentParms> DefaultParmsNowAsync(IncidentCategoryDef category, IIncidentTarget target) =>
        await TaskExtensions.OnMainAsync(StorytellerUtility.DefaultParmsNow, category, target);

    /// <inheritdoc cref="StorytellerUtility.DefaultThreatPointsNow"/>
    public static async Task<float> DefaultThreatPointsNowAsync(IIncidentTarget target) =>
        await TaskExtensions.OnMainAsync(StorytellerUtility.DefaultThreatPointsNow, target);

    /// <inheritdoc cref="StorytellerUtility.DefaultSiteThreatPointsNow"/>
    public static async Task<float> DefaultSitePointsNowAsync() => await TaskExtensions.OnMainAsync(StorytellerUtility.DefaultSiteThreatPointsNow);
}
