using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Verse;

namespace StreamKit.Mod.Api.Async;

[PublicAPI]
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

    public static async Task<T> OnMainAsync<T>(this Func<T> func) => await MainThreadFactory.StartNew(func, TaskCreationOptions.RunContinuationsAsynchronously);

    public static async Task<T> OnMainAsync<T, T1>(this Func<T1, T> func, T1 arg1)
    {
        return await MainThreadFactory.StartNew(() => func(arg1), TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public static async Task<T> OnMainAsync<T, T1, T2>(this Func<T1, T2, T> func, T1 arg1, T2 arg2)
    {
        return await MainThreadFactory.StartNew(() => func(arg1, arg2), TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public static async Task<T> OnMainAsync<T, T1, T2, T3>(this Func<T1, T2, T3, T> func, T1 arg1, T2 arg2, T3 arg3)
    {
        return await MainThreadFactory.StartNew(() => func(arg1, arg2, arg3), TaskCreationOptions.RunContinuationsAsynchronously);
    }

    public static async Task<T> OnMainAsync<T, T1, T2, T3, T4>(this Func<T1, T2, T3, T4, T> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return await MainThreadFactory.StartNew(() => func(arg1, arg2, arg3, arg4), TaskCreationOptions.RunContinuationsAsynchronously);
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
