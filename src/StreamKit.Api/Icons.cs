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

using UnityEngine;
using Verse;

namespace StreamKit.Mod;

[StaticConstructorOnStartup]
public static class Icons
{
    public static readonly Texture2D Bug = ContentFinder<Texture2D>.Get("Icons/Bug");
    public static readonly Texture2D Dove = ContentFinder<Texture2D>.Get("Icons/Dove");
    public static readonly Texture2D Store = ContentFinder<Texture2D>.Get("Icons/Store");
    public static readonly Texture2D People = ContentFinder<Texture2D>.Get("Icons/People");
    public static readonly Texture2D Twitch = ContentFinder<Texture2D>.Get("Icons/Twitch");
    public static readonly Texture2D ArrowLeft = ContentFinder<Texture2D>.Get("Icons/ArrowLeft");
    public static readonly Texture2D PiggyBank = ContentFinder<Texture2D>.Get("Icons/PiggyBank");
    public static readonly Texture2D ArrowRight = ContentFinder<Texture2D>.Get("Icons/ArrowRight");
    public static readonly Texture2D PaperPlane = ContentFinder<Texture2D>.Get("Icons/PaperPlane");
    public static readonly Texture2D SquarePollVertical = ContentFinder<Texture2D>.Get("Icons/SquarePollVertical");
}
