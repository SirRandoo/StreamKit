using System;
using System.Collections.Generic;

namespace StreamKit.Parsers.Irc
{
    /// <inheritdoc cref="IMessageTag"/>
    public class BadgesMessageTag : MessageTag
    {
        private string _value;

        /// <inheritdoc/>
        public override string Value
        {
            get => _value;
            set
            {
                _value = value;

                var current = new Badge();
                var tokenStart = 0;

                ReadOnlySpan<char> span = _value.AsSpan();

                for (var i = 0; i < span.Length; i++)
                {
                    switch (span[i])
                    {
                        case '/':
                            current.Key = span.Slice(tokenStart, i - tokenStart).ToString();
                            tokenStart = i + 1;

                            break;
                        case ',':
                            current.Version = span.Slice(tokenStart, i - tokenStart).ToString();
                            Badges.Add(current);

                            current = new Badge();
                            tokenStart = i + 1;

                            break;
                    }
                }

                current.Version = span.Slice(tokenStart).ToString();
                Badges.Add(current);
            }
        }

        /// <summary>
        ///     The parsed list of badges within the tag.
        /// </summary>
        public List<Badge> Badges { get; } = new List<Badge>();
    }
}
