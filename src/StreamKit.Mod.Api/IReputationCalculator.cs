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

using StreamKit.Common.Data.Abstractions;

namespace StreamKit.Mod.Api;

public interface IReputationCalculator
{
    /// <summary>
    ///     Calculates a score that's used to indicate the viewer's overall reputation.
    /// </summary>
    /// <param name="viewer">The viewer in question.</param>
    /// <returns>
    ///     A value between 1 and 0 that indicates the overall percentage of influence
    /// </returns>
    float CalculateScore(IUser viewer);

    /// <summary>
    ///     Calculates a score that's used to indicate the viewer's reputation score for a given
    ///     transaction.
    /// </summary>
    /// <param name="transaction">The transaction being calculated.</param>
    /// <returns>
    ///     A value between 1 and 0 that indicates the percentage of influence a viewer had on a given
    ///     transaction.
    /// </returns>
    float CalculateScore(ITransaction transaction);
}
