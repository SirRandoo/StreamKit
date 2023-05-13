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
using JetBrains.Annotations;
using RimWorld;

namespace StreamKit.Async.Wrappers
{
    /// <summary>
    ///     A set of wrappers around the synchronous, unsafe methods within
    ///     the <see cref="IncidentWorker"/> class.
    /// </summary>
    /// <remarks>
    ///     Usage of the extensions provided by this class are to be used
    ///     with care as RimWorld was not designed with asynchronous code in
    ///     mind.
    /// </remarks>
    public static class IncidentWorkerExtensions
    {
        /// <inheritdoc cref="IncidentWorker.FiredTooRecently"/>
        public static async Task<bool> FiredTooRecentlyAsync([NotNull] this IncidentWorker worker, IIncidentTarget target) =>
            await TaskExtensions.OnMainAsync(worker.FiredTooRecently, target);

        /// <inheritdoc cref="IncidentWorker.CanFireNow"/>
        public static async Task<bool> CanFireNowAsync([NotNull] this IncidentWorker worker, IncidentParms @params) =>
            await TaskExtensions.OnMainAsync(worker.CanFireNow, @params);

        /// <inheritdoc cref="IncidentWorker.TryExecute"/>
        public static async Task<bool> TryExecuteAsync([NotNull] this IncidentWorker worker, IncidentParms @params) =>
            await TaskExtensions.OnMainAsync(worker.TryExecute, @params);
    }
}
