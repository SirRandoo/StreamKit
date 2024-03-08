using System.Xml.Serialization;

namespace StreamKit.Bootstrap.Shared.Core;

/// <summary>
///     Represents a runtime representation of a given directory tree.
/// </summary>
public class Corpus
{
    /// <summary>
    ///     A collection of bundles which are a representation of a
    ///     directory.
    /// </summary>
    [XmlArrayItem(typeof(ResourceBundle))]
    public ResourceBundle[] Resources { get; init; } = null!;
}

/// <summary>
///     Represents a directory on disk.
/// </summary>
public class ResourceBundle
{
    /// <summary>
    ///     The path of the directory on disk.
    /// </summary>
    [XmlAttribute]
    public string Root { get; init; } = null!;

    /// <summary>
    ///     Whether the current version of RimWorld should be appended to the
    ///     <see cref="Root"/> path.<br/>
    ///     <br/>
    ///     The version used depends on the first directory found with the
    ///     associated version. The first version used is the version with
    ///     the build number included, while the second version checked is
    ///     the version without the build number.
    /// </summary>
    [XmlAttribute]
    public bool Versioned { get; init; }

    /// <summary>
    ///     A collection of resources found in the directory.
    /// </summary>
    [XmlElement("Resource", typeof(Resource))]
    public Resource[] Resources { get; set; } = null!;
}

/// <summary>
///     Represents a file on disk.
/// </summary>
public class Resource
{
    /// <summary>
    ///     The type of file being represented.
    /// </summary>
    [XmlAttribute]
    public ResourceType Type { get; init; }

    /// <summary>
    ///     The name of the file (without an extension).
    /// </summary>
    [XmlAttribute]
    public string Name { get; init; } = null!;

    /// <summary>
    ///     The path to the directory housing the file.
    /// </summary>
    [XmlAttribute]
    public string Root { get; init; } = null!;
}

/// <summary>
///     The various types of resources that can be loaded by the mod's
///     bootloader.
/// </summary>
public enum ResourceType
{
    /// <summary>
    ///     Represents a dll that can only be loaded by the operating system.
    /// </summary>
    [XmlEnum(Name = "Dll")]
    Dll,

    /// <summary>
    ///     Represents a dll that can be loaded by the C# runtime without
    ///     special platform handling.
    /// </summary>
    [XmlEnum(Name = "Assembly")]
    Assembly,

    /// <summary>
    ///     Represents a dll that can't be loaded by the C# runtime without
    ///     special runtime handling.
    /// </summary>
    [XmlEnum(Name = "NetStandardAssembly")]
    NetStandardAssembly
}
