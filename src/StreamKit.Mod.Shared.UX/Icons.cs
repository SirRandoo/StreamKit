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

using UnityEngine;
using Verse;

namespace StreamKit.Mod.Shared.UX;

[StaticConstructorOnStartup]
public static class Icons
{
    public static readonly Texture2D Bug = ContentFinder<Texture2D>.Get("Icons/Bug");
    public static readonly Texture2D Code = ContentFinder<Texture2D>.Get("Icons/Code");
    public static readonly Texture2D Dove = ContentFinder<Texture2D>.Get("Icons/Dove");
    public static readonly Texture2D Plus = ContentFinder<Texture2D>.Get("Icons/Plus");
    public static readonly Texture2D Minus = ContentFinder<Texture2D>.Get("Icons/Minus");
    public static readonly Texture2D Store = ContentFinder<Texture2D>.Get("Icons/Store");
    public static readonly Texture2D People = ContentFinder<Texture2D>.Get("Icons/People");
    public static readonly Texture2D AngleUp = ContentFinder<Texture2D>.Get("Icons/AngleUp");
    public static readonly Texture2D Message = ContentFinder<Texture2D>.Get("Icons/Message");
    public static readonly Texture2D AngleDown = ContentFinder<Texture2D>.Get("Icons/AngleDown");
    public static readonly Texture2D ArrowLeft = ContentFinder<Texture2D>.Get("Icons/ArrowLeft");
    public static readonly Texture2D PiggyBank = ContentFinder<Texture2D>.Get("Icons/PiggyBank");
    public static readonly Texture2D AngleLeft = ContentFinder<Texture2D>.Get("Icons/AngleLeft");
    public static readonly Texture2D AnglesLeft = ContentFinder<Texture2D>.Get("Icons/AnglesLeft");
    public static readonly Texture2D AngleRight = ContentFinder<Texture2D>.Get("Icons/AngleRight");
    public static readonly Texture2D ArrowRight = ContentFinder<Texture2D>.Get("Icons/ArrowRight");
    public static readonly Texture2D PaperPlane = ContentFinder<Texture2D>.Get("Icons/PaperPlane");
    public static readonly Texture2D AnglesRight = ContentFinder<Texture2D>.Get("Icons/AnglesRight");
    public static readonly Texture2D MagnifyingGlass = ContentFinder<Texture2D>.Get("Icons/MagnifyingGlass");
    public static readonly Texture2D SquarePollVertical = ContentFinder<Texture2D>.Get("Icons/SquarePollVertical");
    public static readonly Texture2D TriangleExclamation = ContentFinder<Texture2D>.Get("Icons/TriangleExclamation");

    public static readonly Texture2D Square = ContentFinder<Texture2D>.Get("Icons/Square");
    public static readonly Texture2D Check = ContentFinder<Texture2D>.Get("Icons/Input/Check");
    public static readonly Texture2D XMark = ContentFinder<Texture2D>.Get("Icons/Input/XMark");
    public static readonly Texture2D Circle = ContentFinder<Texture2D>.Get("Icons/Input/Circle");
    public static readonly Texture2D CircleFilled = ContentFinder<Texture2D>.Get("Icons/Circle");
    public static readonly Texture2D SquareMinus = ContentFinder<Texture2D>.Get("Icons/SquareMinus");
    public static readonly Texture2D CircleDot = ContentFinder<Texture2D>.Get("Icons/Input/CircleDot");
    public static readonly Texture2D SquareFilled = ContentFinder<Texture2D>.Get("Icons/SquareFilled");

    public static readonly Texture2D Lua = ContentFinder<Texture2D>.Get("Icons/Languages/Lua");
    public static readonly Texture2D NodeJs = ContentFinder<Texture2D>.Get("Icons/Languages/NodeJS");

    public static readonly Texture2D Info = ContentFinder<Texture2D>.Get("Icons/Info");
    public static readonly Texture2D Eye = ContentFinder<Texture2D>.Get("Icons/Actions/Eye");
    public static readonly Texture2D Copy = ContentFinder<Texture2D>.Get("Icons/Actions/Copy");
    public static readonly Texture2D Dice = ContentFinder<Texture2D>.Get("Icons/Actions/Dice");
    public static readonly Texture2D Sort = ContentFinder<Texture2D>.Get("Icons/Actions/Sort");
    public static readonly Texture2D Paste = ContentFinder<Texture2D>.Get("Icons/Actions/Paste");
    public static readonly Texture2D Trash = ContentFinder<Texture2D>.Get("Icons/Actions/Trash");
    public static readonly Texture2D Filter = ContentFinder<Texture2D>.Get("Icons/Actions/Filter");
    public static readonly Texture2D Folder = ContentFinder<Texture2D>.Get("Icons/Actions/Folder");
    public static readonly Texture2D Rotate = ContentFinder<Texture2D>.Get("Icons/Actions/Rotate");
    public static readonly Texture2D SortUp = ContentFinder<Texture2D>.Get("Icons/Actions/SortUp");
    public static readonly Texture2D Bookmark = ContentFinder<Texture2D>.Get("Icons/Actions/Bookmark");
    public static readonly Texture2D EyeSlash = ContentFinder<Texture2D>.Get("Icons/Actions/EyeSlash");
    public static readonly Texture2D SortDown = ContentFinder<Texture2D>.Get("Icons/Actions/SortDown");
    public static readonly Texture2D ScaleBalanced = ContentFinder<Texture2D>.Get("Icons/ScaleBalanced");
    public static readonly Texture2D Bookmarked = ContentFinder<Texture2D>.Get("Icons/Actions/Bookmarked");
    public static readonly Texture2D FloppyDisk = ContentFinder<Texture2D>.Get("Icons/Actions/FloppyDisk");
    public static readonly Texture2D PenToSquare = ContentFinder<Texture2D>.Get("Icons/Actions/PenToSquare");
    public static readonly Texture2D WandMagicSparkles = ContentFinder<Texture2D>.Get("Icons/WandMagicSparkles");
    public static readonly Texture2D FilterCircleXMark = ContentFinder<Texture2D>.Get("Icons/Actions/FilterCircleXMark");

    public static readonly Texture2D Kick = ContentFinder<Texture2D>.Get("Icons/Brands/Kick");
    public static readonly Texture2D Steam = ContentFinder<Texture2D>.Get("Icons/Brands/Steam");
    public static readonly Texture2D Trovo = ContentFinder<Texture2D>.Get("Icons/Brands/Trovo");
    public static readonly Texture2D GitHub = ContentFinder<Texture2D>.Get("Icons/Brands/GitHub");
    public static readonly Texture2D Twitch = ContentFinder<Texture2D>.Get("Icons/Brands/Twitch");
    public static readonly Texture2D Discord = ContentFinder<Texture2D>.Get("Icons/Brands/Discord");
    public static readonly Texture2D YouTube = ContentFinder<Texture2D>.Get("Icons/Brands/YouTube");
}
