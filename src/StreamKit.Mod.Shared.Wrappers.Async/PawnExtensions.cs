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
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace StreamKit.Mod.Shared.Wrappers.Async;

/// <summary>
///     A set of wrappers around the synchronous, unsafe methods within
///     the <see cref="Pawn" /> class.
/// </summary>
/// <remarks>
///     Usage of the extensions provided by this class are to be used
///     with care as RimWorld was not designed with asynchronous code in
///     mind.
/// </remarks>
public static class PawnExtensions
{
    /// <inheritdoc cref="ParentRelationUtility.SetFather" />
    public static async Task SetFatherAsync(this Pawn pawn, Pawn father)
    {
        await MainThreadExtensions.OnMainAsync(ParentRelationUtility.SetFather, pawn, father);
    }

    /// <inheritdoc cref="ParentRelationUtility.SetMother" />
    public static async Task SetMotherAsync(this Pawn pawn, Pawn mother)
    {
        await MainThreadExtensions.OnMainAsync(ParentRelationUtility.SetMother, pawn, mother);
    }

    /// <inheritdoc cref="Pawn.SetFactionDirect" />
    public static async Task SetFactionDirectAsync(this Pawn pawn, Faction faction)
    {
        await MainThreadExtensions.OnMainAsync(pawn.SetFactionDirect, faction);
    }

    /// <inheritdoc cref="Pawn.SetFaction" />
    public static async Task SetFactionAsync(this Pawn pawn, Faction faction, Pawn? recruiter = null)
    {
        await MainThreadExtensions.OnMainAsync(pawn.SetFaction, faction, recruiter);
    }

    /// <inheritdoc cref="Pawn.Kill" />
    public static async Task KillAsync(this Pawn pawn, DamageInfo? info, Hediff? culprit = null)
    {
        await MainThreadExtensions.OnMainAsync(pawn.Kill, info, culprit);
    }

    /// <inheritdoc cref="PawnRelationUtility.GetRelations" />
    public static async ValueTask<List<PawnRelationDef>> GetRelationsAsync(this Pawn pawn, Pawn other)
    {
        return await MainThreadExtensions.OnMainAsync(GetRelations, pawn, other);

        List<PawnRelationDef> GetRelations(Pawn subject, Pawn target) => [..subject.GetRelations(target)];
    }
}
