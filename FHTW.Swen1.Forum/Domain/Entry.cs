using FHTW.Swen1.Forum.Repositories;
using FHTW.Swen1.Forum.System;

namespace FHTW.Swen1.Forum.Domain;



/// <summary>This class represents a forum entry.</summary>
public class Entry: Atom, IAtom, __IVerifiable
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // private static members                                                                                           //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Entry repository.</summary>
    private static EntryRepository _Repository = new();



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // constructors                                                                                                     //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Creates a new instance of this class.</summary>
    /// <param name="session">Session.</param>
    public Entry(Session? session, Thread thread): base(session)
    {
        Owner = session?.User?.UserName ?? string.Empty;
        Thread = thread;
    }


    /// <summary>Creates a new instance of this class.</summary>
    internal Entry(): this(null, new Thread())
    {}



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public properties                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Gets the entry ID.</summary>
    public int ID
    {
        get { return (int?) _InternalID ?? -1; }
    }


    /// <summary>Gets or sets the thread title.</summary>
    public string Text
    {
        get; set;
    } = string.Empty;


    /// <summary>Gets or sets the entry time.</summary>
    public DateTime Time
    {
        get; internal set;
    } = DateTime.Now;


    /// <summary>Gets the thread the entry belongs to.</summary>
    public Thread Thread
    {
        get; internal set;
    }


    /// <summary>Gets the entry owner user name.</summary>
    public string Owner
    {
        get; internal set;
    } = string.Empty;



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public static properties                                                                                         //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Gets all threads.</summary>
    public static IEnumerable<Entry> All
    {
        get { return _Repository.GetAll(); }
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public static methods                                                                                            //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Gets an entry by its ID.</summary>
    public static Entry? Get(int id)
    {
        return _Repository.Get(id);
    }


    /// <summary>Gets the entries for a thread.</summary>
    /// <param name="thread">Thread.</param>
    /// <returns>Returns all entries for the given thread.</returns>
    public static IEnumerable<Entry> For(Thread thread)
    {
        return _Repository.For(thread);
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
