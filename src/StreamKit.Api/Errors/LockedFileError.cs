using FluentResults;

namespace StreamKit.Api;

/// <summary>
///     An error used to indicate that a file isn't available for write operations.
/// </summary>
public class LockedFileError : Error
{
    private const string DefaultErrorMessage = "The file specified is locked from further modifications.";

    public LockedFileError(string path) : base(DefaultErrorMessage)
    {
        Path = path;
    }

    public LockedFileError(string path, IError causedBy) : base(DefaultErrorMessage, causedBy)
    {
        Path = path;
    }

    /// <summary>
    ///     The path to the file whose write access is blocked.
    /// </summary>
    public string Path { get; }
}