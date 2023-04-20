// MIT License
// 
// Copyright (c) 2022 SirRandoo
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

namespace StreamKit.Platform.Abstractions
{
    /// <summary>
    ///     Represents a user on a given <see cref="IPlatform"/>.
    /// </summary>
    public interface IUser : IPlatformSided
    {
        /// <summary>
        ///     The display name of the user.
        ///     <br/>
        ///     <br/>
        ///     Display names are generally a variant of <see cref="Login"/> with
        ///     different capitalization, but in some cases, like on Twitch,
        ///     display names can be a completely different string altogether.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The login name, or better known as the "username", of a user.
        /// </summary>
        string Login { get; }

        /// <summary>
        ///     The types of privileges the user has within the channel.
        /// </summary>
        UserPrivileges Privileges { get; }
    }
}
