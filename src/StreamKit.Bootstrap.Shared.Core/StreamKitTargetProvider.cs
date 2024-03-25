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
using System.IO;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using RimWorld.Logging.Api;
using Verse;

namespace StreamKit.Bootstrap.Shared.Core;

public class StreamKitTargetProvider : ITargetProvider
{
    public Target? Get(string packageId, LoggingConfiguration config)
    {
        if (!BootstrapMod.Instance.Content.PackageId.Equals(packageId, StringComparison.Ordinal))
        {
            return null;
        }

        var target = new FileTarget("StreamKit.File");
        target.Layout = "${time} [${level:uppercase=true}][Thread ${threadid}] (${logger}) :: ${message:withexception=true}";
        target.FileName = Path.Combine(GenFilePaths.SaveDataFolderPath, "StreamKit", "streamkit.log");
        target.DeleteOldFileOnStartup = true;

        return new AsyncTargetWrapper(target);
    }
}
