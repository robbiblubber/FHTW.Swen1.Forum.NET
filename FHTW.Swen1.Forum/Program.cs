using FHTW.Swen1.Forum.Handlers;
using FHTW.Swen1.Forum.Server;
using FHTW.Swen1.Forum.System;

namespace FHTW.Swen1.Forum;

/// <summary>Program class.</summary>
internal static class Program
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // entry point                                                                                                      //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Main entry point of the application.</summary>
    /// <param name="args">Command line arguments.</param>
    static void Main(string[] args)
    {
        User u1 = new();
        u1.UserName = "sophie";
        u1.FullName = "Sophie";
        u1.EMail = "sophie";
        u1.IsAdmin = true;
        u1.SetPassword("sophie");
        u1.Save();


        return;

        HttpRestServer svr = new();
        svr.RequestReceived += Handler.HandleEvent;
        svr.Run();
    }
}
