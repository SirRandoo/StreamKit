using StreamKit.Common.Data.Abstractions;
using StreamKit.Mod.Api;
using UnityEngine;
using Verse;

namespace StreamKit.Mod.Shared.Core.Windows;

// TODO: This class requires a system for managing viewers who want pawns.
// TODO: Map out the user data this class requires in order to complete queue tasks.
// TODO: This class requires special wrapper classes for converting pawns into "IAssignableEntity" instances.

// FIXME: This class stores a "read-only" copy of data that isn't periodically updated.

public class PawnQueueWindow : Window
{
    private IUser[] _users = [];

    private PawnQueueWindow()
    {
    }

    /// <inheritdoc />
    public override void DoWindowContents(Rect inRect)
    {
        var previewRegion = new Rect(0f, 0f, inRect.width * 0.2f, inRect.width * 0.2f);
    }

    public static PawnQueueWindow CreateDefaultInstance() => new();

#if DEBUG
    /// <summary>
    ///     Creates a debug instance of a pawn queue window preloaded with generated data.
    /// </summary>
    public static PawnQueueWindow CreateDebugInstance() => new() { _users = PseudoDataGenerator.Instance.GeneratePseudoUsers(50) };
#endif
}
