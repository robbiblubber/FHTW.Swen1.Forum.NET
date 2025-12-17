using FHTW.Swen1.Forum.Repositories;

namespace FHTW.Swen1.Forum.System;

/// <summary>This class provides a base implementation for data objects.</summary>
public abstract class Atom: IAtom, __IVerifiable
{
    protected Session? _EditingSession = null;

    protected object? _InternalID;


    public Atom(Session? session)
    {
        _EditingSession = session;
    }


    protected abstract IRepository _GetRepository();


    protected void _VerifySession(Session? session = null)
    {
        if(session is not null) { _EditingSession = session; }
        if(_EditingSession is null || !_EditingSession.Valid) { throw new UnauthorizedAccessException("Invalid session."); }
    }

    protected void _EndEdit()
    {
        _EditingSession = null;
    }

    protected void _EnsureAdmin()
    {
        if(!(_EditingSession?.IsAdmin ?? false))
        {
            throw new UnauthorizedAccessException("Admin privileges required.");
        }
    }


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
    
    object? __IVerifiable.__InternalID
    { 
        get { return _InternalID; }
        set { _InternalID = value; }
    }


    /// <summary>Verifies a session.</summary>
    /// <param name="session">Session.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when the session could not be verified.</exception>
    void __IVerifiable.__VerifySession(Session? session)
    {
        _VerifySession(session);
    }


    /// <summary>Ends editing the object.</summary>
    void __IVerifiable.__EndEdit()
    {
        _EndEdit();
    }


    /// <summary>Checks if the session has administrative privileges.</summary>
    /// <exception cref="UnauthorizedAccessException">Thrown when the session doesn't have access.</exception>
    void __IVerifiable.__EnsureAdmin()
    {
        _EnsureAdmin();
    }


    /// <summary>Checks if the session has administrative privileges or represents the objevt owner.</summary>
    /// <exception cref="UnauthorizedAccessException">Thrown when the session doesn't have access.</exception>
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
