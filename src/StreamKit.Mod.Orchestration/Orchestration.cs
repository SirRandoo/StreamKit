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

using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using StreamKit.Mod.Orchestration.Logging;
using Verse;

namespace StreamKit.Mod.Orchestration;

[StaticConstructorOnStartup]
[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
internal sealed class Orchestration
{
    private static IHost _diHost = null!;

    /// <summary>Provides the synchronization context for the game's main thread.</summary>
    /// <remarks>
    ///     This field should always be initialized from the game's main thread, typically from a
    ///     mechanism within the game.
    /// </remarks>
    internal static readonly SynchronizationContext SynchronizationContext = SynchronizationContext.Current;

    static Orchestration()
    {
        Task.Factory.StartNew(PerformOrchestration, TaskCreationOptions.LongRunning);
    }

    private static async Task PerformOrchestration()
    {
        _diHost = Host.CreateDefaultBuilder().ConfigureLogging(LoggingServiceExtensions.ConfigureNLog).Build();

        await _diHost.RunAsync();
    }
}
