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

using System.Runtime.Serialization;

namespace StreamKit.Platform.Twitch.Helix
{
    public enum CheermoteType
    {
        /// <summary>
        ///     Represents a cheermote with an unsupported type.
        /// </summary>
        Unknown,

        /// <summary>
        ///     A Twitch-defined cheermote that is shown in the bits card -- the
        ///     popup shown when you click the bits icon near the chat box.
        /// </summary>
        [EnumMember(Value = "global_first_party")]
        GlobalFirstParty,

        /// <summary>
        ///     A Twitch-defined cheermote that isn't shown in the bits card --
        ///     the popup shown when you click the bits icon near the chat box.
        /// </summary>
        [EnumMember(Value = "global_third_party")]
        GlobalThirdParty,

        /// <summary>
        ///     A custom cheermote provided by a broadcaster.
        /// </summary>
        [EnumMember(Value = "channel_custom")]
        ChannelCustom,

        /// <summary>
        ///     A cheermote provided by a sponsor.
        /// </summary>
        [EnumMember(Value = "sponsored")]
        Sponsored
    }
}
