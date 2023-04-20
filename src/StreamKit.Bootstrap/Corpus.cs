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

using System.Xml.Serialization;

namespace StreamKit.Bootstrap
{
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
        public ResourceBundle[] Resources { get; set; }
    }

    /// <summary>
    ///     Represents a directory on disk.
    /// </summary>
    public class ResourceBundle
    {
        /// <summary>
        ///     The path of the directory on disk.
        /// </summary>
        [XmlAttribute] public string Root { get; set; }

        /// <summary>
        ///     Whether the current version of RimWorld should be appended to the
        ///     <see cref="Root"/> path.
        /// </summary>
        /// <remarks>
        ///     The version used depends on the first directory found with the
        ///     associated version. The first version used is the version with
        ///     the build number included, while the second version checked is
        ///     the version without the build number.
        /// </remarks>
        [XmlAttribute] public bool Versioned { get; set; }

        /// <summary>
        ///     A collection of resources found in the directory.
        /// </summary>
        [XmlArrayItem(nameof(Resource))]
        public Resource[] Resources { get; set; }
    }

    /// <summary>
    ///     Represents a file on disk.
    /// </summary>
    public class Resource
    {
        /// <summary>
        ///     The type of file being represented.
        /// </summary>
        [XmlAttribute] public ResourceType Type { get; set; }

        /// <summary>
        ///     The name of the file (without an extension).
        /// </summary>
        [XmlAttribute] public string Name { get; set; }

        /// <summary>
        ///     The path to the directory housing the file.
        /// </summary>
        [XmlAttribute] public string Root { get; set; }
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
        [XmlEnum("Dll")]
        Dll,

        /// <summary>
        ///     Represents a dll that can be loaded by the C# runtime without
        ///     special platform handling.
        /// </summary>
        [XmlEnum("Assembly")]
        Assembly
    }
}
