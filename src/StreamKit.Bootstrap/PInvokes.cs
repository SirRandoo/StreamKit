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

using System;
using System.Runtime.InteropServices;

namespace StreamKit.Bootstrap
{
    internal static class PInvokesWindows
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)] public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32", CharSet = CharSet.Unicode)] public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);
        [DllImport("kernel32")] public static extern IntPtr GetCurrentProcess();
        [DllImport("kernel32")] internal static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, Protections flNewProtect, out Protections lpflOldProtect);
        [DllImport("kernel32")] internal static extern bool FlushInstructionCache(IntPtr hProcess, IntPtr lpBaseAddress, UIntPtr dwSize);

        /// <summary>A bit-field of flags for protections</summary>
        [Flags]
        internal enum Protections
        {
            /// <summary>No access</summary>
            PageNoAccess = 0x01,
            /// <summary>Read only</summary>
            PageReadonly = 0x02,
            /// <summary>Read write</summary>
            PageReadWrite = 0x04,
            /// <summary>Write copy</summary>
            PageWriteCopy = 0x08,
            /// <summary>No access</summary>
            PageExecute = 0x10,
            /// <summary>Execute read</summary>
            PageExecuteRead = 0x20,
            /// <summary>Execute read write</summary>
            PageExecuteReadWrite = 0x40,
            /// <summary>Execute write copy</summary>
            PageExecuteWriteCopy = 0x80,
            /// <summary>guard</summary>
            PageGuard = 0x100,
            /// <summary>No cache</summary>
            PageNoCache = 0x200,
            /// <summary>Write combine</summary>
            PageWriteCombine = 0x400
        }
    }

    internal static class PInvokesLinux
    {
        private const string LibDL = "libdl.so";
        private const string LibC = "libc.so";

        public const int ScPageSize = 30;


        [DllImport(LibDL, CharSet = CharSet.Unicode)] public static extern IntPtr dlopen(string filename, int flags);
        [DllImport(LibDL, CharSet = CharSet.Unicode)] public static extern IntPtr dlsym(IntPtr handle, string symbol);
        [DllImport(LibDL)] public static extern int dlclose(IntPtr handle);
        [DllImport(LibC)] public static extern int mprotect(IntPtr addr, UIntPtr len, Prots prots);
        [DllImport(LibC)] public static extern IntPtr sysconf(int name);

        [Flags]
        internal enum Prots
        {
            /// <summary>page can be read</summary>
            ProtRead = 0x1,
            /// <summary>page can be written</summary>
            ProtWrite = 0x2,
            /// <summary>page can be executed</summary>
            ProtExec = 0x4,
            /// <summary>page may be used for atomic ops</summary>
            ProtSem = 0x8,
            /// <summary>page can not be accessed</summary>
            ProtNone = 0x0,
            /// <summary>extend change to start of growsdown vma</summary>
            ProtGrowsDown = 0x01000000,
            /// <summary>extend change to end of growsup vma</summary>
            ProtGrowsUp = 0x02000000
        }
    }

    internal static class PInvokesOsx
    {
        [DllImport("libdl.dylib", CharSet = CharSet.Unicode)] public static extern IntPtr dlopen(string filename, int flags);
        [DllImport("libdl.dylib", CharSet = CharSet.Unicode)] public static extern IntPtr dlsym(IntPtr handle, string symbol);
        [DllImport("libdl.dylib")] public static extern int dlclose(IntPtr handle);
    }

    public enum PosixDlOpen { Lazy = 0x00001, Now = 0x00002, LazyGlobal = 0x00100 | Lazy, NowGlobal = 0x00100 | Now }
}
