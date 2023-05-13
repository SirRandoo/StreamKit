﻿// MIT License
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

namespace StreamKit.Platform.Abstractions
{
    /// <summary>
    ///     Represents a response from the mod itself.
    ///     <br/>
    ///     <br/>
    ///     Responses are returned from commands and given to all
    ///     <see cref="IPlatform"/>s registered with the mod. Responses only
    ///     provide an override of <see cref="object.ToString()"/> for use in
    ///     text-based platform implementations -- handling of response
    ///     objects outside of this use case is up to the platform itself.
    /// </summary>
    /// <seealso cref="PlatformRegistry"/>
    public interface IResponse
    {
        /// <summary>
        ///     The intended recipient of the response. If there is no intended
        ///     recipient the value of this property will be
        ///     <see langword="null"/>.
        /// </summary>
        IUser Recipient { get; set; }
    }
}