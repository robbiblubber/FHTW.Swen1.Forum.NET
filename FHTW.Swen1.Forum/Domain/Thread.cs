namespace FHTW.Swen1.Forum.Domain;

using global::System.Data;

using FHTW.Swen1.Forum.Repositories;
using FHTW.Swen1.Forum.System;



/// <summary>This class represents a forum thread.</summary>
public class Thread: Atom, IAtom, __IVerifiable
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // private static members                                                                                           //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Thread repository.</summary>
    private static ThreadRepository _Repository = new();



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // constructors                                                                                                     //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Creates a new instance of this class.</summary>
    /// <param name="session">Session.</param>
    public Thread(Session? session): base(session)
    {
        Owner = session?.User?.UserName ?? string.Empty;
    }


    /// <summary>Creates a new instance of this class.</summary>
    internal Thread(): this(null)
    {}



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public properties                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Gets the thread ID.</summary>
    public int ID
    {
        get { return (int?) _InternalID ?? -1; }
    }


    /// <summary>Gets or sets the thread title.</summary>
    public string Title
    {
        get; set;
    } = string.Empty;


    /// <summary>Gets or sets the thread time.</summary>
    public DateTime Time
    {
        get; internal set;
    } = DateTime.Now;


    /// <summary>Gets the thread owner user name.</summary>
    public string Owner
    {
        get; internal set;
    } = string.Empty;


    /// <summary>Gets the entries for this thread.</summary>
    public IEnumerable<Entry> Entries
    {
        get { return Entry.For(this); }
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public static properties                                                                                         //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Gets all threads.</summary>
    public static IEnumerable<Thread> All
    {
        get { return _Repository.GetAll(); }
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public static methods                                                                                            //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Gets a thread by its ID.</summary>
    public static Thread? Get(int id)
    {
        return _Repository.Get(id);
    }


    /// <summary>Gets the threads for an owner and/or a time range.</summary>
    /// <param name="owner">Owner.</param>
    /// <param name="from">Start time.</param>
    /// <param name="to">End time.</param>
    /// <param name="session">Session.</param>
    /// <returns>Returns all threads that match the given criteria.</returns>
    public static IEnumerable<Thread> For(string? owner = null, DateTime? from = null, DateTime? to = null, Session? session = null)
    {
        return _Repository.For(owner, from, to, session);
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [override] Atom                                                                                                  //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Gets the repository for this class.</summary>
    /// <returns>Returns the repository.</returns>
    protected override IRepository _GetRepository()
    {
        return _Repository;
    }

       
    /// <summary>Saves the object.</summary>
    public override void Save()
    {
        _EnsureAdminOrOwner(Owner);
        base.Save();
    }


    /// <summary>Deletes the object.</summary>
    public override void Delete()
    {
        _EnsureAdminOrOwner(Owner);
        base.Delete();
    }
}
