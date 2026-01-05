namespace FHTW.Swen1.Forum.Repositories;

using global::System.Collections;
using FHTW.Swen1.Forum.System;



/// <summary>Repository classes implement this interface.</summary>
public interface IRepository
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public methods                                                                                                   //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Gets an object by its ID.</summary>
    /// <param name="id">Object ID.</param>
    /// <param name="session">Session.</param>
    /// <returns>Returns the requested object or NULL if no object was found.</returns>
    public object? Get(object id, Session? session = null);


    /// <summary>Gets all objects.</summary>
    /// <param name="session">Session.</param>
    /// <returns>Returns a set containing all objects for the repository type.</returns>
    public IEnumerable GetAll(Session? session = null);


    /// <summary>Refreshes the object with persistant data.</summary>
    /// <param name="obj">Object.</param>
    public void Refresh(object obj);


    /// <summary>Saves the object to persistance layer.</summary>
    /// <param name="obj">Object.</param>
    public void Save(object obj);


    /// <summary>Deletes the object from persistance layer.</summary>
    /// <param name="obj">Object.</param>
    public void Delete(object obj);
}
