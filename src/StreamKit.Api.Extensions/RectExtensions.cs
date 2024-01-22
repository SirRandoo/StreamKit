using System;
using RimWorld;
using UnityEngine;

namespace StreamKit.Api.Extensions;

/// <summary>
///     A collection of extension methods for modifying <see cref="Rect" />s without an allocation.
/// </summary>
/// <remarks>
///     These extension methods mutate an existing <see cref="Rect" />, and thus aren't suitable for
///     operations that require immutable modifications.
/// </remarks>
public static class RectExtensions
{
    /// <summary>
    ///     Shifts a region to the direction specified with an optional padding between the origin region
    ///     and the destination region.
    /// </summary>
    /// <param name="region">The original region being shifted in a given direction.</param>
    /// <param name="direction">The direction a region is being shifted to.</param>
    /// <param name="padding">An optional amount of additional shifting the region will go through.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">An unsupported direction was specified.</exception>
    public static ref Rect Shift(this ref Rect region, Direction8Way direction, float padding = 2f)
    {
        switch (direction)
        {
            case Direction8Way.North:
                region.y -= region.height + padding;

                break;
            case Direction8Way.NorthEast:
                region.x += region.width + padding;
                region.y -= region.height + padding;

                break;
            case Direction8Way.East:
                region.x += region.width + padding;

                break;
            case Direction8Way.SouthEast:
                region.x += region.width + padding;
                region.y += region.height + padding;

                break;
            case Direction8Way.South:
                region.y += region.height + padding;

                break;
            case Direction8Way.SouthWest:
                region.y += region.height + padding;
                region.x -= region.width + padding;

                break;
            case Direction8Way.West:
                region.x -= region.width + padding;

                break;
            case Direction8Way.NorthWest:
                region.x -= region.width + padding;
                region.y -= region.height + padding;

                break;
            case Direction8Way.Invalid:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, $@"The direction ""{direction.ToStringFast()}"" isn't supported for shift operations.");
        }

        return ref region;
    }

    public static ref Rect ContractedBy(this ref Rect region, float margin)
    {
        region.x += margin;
        region.y += margin;
        region.height -= margin + margin;
        region.width -= margin + margin;

        return ref region;
    }

    public static ref Rect ExpandedBy(this ref Rect region, float margin)
    {
        region.x -= margin;
        region.y -= margin;
        region.height += margin + margin;
        region.width += margin + margin;

        return ref region;
    }

    public static ref Rect AtZero(this ref Rect region)
    {
        region.x = 0f;
        region.y = 0f;

        return ref region;
    }

    public static ref Rect SetX(this ref Rect region, float x)
    {
        region.x = x;

        return ref region;
    }

    public static ref Rect SetY(this ref Rect region, float y)
    {
        region.y = y;

        return ref region;
    }

    public static ref Rect SetWidth(this ref Rect region, float width)
    {
        region.width = width;

        return ref region;
    }

    public static ref Rect SetHeight(this ref Rect region, float height)
    {
        region.height = height;

        return ref region;
    }
}

public static class Direction8WayExtensions
{
    public static string ToStringFast(this Direction8Way direction)
    {
        return direction switch
        {
            Direction8Way.North => nameof(Direction8Way.North),
            Direction8Way.NorthEast => nameof(Direction8Way.NorthEast),
            Direction8Way.East => nameof(Direction8Way.East),
            Direction8Way.SouthEast => nameof(Direction8Way.SouthEast),
            Direction8Way.South => nameof(Direction8Way.South),
            Direction8Way.SouthWest => nameof(Direction8Way.SouthWest),
            Direction8Way.West => nameof(Direction8Way.West),
            Direction8Way.NorthWest => nameof(Direction8Way.NorthWest),
            Direction8Way.Invalid => nameof(Direction8Way.Invalid),
            var _ => direction.ToString()
        };
    }
}
