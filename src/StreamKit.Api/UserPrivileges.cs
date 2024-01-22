using System;
using NetEscapades.EnumGenerators;

namespace StreamKit.Api;

[Flags] [EnumExtensions] public enum UserPrivileges { None = 0, Vip = 1, Subscriber = 2, Moderator = 4 }