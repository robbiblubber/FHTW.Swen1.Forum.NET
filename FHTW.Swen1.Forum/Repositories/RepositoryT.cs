namespace FHTW.Swen1.Forum.Repositories;

using global::System.Collections;
using global::System.Data;
using global::System.Data.SQLite;

using FHTW.Swen1.Forum.System;



/// <summary>This class provides a generic base implementation for repository classes.</summary>
/// <typeparam name="T">Type.</typeparam>
public abstract class Repository<T>: IRepository<T>, IRepository where T: IAtom, __IVerifiable
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // private members                                                                                                  //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Database connection.</summary>
    private static IDbConnection? _DbConnection;
    


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected properties                                                                                             //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Gets the database connection object.</summary>
    protected static IDbConnection _Cn
    {
        get
        {
            if(_DbConnection == null) 
            {
                _DbConnection = new SQLiteConnection("Data Source=forum.db; Version=3;");
                _DbConnection.Open();
            }

            return _DbConnection;
        }
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected methods                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Creates an objevt from a data reader.</summary>
    /// <param name="re">Data reader.</param>
    /// <returns>Returns an object.</returns>
    protected virtual T _CreateObject(IDataReader re)
    {
        T rval = (T) Activator.CreateInstance(typeof(T))!;
        ((__IVerifiable) rval).__InternalID = re.GetInt("ID");
        
        return _RefreshObject(re, rval);
    }


    /// <summary>Refreshes an object from a data reader.</summary>
    /// <param name="re">Data reader.</param>
    /// <param name="rval">Object.</param>
    /// <returns>Returns the objevt that has been refreshed.</returns>
    protected abstract T _RefreshObject(IDataReader re, T rval);



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [interface] IRepsoitory<T>                                                                                       //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Gets an object by its ID.</summary>
    /// <param name="id">Object ID.</param>
    /// <param name="session">Session.</param>
    /// <returns>Returns the requested object or NULL if no object was found.</returns>
    public abstract T? Get(object id, Session? session = null);


    /// <summary>Gets all objects.</summary>
    /// <param name="session">Session.</param>
    /// <returns>Returns a set containing all objects for the repository type.</returns>
    public abstract IEnumerable<T> GetAll(Session? session = null);


    /// <summary>Refreshes the object with persistant data.</summary>
    /// <param name="obj">Object.</param>
    public abstract void Refresh(T obj);


    /// <summary>Saves the object to persistance layer.</summary>
    /// <param name="obj">Object.</param>
    public abstract void Save(T obj);


    /// <summary>Deletes the object from persistance layer.</summary>
    /// <param name="obj">Object.</param>
    public abstract void Delete(T obj);



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [interface] IRepsoitory                                                                                          //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Gets an object by its ID.</summary>
    /// <param name="id">Object ID.</param>
    /// <param name="session">Session.</param>
    /// <returns>Returns the requested object or NULL if no object was found.</returns>
    object? IRepository.Get(object id, Session? session)
    {
        return Get(id, session);
    }


    /// <summary>Gets all objects.</summary>
    /// <param name="session">Session.</param>
    /// <returns>Returns a set containing all objects for the repository type.</returns>
    IEnumerable IRepository.GetAll(Session? session)
    {
        return GetAll(session);
    }


    /// <summary>Refreshes the object with persistant data.</summary>
    /// <param name="obj">Object.</param>
    void IRepository.Refresh(object obj)
    {
        Refresh((T) obj);
    }


    /// <summary>Saves the object to persistance layer.</summary>
    /// <param name="obj">Object.</param>
    void IRepository.Save(object obj)
    {
        Save((T) obj);
    }


    /// <summary>Deletes the object from persistance layer.</summary>
    /// <param name="obj">Object.</param>
    void IRepository.Delete(object obj)
    {
        Delete((T) obj);
    }
}
