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
using NetEscapades.EnumGenerators;

namespace StreamKit.Parsers.Irc;

public record IrcMessage(MessageTag[] Tags, string Prefix, string Command, string Params) : IIrcMessage;

public static class IrcParser
{
    public static IIrcMessage Parse(ReadOnlySpan<char> message)
    {
        Boundaries boundaries = GetMessageBoundaries(message);
        MessageTag[] tags = ParseTags(boundaries.Tags);

        var command = boundaries.Command.ToString();

        return new IrcMessage(tags, boundaries.Prefix.ToString(), command, boundaries.Params.ToString());
    }

    private static Boundaries GetMessageBoundaries(ReadOnlySpan<char> message)
    {
        var boundaries = new Boundaries();

        var section = MessageSection.None;
        var boundaryStart = 0;
        var spacesEncountered = 0;

        for (var i = 0; i < message.Length; i++)
        {
            switch (message[i])
            {
                case '@':
                    boundaryStart = i + 1;
                    section = MessageSection.Tags;

                    break;
                case ':' when spacesEncountered == 1:
                    boundaryStart = i + 1;
                    section = MessageSection.Prefix;

                    break;
                case ' ' when spacesEncountered == 2:
                    boundaryStart = i + 1;
                    section = MessageSection.Command;

                    break;
                case ' ' when spacesEncountered == 3:
                    boundaryStart = i + 1;
                    section = MessageSection.Params;

                    break;
                case ' ':
                    spacesEncountered += 1;
                    ReadOnlySpan<char> sectionSpan = message.Slice(boundaryStart, i - boundaryStart);

                    boundaryStart = i + 1;

                    switch (section)
                    {
                        case MessageSection.Tags:
                            boundaries.Tags = sectionSpan;

                            break;
                        case MessageSection.Command:
                            boundaries.Command = sectionSpan;

                            break;
                        case MessageSection.Prefix:
                            boundaries.Prefix = sectionSpan;

                            break;
                    }

                    break;
            }
        }

        boundaries.Params = message.Slice(boundaryStart);

        return boundaries;
    }

    public static MessageTag[] ParseTags(ReadOnlySpan<char> tags)
    {
        var tagCount = 0;

        foreach (char c in tags)
        {
            switch (c)
            {
                case ' ':
                case ';':
                    tagCount += 1;

                    break;
            }
        }

        var tagIndex = 0;
        var tagsArray = new MessageTag[tagCount + 1];
        MessageTag? current = null;
        var tokenStart = 0;

        for (var i = 0; i < tags.Length; i++)
        {
            switch (tags[i])
            {
                case ' ':
                case ';' when current is not null:
                    current!.Value = tags.Slice(tokenStart, i - tokenStart).ToString();
                    tagsArray[tagIndex++] = current;

                    tokenStart = i + 1;

                    break;
                case '=':
                    var name = tags.Slice(tokenStart, i - tokenStart).ToString();
                    current = GetTagObjectFor(name);

                    tokenStart = i + 1;

                    break;
            }
        }

        current!.Value = tags[tokenStart..].ToString();
        tagsArray[tagIndex] = current;

        return tagsArray;
    }

    private static MessageTag GetTagObjectFor(string name)
    {
        switch (name)
        {
            case "badges":
                return new BadgesMessageTag(name, string.Empty);
            case "bits":
                return new IntegerMessageTag(name, string.Empty);
        }

        return new MessageTag(name, string.Empty);
    }

    [EnumExtensions] private enum MessageSection { None, Tags, Prefix, Command, Params }
}
