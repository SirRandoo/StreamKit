using FluentResults;

namespace StreamKit.Api;

/// <summary>
///     An error used to indicate that a file wasn't found on disk.
/// </summary>
public class FileNotFoundError : Error
{
    private const string DefaultErrorMessage = "No file at the specified path could be found.";

    public FileNotFoundError(string path) : base(DefaultErrorMessage)
    {
        Path = path;
    }

    public FileNotFoundError(string path, IError causedBy) : base(DefaultErrorMessage, causedBy)
    {
        Path = path;
    }

    /// <summary>
    ///     The path to the non-existent file on disk.
    /// </summary>
    public string Path { get; }
}