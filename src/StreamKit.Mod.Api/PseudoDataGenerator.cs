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

#if DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using StreamKit.Common.Data;
using StreamKit.Common.Data.Abstractions;

namespace StreamKit.Mod.Api;

/// <summary>
///     A class for generating random mod data.
/// </summary>
public sealed class PseudoDataGenerator
{
    private static readonly Lazy<PseudoDataGenerator> InternalInstance = new(() => new PseudoDataGenerator());

    private static readonly string[] ProductTypes = ["ITEM", "EVENT", "PAWN", "TRAIT", "BACKSTORY"];

    private static readonly IRegistry<IPlatform> Platforms = new FrozenRegistry<IPlatform>(
        [
            new Platform("platforms.debug.twitch", []) { Name = "Twitch (DEBUG)" }, new Platform("platforms.debug.trovo", []) { Name = "Trovo (DEBUG)" },
            new Platform("platforms.debug.kick", []) { Name = "Kick (DEBUG)" }
        ]
    );

    private static readonly Internet Internet = new();
    private static readonly Randomizer Randomizer = new();
    private static readonly Date Date = new();
    private static readonly Lorem Lorem = new();

    private PseudoDataGenerator()
    {
    }

    public static PseudoDataGenerator Instance => InternalInstance.Value;

    /// <summary>
    ///     Returns a randomly generated a ledger seeded with random data.
    /// </summary>
    /// <param name="viewerCount">The total amount of viewers to populate the ledger with.</param>
    /// <param name="transactionCount">The length of each viewer's transaction history.</param>
    public ILedger GeneratePseudoLedger(int viewerCount, int transactionCount) => new PseudoLedger(Guid.NewGuid().ToString())
    {
        Name = Lorem.Sentence(1, 1), Data = new MutableRegistry<IUser>(GeneratePseudoUsers(viewerCount, transactionCount))
    };

    /// <summary>
    ///     Returns a collection of randomly generated transactions.
    /// </summary>
    /// <param name="count">The total amount of transactions to generate.</param>
    public ITransaction[] GeneratePseudoTransactions(int count)
    {
        return Enumerable.Range(0, count)
           .AsParallel()
           .Select(
                _ => (ITransaction)new PseudoTransaction(Guid.NewGuid().ToString(), Lorem.Sentence(1, 1), Randomizer.ArrayElement(ProductTypes), Date.Past())
                {
                    Amount = Randomizer.Int(1), Morality = Randomizer.Enum<Morality>(), Refunded = Randomizer.Bool(0.1f)
                }
            )
           .ToArray();
    }

    public IUser[] GeneratePseudoUsers(int count, int transactionCount = 50)
    {
        return Enumerable.Range(0, count)
           .Select(
                _ => (IUser)new PseudoUser(Guid.NewGuid().ToString(), Randomizer.CollectionItem(Platforms.AllRegistrants).Id)
                {
                    Name = Internet.UserName(),
                    Privileges = GetRandomPrivileges(),
                    Karma = Randomizer.Short(),
                    Points = Randomizer.Int(),
                    LastSeen = Date.Past(),
                    Transactions = [..GeneratePseudoTransactions(transactionCount)]
                }
            )
           .ToArray();
    }

    public IMessage[] GeneratePseudoMessages(int count)
    {
        return Enumerable.Range(0, count)
           .Select(_ => (IMessage)new PseudoMessage(Guid.NewGuid().ToString(), Lorem.Sentence(range: 10)) { Author = GeneratePseudoUsers(1)[0] })
           .ToArray();
    }

    private UserPrivileges GetRandomPrivileges()
    {
        return Randomizer.EnumValues<UserPrivileges>().Aggregate(UserPrivileges.None, (current, privilege) => current | privilege);
    }

    private sealed record PseudoUser(string Id, string Platform) : IUser
    {
        /// <inheritdoc />
        public required string Name { get; set; }

        /// <inheritdoc />
        public long Points { get; set; }

        /// <inheritdoc />
        public short Karma { get; set; }

        /// <inheritdoc />
        public DateTime LastSeen { get; set; } = DateTime.UtcNow;

        /// <inheritdoc />
        public UserPrivileges Privileges { get; set; } = UserPrivileges.None;

        /// <inheritdoc />
        public List<ITransaction> Transactions { get; init; } = [];
    }

    private sealed record PseudoMessage(string Id, string Content) : IMessage
    {
        /// <inheritdoc />
        public required IUser Author { get; init; }

        /// <inheritdoc />
        public DateTime ReceivedAt { get; init; } = DateTime.UtcNow;
    }

    private sealed record PseudoTransaction(string Id, string Name, string ProductId, DateTime OccurredAt) : ITransaction
    {
        /// <inheritdoc />
        public string Id { get; init; } = Id;

        /// <inheritdoc />
        public string Name { get; set; } = Name;

        /// <inheritdoc />
        public string ProductId { get; set; } = ProductId;

        /// <inheritdoc />
        public Morality Morality { get; set; }

        /// <inheritdoc />
        public DateTime OccurredAt { get; set; } = OccurredAt;

        /// <inheritdoc />
        public long Amount { get; set; }

        /// <inheritdoc />
        public bool Refunded { get; set; }
    }

    private sealed record PseudoLedger(string Id) : ILedger
    {
        /// <inheritdoc />
        public required string Name { get; set; }

        /// <inheritdoc />
        public required IRegistry<IUser> Data { get; init; }

        /// <inheritdoc />
        public DateTime LastModified { get; set; }
    }
}

#endif
