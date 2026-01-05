namespace FHTW.Swen1.Forum.System;



/// <summary>Objects that allow authentication implement this interface.</summary>
internal interface _IAuthentificable
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public properties                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Gets or sets the object user name.</summary>
    public string? __Username { get; set; }


    /// <summary>Gets or sets the object password hash.</summary>
    public string? __PasswordHash { get; set; }
}
