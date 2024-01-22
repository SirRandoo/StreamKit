using FluentResults;

namespace StreamKit.Api;

/// <summary>
///     An error used to indicate that a given path was empty, null, or contained invalid path
///     characters.
/// </summary>
/// <seealso cref="System.IO.Path.InvalidPathChars" />
public class InvalidPathError : Error
{
    private const string DefaultErrorMessage = "The path specified is empty, null, or otherwise invalid.";

    public InvalidPathError(string path) : base(DefaultErrorMessage)
    {
        Path = path;
    }

    public InvalidPathError(string path, IError causedBy) : base(DefaultErrorMessage, causedBy)
    {
        Path = path;
    }

    /// <summary>
    ///     The invalid path that was passed to a method.
    /// </summary>
    public string Path { get; }
}