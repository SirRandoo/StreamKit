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
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using StreamKit.Data.Abstractions;

namespace StreamKit.Api;

/// <summary>
///     A static class dedicated to generating random information used throughout the mod.
/// </summary>
public static class PseudoDataGenerator
{
    private static readonly Morality[] Moralities = MoralityExtensions.GetValues();
    private static readonly ProductType[] ProductTypes = ProductTypeExtensions.GetValues();
    private static readonly UserPrivileges[] Privileges = UserPrivilegesExtensions.GetValues();

    private static readonly PlatformRegistry Platforms = PlatformRegistry.CreateDebugInstance();
    private static readonly Internet Internet = new();
    private static readonly Randomizer Randomizer = new();
    private static readonly Date Date = new();
    private static readonly Lorem Lorem = new();

    /// <summary>
    ///     Returns a randomly generated a ledger seeded with random data.
    /// </summary>
    /// <param name="viewerCount">The total amount of viewers to populate the ledger with.</param>
    /// <param name="transactionCount">The length of each viewer's transaction history.</param>
    public static ILedger GeneratePseudoLedger(int viewerCount, int transactionCount) => new PseudoLedger(Guid.NewGuid().ToString())
    {
        Name = Lorem.Sentence(1, 1), Data = new MutableRegistry<IViewerData>(GeneratePseudoViewerData(viewerCount, transactionCount))
    };

    /// <summary>
    ///     Returns a readonly list of randomly generated viewer information.
    /// </summary>
    /// <param name="count">The total amount of viewers to generate.</param>
    /// <param name="transactionCounts">The length of each viewer's transaction history.</param>
    public static ImmutableList<IViewerData> GeneratePseudoViewerData(int count, int transactionCounts)
    {
        return Enumerable.Range(0, count)
           .AsParallel()
           .Select(
                _ =>
                {
                    return (IViewerData)new PseudoViewerData(Guid.NewGuid().ToString())
                    {
                        Name = Internet.UserName(),
                        Platform = Platforms.RandomPlatform.Name,
                        Points = Randomizer.Int(),
                        Karma = Randomizer.Short(),
                        LastSeen = Date.Past(),
                        Transactions = GeneratePseudoTransactions(transactionCounts).ToList()
                    };
                }
            )
           .ToImmutableList();
    }

    /// <summary>
    ///     Returns a collection of randomly generated transactions.
    /// </summary>
    /// <param name="count">The total amount of transactions to generate.</param>
    public static ITransaction[] GeneratePseudoTransactions(int count)
    {
        return Enumerable.Range(0, count)
           .AsParallel()
           .Select(
                _ => (ITransaction)new PseudoTransaction(Guid.NewGuid().ToString(), Lorem.Sentence(1, 1), Date.Past())
                {
                    Type = Randomizer.Enum<ProductType>(), Price = Randomizer.Int(1), Morality = Randomizer.Enum<Morality>(), Refunded = Randomizer.Bool(0.1f)
                }
            )
           .ToArray();
    }

    public static IUser[] GeneratePseudoUsers(int count)
    {
        return Enumerable.Range(0, count)
           .Select(
                _ => (IUser)new PseudoUser(Guid.NewGuid().ToString(), Platforms.RandomPlatform)
                {
                    Name = Internet.UserName(), Data = GeneratePseudoViewerData(1, 0)[0], Privileges = GetRandomPrivileges()
                }
            )
           .ToArray();
    }

    public static IMessage[] GeneratePseudoMessages(int count)
    {
        return Enumerable.Range(0, count)
           .Select(_ => (IMessage)new PseudoMessage(Guid.NewGuid().ToString(), Lorem.Sentence(range: 10)) { Author = GeneratePseudoUsers(1)[0] })
           .ToArray();
    }

    private static UserPrivileges GetRandomPrivileges()
    {
        return Randomizer.EnumValues<UserPrivileges>().Aggregate(UserPrivileges.None, (current, privilege) => current | privilege);
    }

    private sealed record PseudoUser(string Id, IPlatform Platform) : IUser
    {
        /// <inheritdoc />
        public required string Name { get; set; }

        /// <inheritdoc />
        public required IViewerData Data { get; set; }

        /// <inheritdoc />
        public DateTime LastSeen { get; set; } = DateTime.UtcNow;

        /// <inheritdoc />
        public UserPrivileges Privileges { get; set; } = UserPrivileges.None;
    }

    private sealed record PseudoMessage(string Id, string Content) : IMessage
    {
        /// <inheritdoc />
        public required IUser Author { get; set; }

        /// <inheritdoc />
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }

    private sealed record PseudoTransaction(string Id, string Name, DateTime PurchasedAt) : ITransaction
    {
        /// <inheritdoc />
        public string Id { get; } = Id;

        /// <inheritdoc />
        public string Name { get; set; } = Name;

        /// <inheritdoc />
        public DateTime PurchasedAt { get; } = PurchasedAt;

        /// <inheritdoc />
        public ProductType Type { get; set; }

        /// <inheritdoc />
        public long Price { get; set; }

        /// <inheritdoc />
        public Morality Morality { get; set; }

        /// <inheritdoc />
        public bool Refunded { get; set; }
    }

    private sealed record PseudoViewerData(string Id) : IViewerData
    {
        /// <inheritdoc />
        public required string Name { get; set; }

        /// <inheritdoc />
        public required string Platform { get; set; }

        /// <inheritdoc />
        public long Points { get; set; }

        /// <inheritdoc />
        public short Karma { get; set; }

        /// <inheritdoc />
        public DateTime LastSeen { get; set; }

        /// <inheritdoc />
        public List<ITransaction> Transactions { get; set; } = [];
    }

    private sealed record PseudoLedger(string Id) : ILedger
    {
        /// <inheritdoc />
        public required string Name { get; set; }

        /// <inheritdoc />
        public required IRegistry<IViewerData> Data { get; init; }

        /// <inheritdoc />
        public DateTime LastModified { get; set; }
    }
}
