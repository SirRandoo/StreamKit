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

using System;
using System.Threading;
using System.Threading.Tasks;
using Verse;

namespace StreamKit.Mod.Shared.Wrappers.Async;

[StaticConstructorOnStartup]
public static class MainThreadExtensions
{
    private static readonly TaskScheduler MainThreadScheduler = TaskScheduler.FromCurrentSynchronizationContext();

    private static readonly TaskFactory MainThreadFactory = new(
        CancellationToken.None,
        TaskCreationOptions.DenyChildAttach,
        TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.ExecuteSynchronously,
        MainThreadScheduler
    );

    public static async ValueTask<T> OnMainAsync<T>(this Func<T> func) =>
        await new ValueTask<T>(MainThreadFactory.StartNew(func, TaskCreationOptions.RunContinuationsAsynchronously));

    public static async ValueTask<T> OnMainAsync<T, T1>(this Func<T1, T> func, T1 arg1)
    {
        return await MainThreadFactory.StartNew(() => func(arg1), TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public static async ValueTask<T> OnMainAsync<T, T1, T2>(this Func<T1, T2, T> func, T1 arg1, T2 arg2)
    {
        return await new ValueTask<T>(MainThreadFactory.StartNew(() => func(arg1, arg2), TaskCreationOptions.RunContinuationsAsynchronously));
    }

    public static async ValueTask<T> OnMainAsync<T, T1, T2, T3>(this Func<T1, T2, T3, T> func, T1 arg1, T2 arg2, T3 arg3)
    {
        return await new ValueTask<T>(MainThreadFactory.StartNew(() => func(arg1, arg2, arg3), TaskCreationOptions.RunContinuationsAsynchronously));
    }

    public static async ValueTask<T> OnMainAsync<T, T1, T2, T3, T4>(this Func<T1, T2, T3, T4, T> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return await new ValueTask<T>(MainThreadFactory.StartNew(() => func(arg1, arg2, arg3, arg4), TaskCreationOptions.RunContinuationsAsynchronously));
    }

    public static async Task OnMainAsync(this Action func)
    {
        await MainThreadFactory.StartNew(func, TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public static async Task OnMainAsync<T>(this Action<T> func, T arg1)
    {
        await MainThreadFactory.StartNew(() => func(arg1), TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public static async Task OnMainAsync<T1, T2>(this Action<T1, T2> func, T1 arg1, T2 arg2)
    {
        await MainThreadFactory.StartNew(() => func(arg1, arg2), TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public static async Task OnMainAsync<T1, T2, T3>(this Action<T1, T2, T3> func, T1 arg1, T2 arg2, T3 arg3)
    {
        await MainThreadFactory.StartNew(() => func(arg1, arg2, arg3), TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public static async Task OnMainAsync<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        await MainThreadFactory.StartNew(() => func(arg1, arg2, arg3, arg4), TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public static async Task OnMainAsync(this Task task)
    {
        task.Start(MainThreadScheduler);

        await task;
    }

    public static async Task<T> OnMainAsync<T>(this Task<T> task)
    {
        task.Start(MainThreadScheduler);

        return await task;
    }
}
