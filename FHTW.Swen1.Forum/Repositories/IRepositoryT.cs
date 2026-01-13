namespace FHTW.Swen1.Forum.Repositories;

using FHTW.Swen1.Forum.System;



/// <summary>Typed repository classes implement this interface.</summary>
/// <typeparam name="T">Type.</typeparam>
public interface IRepository<T>: IRepository where T: IAtom, __IVerifiable
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public methods                                                                                                   //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Gets an object by its ID.</summary>
    /// <param name="id">Object ID.</param>
    /// <param name="session">Session.</param>
    /// <returns>Returns the requested object or NULL if no object was found.</returns>
    public new T? Get(object id, Session? session = null);


    /// <summary>Gets all objects.</summary>
    /// <param name="session">Session.</param>
    /// <returns>Returns a set containing all objects for the repository type.</returns>
    public new IEnumerable<T> GetAll(Session? session = null);


    /// <summary>Refreshes the object with persistant data.</summary>
    /// <param name="obj">Object.</param>
    public void Refresh(T obj);


    /// <summary>Saves the object to persistance layer.</summary>
    /// <param name="obj">Object.</param>
    public void Save(T obj);


    /// <summary>Deletes the object from persistance layer.</summary>
    /// <param name="obj">Object.</param>
    public void Delete(T obj);
}
