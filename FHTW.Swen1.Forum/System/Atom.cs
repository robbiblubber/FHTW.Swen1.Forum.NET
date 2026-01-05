namespace FHTW.Swen1.Forum.System;

using FHTW.Swen1.Forum.Repositories;



/// <summary>This class provides a base implementation for data objects.</summary>
public abstract class Atom: IAtom, __IVerifiable
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected members                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Editing session.</summary>
    protected Session? _EditingSession = null;

    /// <summary>Internal ID.</summary>
    protected object? _InternalID;



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // constructors                                                                                                     //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Creates a new instance of this class.</summary>
    /// <param name="session">Editing session.</param>
    public Atom(Session? session)
    {
        _EditingSession = session;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected methods                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Gets the repository for this class.</summary>
    /// <returns>Returns the repository.</returns>
    protected abstract IRepository _GetRepository();


    /// <summary>Verifies the session locking the object.</summary>
    /// <param name="session">Session.</param>
    protected void _VerifySession(Session? session = null)
    {
        if(session is not null) { _EditingSession = session; }
        if(_EditingSession is null || !_EditingSession.Valid) { throw new UnauthorizedAccessException("Invalid session."); }
    }


    /// <summary>Ends editing the object.</summary>
    protected void _EndEdit()
    {
        _EditingSession = null;
    }


    /// <summary>Ensures the session locking the object has administrative privileges.</summary>
    protected void _EnsureAdmin()
    {
        if(!(_EditingSession?.IsAdmin ?? false))
        {
            throw new UnauthorizedAccessException("Admin privileges required.");
        }
    }


    /// <summary>Ensures the session locking the object has administrative privileges or is the object owner.</summary>
    /// <param name="owner">Owner.</param>
    protected void _EnsureAdminOrOwner(string? owner)
    {
        ((__IVerifiable) this).__VerifySession();
        if(!(_EditingSession!.IsAdmin || (_EditingSession.UserName == owner)))
        {
            throw new UnauthorizedAccessException("Admin or owner privileges required.");
        }
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [interface] __IVerifiable                                                                                        //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Gets or sets the internal ID for the object.</summary>
    object? __IVerifiable.__InternalID
    { 
        get { return _InternalID; }
        set { _InternalID = value; }
    }


    /// <summary>Verifies the session locking the object.</summary>
    /// <param name="session">Session.</param>
    void __IVerifiable.__VerifySession(Session? session)
    {
        _VerifySession(session);
    }


    /// <summary>Ends editing the object.</summary>
    void __IVerifiable.__EndEdit()
    {
        _EndEdit();
    }


    /// <summary>Ensures the session locking the object has administrative privileges.</summary>
    void __IVerifiable.__EnsureAdmin()
    {
        _EnsureAdmin();
    }


    /// <summary>Ensures the session locking the object has administrative privileges or is the object owner.</summary>
    /// <param name="owner">Owner.</param>
    void __IVerifiable.__EnsureAdminOrOwner(string? owner)
    {
        _EnsureAdminOrOwner(owner);
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [interface] IAtom                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Begins editing the object.</summary>
    /// <param name="session">Session.</param>
    public virtual void BeginEdit(Session session)
    {
        _VerifySession(session);
    }


    /// <summary>Saves the object.</summary>
    public virtual void Save()
    {
        _GetRepository().Save(this);
        _EndEdit();
    }


    /// <summary>Deletes the object.</summary>
    public virtual void Delete()
    {
        _GetRepository().Delete(this);
        _EndEdit();
    }


    /// <summary>Refreshes the object.</summary>
    public virtual void Refresh()
    {
        _GetRepository().Refresh(this);
        _EndEdit();
    }
}
