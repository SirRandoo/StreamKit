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
using JetBrains.Annotations;
using StreamKit.Shared;
using StreamKit.Shared.Interfaces;
using StreamKit.Shared.Registries;

namespace StreamKit.Mod.Api;

/// <summary>A class for generating random mod data.</summary>
[PublicAPI]
public sealed class PseudoDataGenerator
{
    private static readonly Lazy<PseudoDataGenerator> InternalInstance = new(() => new PseudoDataGenerator());

    private static readonly string[] ProductTypes = ["ITEM", "EVENT", "PAWN", "TRAIT", "BACKSTORY"];

    public static readonly IReadOnlyRegistry<IPlatform> Platforms = FrozenRegistry<IPlatform>.CreateInstance(
        [
            new PseudoPlatform("streamkit.platforms.twitch.debug", "Twitch (DEBUG)"),
            new PseudoPlatform("streamkit.platforms.trovo.debug", "Trovo (DEBUG)"),
            new PseudoPlatform("streamkit.platforms.kick.debug", "Kick (DEBUG)")
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

    /// <summary>Returns a randomly generated a ledger seeded with random data.</summary>
    /// <param name="viewerCount">The total amount of viewers to populate the ledger with.</param>
    /// <param name="transactionCount">The length of each viewer's transaction history.</param>
    public ILedger GeneratePseudoLedger(int viewerCount, int transactionCount) => new PseudoLedger(Guid.NewGuid().ToString())
    {
        Name = Lorem.Sentence(1, 1), Data = MutableRegistry<IUser>.CreateInstance(GeneratePseudoUsers(viewerCount, transactionCount))
    };

    /// <summary>Returns a collection of randomly generated transactions.</summary>
    /// <param name="count">The total amount of transactions to generate.</param>
    public ITransaction[] GeneratePseudoTransactions(int count)
    {
        return Enumerable.Range(0, count)
           .AsParallel()
           .Select(
                ITransaction (_) => new PseudoTransaction(Guid.NewGuid().ToString(), Lorem.Sentence(1, 1), Randomizer.ArrayElement(ProductTypes), Date.Past())
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
                IUser (_) =>
                {
                    int randomPlatformIndex = Randomizer.Number(Platforms.AllRegistrants.Count - 1);
                    IPlatform randomPlatform = Platforms.AllRegistrants[randomPlatformIndex];

                    return new PseudoUser(Guid.NewGuid().ToString(), randomPlatform.Id)
                    {
                        Name = Internet.UserName(),
                        Roles = GetRandomPrivileges(),
                        Karma = Randomizer.Short(),
                        Points = Randomizer.Int(),
                        LastSeen = Date.Past(),
                        Transactions = [..GeneratePseudoTransactions(transactionCount)]
                    };
                }
            )
           .ToArray();
    }

    public IMessage[] GeneratePseudoMessages(int count)
    {
        return Enumerable.Range(0, count)
           .Select(IMessage (_) => new PseudoMessage(Guid.NewGuid().ToString(), Lorem.Sentence(range: 10)) { Author = GeneratePseudoUsers(1)[0] })
           .ToArray();
    }

    private UserRoles GetRandomPrivileges()
    {
        return Randomizer.EnumValues<UserRoles>().Aggregate(UserRoles.None, (current, privilege) => current | privilege);
    }

    private sealed class PseudoUser(string id, string platform) : IUser
    {
        /// <inheritdoc />
        public string Id { get; set; } = id;

        /// <inheritdoc />
        public string Platform { get; set; } = platform;

        /// <inheritdoc />
        public required string Name { get; set; }

        /// <inheritdoc />
        public long Points { get; set; }

        /// <inheritdoc />
        public short Karma { get; set; }

        /// <inheritdoc />
        public DateTime LastSeen { get; set; } = DateTime.UtcNow;

        /// <inheritdoc />
        public UserRoles Roles { get; set; } = UserRoles.None;

        /// <inheritdoc />
        public List<ITransaction> Transactions { get; set; } = [];
    }

    private sealed class PseudoMessage(string id, string content) : IMessage
    {
        /// <inheritdoc />
        public string Id { get; set; } = id;

        /// <inheritdoc />
        public string Content { get; set; } = content;

        /// <inheritdoc />
        public required IUser Author { get; set; }

        /// <inheritdoc />
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }

    private sealed class PseudoTransaction(string id, string name, string productId, DateTime occurredAt) : ITransaction
    {
        /// <inheritdoc />
        public string Id { get; set; } = id;

        /// <inheritdoc />
        public string Name { get; set; } = name;

        /// <inheritdoc />
        public string ProductId { get; set; } = productId;

        /// <inheritdoc />
        public Morality Morality { get; set; }

        /// <inheritdoc />
        public DateTime OccurredAt { get; set; } = occurredAt;

        /// <inheritdoc />
        public long Amount { get; set; }

        /// <inheritdoc />
        public bool Refunded { get; set; }
    }

    private sealed class PseudoLedger(string id) : ILedger
    {
        /// <inheritdoc />
        public string Id { get; set; } = id;

        /// <inheritdoc />
        public required string Name { get; set; }

        /// <inheritdoc />
        public required IRegistry<IUser> Data { get; set; }

        /// <inheritdoc />
        public DateTime LastModified { get; set; }
    }

    private sealed class PseudoPlatform(string id, string name) : IPlatform
    {
        /// <inheritdoc />
        public string Id { get; set; } = id;

        /// <inheritdoc />
        public string Name { get; set; } = name;

        /// <inheritdoc />
        public byte[] IconData { get; set; } = [];

        /// <inheritdoc />
        public PlatformFeatures Features { get; } = PlatformFeatures.None;
    }
}

#endif
