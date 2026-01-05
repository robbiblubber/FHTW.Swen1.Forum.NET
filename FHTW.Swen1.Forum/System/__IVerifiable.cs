namespace FHTW.Swen1.Forum.System;



/// <summary>Objects that allow verification implement this interface.</summary>
public interface __IVerifiable
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public properties                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Gets or sets the internal ID for the object.</summary>
    public object? __InternalID { get; set; }


    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public methods                                                                                                   //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Verifies the session locking the object.</summary>
    /// <param name="session">Session.</param>
    public void __VerifySession(Session? session = null);


    /// <summary>Ends editing the object.</summary>
    public void __EndEdit();


    /// <summary>Ensures the session locking the object has administrative privileges.</summary>
    public void __EnsureAdmin();


    /// <summary>Ensures the session locking the object has administrative privileges or is the object owner.</summary>
    /// <param name="owner">Owner.</param>
    public void __EnsureAdminOrOwner(string? owner);
}
