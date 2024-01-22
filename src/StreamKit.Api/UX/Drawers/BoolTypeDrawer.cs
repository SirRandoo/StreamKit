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

using SirRandoo.CommonLib.Helpers;
using StreamKit.Api.Extensions;
using UnityEngine;
using Verse;

namespace StreamKit.Api.UX.Drawers;

/// <summary>
///     A specialized class for drawing bools on screen.
/// </summary>
public class BoolTypeDrawer : TypeDrawer<bool>
{
    private const float AnimationSpeed = 0.16f;
    private const float SliderWidthPercent = 0.24f;

    private float _animationPercent;
    private SliderState _sliderAnimationState;
    private readonly bool _useSliders;

    /// <summary>
    ///     A specialized class for drawing bools on screen.
    /// </summary>
    public BoolTypeDrawer(bool initialValue)
    {
        _useSliders = RuntimeConfig.HasFlag(RuntimeConfig.RuntimeFlags.ToggleSliders);
        _sliderAnimationState = initialValue ? SliderState.On : SliderState.Off;
    }

    private bool IsAnimating => _sliderAnimationState is SliderState.TransitioningOff or SliderState.TransitioningOn;

    /// <inheritdoc />
    public override void Draw(ref Rect region)
    {
        if (!_useSliders)
        {
            DrawCheckbox(ref region);
        }
        else
        {
            DrawToggleSlider(ref region);
        }
    }

    private void DrawCheckbox(ref Rect region)
    {
        bool value = Value;

        Rect checkboxRegion = LayoutHelper.IconRect(region.x + region.width - region.height, region.y, region.height, region.height);

        if (UiHelper.DrawCheckbox(checkboxRegion, ref value))
        {
            Value = value;
        }
    }

    private void DrawToggleSlider(ref Rect region)
    {
        float slicedRegion = region.width * SliderWidthPercent;
        var workingRegion = new Rect(region.x + region.width - slicedRegion, region.y + (Text.SmallFontHeight * 0.15f), slicedRegion, region.height - (Text.SmallFontHeight * 0.30f));

        float dotOffset = (workingRegion.width - workingRegion.height) * _animationPercent;
        Rect dotRegion = LayoutHelper.IconRect(workingRegion.x + dotOffset, workingRegion.y, workingRegion.height, workingRegion.height);
        var backgroundRegion = new Rect(workingRegion.x, workingRegion.y, workingRegion.width * _animationPercent, workingRegion.height);
        dotRegion = RectExtensions.ContractedBy(ref dotRegion, 5f);

        if (_animationPercent >= 0.3f && (IsAnimating || _sliderAnimationState is SliderState.On))
        {
            GUI.color = new Color(1f, 1f, 1f, 0.25f);
            Widgets.DrawAtlas(backgroundRegion, Icons.SquareFilled);
            GUI.color = Color.white;
        }

        GUI.DrawTexture(dotRegion, Icons.CircleFilled, ScaleMode.ScaleToFit);
        Widgets.DrawAtlas(workingRegion, Icons.Square);

        if (!IsAnimating)
        {
            return;
        }

        switch (_sliderAnimationState)
        {
            case SliderState.TransitioningOff:
                _animationPercent = Mathf.SmoothStep(_animationPercent, 1f, AnimationSpeed);

                if (_animationPercent <= 0.01f)
                {
                    _sliderAnimationState = SliderState.Off;
                }

                break;
            case SliderState.TransitioningOn:
                _animationPercent = Mathf.SmoothStep(_animationPercent, 0f, AnimationSpeed);

                if (_animationPercent > 9.9f)
                {
                    _sliderAnimationState = SliderState.On;
                }

                break;
        }
    }

    /// <inheritdoc />
    public override void Toggle()
    {
        bool value = Value;

        Value = !value;
        _sliderAnimationState = !value ? SliderState.TransitioningOff : SliderState.TransitioningOn;
    }

    private enum SliderState { TransitioningOn, On, TransitioningOff, Off }
}
