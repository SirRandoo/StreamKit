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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SirRandoo.CommonLib.Enums;
using UnityEngine;

namespace StreamKit.Mod.UX.Tables;

public abstract partial class TableDrawer<T>
{
    public class Builder<TBase>(Func<TBase> instantiator) where TBase : TableDrawer<T>
    {
        private readonly List<TableColumn> _columns = [];

        public Builder<TBase> WithColumn(Action<TableColumnOptions> columnOptionsAction)
        {
            var options = new TableColumnOptions();

            // User configure
            columnOptionsAction(options);

            _columns.Add(new TableColumn(options.Header, options.RelativeWidth, options.Icon, options.SortAction));

            return this;
        }

        public TBase Build(IEnumerable<T> dataSet)
        {
            TBase instance = instantiator();
            instance._columns = _columns;
            instance._columnRegions = new Rect[_columns.Count];
            instance._contents = dataSet.Select(i => new TableEntry(i)).ToList();
            instance._viewportHeight = instance._contents.Count * UiConstants.LineHeight;

            return instance;
        }
    }

    public class TableColumnOptions
    {
        public Texture2D? Icon { get; set; }
        public Action<SortOrder, IReadOnlyList<TableEntry>>? SortAction { get; set; }
        public string? Header { get; set; }
        public float RelativeWidth { get; set; }
    }

    private sealed class TableColumn(string? header, float relativeWidth, Texture2D? icon = null, Action<SortOrder, IReadOnlyList<TableEntry>>? sortAction = null)
    {
        public SortOrder SortOrder { get; set; }
        public string? Header { get; } = header;
        public float RelativeWidth { get; } = relativeWidth;
        public Texture2D? Icon { get; } = icon;
        public Action<SortOrder, IReadOnlyList<TableEntry>>? SortAction { get; } = sortAction;
    }

    public sealed class TableEntry(T data)
    {
        public T Data { get; } = data;
        public bool Visible { get; set; } = true;
    }
}
