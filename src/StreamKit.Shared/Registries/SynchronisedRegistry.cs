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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using StreamKit.Shared.Interfaces;

namespace StreamKit.Shared.Registries;

/// <summary>
///     A registry that uses eventual consistency to synchronize its contents. This implementation
///     allows for concurrent modifications and processes changes in a background task.
/// </summary>
/// <typeparam name="T">
///     The type of the class being represented within the registry. Must implement
///     <see cref="IIdentifiable" /> to ensure each object has a unique identifier.
/// </typeparam>
public class SynchronisedRegistry<T> : IRegistry<T> where T : class, IIdentifiable
{
    private const int BatchSizeThreshold = 10; // Threshold for batch processing
    private const int MinimumDelayMs = 50; // Minimum delay between processing
    private const int MaximumDelayMs = 200; // Maximum delay between processing
    private readonly ConcurrentDictionary<string, T> _allRegistrantsKeyed = new();
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly ConcurrentQueue<RegistryChangeEvent> _pendingChanges = new();
    private int _pendingChangeCount;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SynchronisedRegistry{T}" /> class. Starts a
    ///     background task to process pending changes to the registry.
    /// </summary>
    public SynchronisedRegistry()
    {
        Task.Factory.StartNew(() => ProcessChanges(_cancellationTokenSource.Token), TaskCreationOptions.LongRunning);
    }

    /// <returns>
    ///     An immutable list of all currently registered objects in the registry. This provides a
    ///     snapshot of the registry's contents at the time of the call.
    /// </returns>
    public IReadOnlyList<T> AllRegistrants => ImmutableList.CreateRange(_allRegistrantsKeyed.Values);

    /// <summary>
    ///     Registers a new object in the registry by enqueuing a registration change event. The
    ///     actual registration will be processed asynchronously.
    /// </summary>
    /// <param name="obj">The object to register.</param>
    /// <returns>
    ///     <see langword="true" /> to indicate that the registration request has been enqueued;
    ///     <see langword="false" /> is not returned as this implementation always enqueues successfully.
    /// </returns>
    public bool Register([DisallowNull] T obj)
    {
        _pendingChanges.Enqueue(new RegistryChangeEvent(obj.Id, obj, true));
        Interlocked.Increment(ref _pendingChangeCount);

        return true;
    }

    /// <summary>
    ///     Unregisters an existing object from the registry by enqueuing an unregistration change
    ///     event. The actual unregistration will be processed asynchronously.
    /// </summary>
    /// <param name="obj">The object to unregister.</param>
    /// <returns>
    ///     <see langword="true" /> to indicate that the unregistration request has been enqueued;
    ///     <see langword="false" /> is not returned as this implementation always enqueues successfully.
    /// </returns>
    public bool Unregister([DisallowNull] T obj)
    {
        _pendingChanges.Enqueue(new RegistryChangeEvent(obj.Id, obj, false));
        Interlocked.Increment(ref _pendingChangeCount);

        return true;
    }

    /// <summary>
    ///     Retrieves an object from the registry using its unique identifier. Returns
    ///     <see langword="null" /> if the object does not exist.
    /// </summary>
    /// <param name="id">The unique identifier of the object to retrieve.</param>
    /// <returns>
    ///     The object associated with the specified identifier, or <see langword="null" /> if not
    ///     found.
    /// </returns>
    public T? Get(string id) => _allRegistrantsKeyed.TryGetValue(id, out T? value) ? value : default;

    /// <summary>
    ///     Creates an instance of the <see cref="SynchronisedRegistry{T}" /> and initializes it with
    ///     the specified registrants. This method ensures that all provided registrants are registered in
    ///     the new instance.
    /// </summary>
    /// <param name="registrants">A read-only list of registrants to initialize the registry with.</param>
    /// <returns>A new instance of <see cref="SynchronisedRegistry{T}" /> with the provided registrants.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if any of the provided registrants have
    ///     duplicate identifiers.
    /// </exception>
    public static SynchronisedRegistry<T> CreateInstance(IReadOnlyList<T> registrants)
    {
        var registry = new SynchronisedRegistry<T>();

        foreach (T? registrant in registrants)
        {
            if (!registry._allRegistrantsKeyed.TryAdd(registrant.Id, registrant))
            {
                throw new InvalidOperationException($"An entry with the id '{registrant.Id}' already exists.");
            }

            // Enqueue the registration event to process it in the background.
            registry._pendingChanges.Enqueue(new RegistryChangeEvent(registrant.Id, registrant, true));
        }

        return registry;
    }

    /// <summary>
    ///     Processes pending changes in the registry in a background task. This method runs
    ///     continuously, checking for changes to apply and managing the timing of processing.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to signal when processing should stop.</param>
    private async Task ProcessChanges(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (_pendingChangeCount > 0)
            {
                var changesToProcess = new List<RegistryChangeEvent>();

                while (_pendingChanges.TryDequeue(out RegistryChangeEvent change) && changesToProcess.Count < BatchSizeThreshold)
                {
                    changesToProcess.Add(change);
                }

                foreach (RegistryChangeEvent change in changesToProcess)
                {
                    if (change.IsRegistering)
                    {
                        _allRegistrantsKeyed.TryAdd(change.Id, change.Registrant);
                    }
                    else
                    {
                        _allRegistrantsKeyed.TryRemove(change.Id, out T _);
                    }
                }

                Interlocked.Add(ref _pendingChangeCount, -changesToProcess.Count);
            }

            int delay = Math.Min(Math.Max(MinimumDelayMs + _pendingChangeCount * 5, MinimumDelayMs), MaximumDelayMs);
            await Task.Delay(delay, cancellationToken);
        }
    }

    /// <summary>
    ///     Represents a change event in the registry, which includes the identifier of the object,
    ///     the object itself, and a flag indicating whether the operation is a registration or
    ///     unregistration.
    /// </summary>
    /// <param name="Id">The unique identifier of the registrant.</param>
    /// <param name="Registrant">The object being registered or unregistered.</param>
    /// <param name="IsRegistering">
    ///     A flag indicating whether the operation is for registration (true) or
    ///     unregistration (false).
    /// </param>
    private record struct RegistryChangeEvent(string Id, T Registrant, bool IsRegistering);
}
