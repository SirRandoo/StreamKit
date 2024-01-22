using System;
using StreamKit.Data.Abstractions;

namespace StreamKit.Api;

public interface IUser : IIdentifiable
{
    DateTime LastSeen { get; set; }
    IPlatform Platform { get; init; }
    IViewerData Data { get; set; }
    UserPrivileges Privileges { get; set; }
}