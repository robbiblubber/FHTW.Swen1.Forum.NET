namespace FHTW.Swen1.Forum.Repositories;

using global::System.Data;

using FHTW.Swen1.Forum.Domain;
using FHTW.Swen1.Forum.System;



/// <summary>This class provides a repository for entry objects.</summary>
public sealed class EntryRepository: Repository<Entry>, IRepository<Entry>, IRepository
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public methods                                                                                                   //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Gets the threads for an owner and/or a time range.</summary>
    /// <param name="owner">Owner.</param>
    /// <param name="from">Start time.</param>
    /// <param name="to">End time.</param>
    /// <param name="session">Session.</param>
    /// <returns>Returns all threads that match the given criteria.</returns>
    public IEnumerable<Entry> For(Thread thread)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "SELECT ID, TEXT, TIME, KTHREAD, OWNER FROM ENTRIES WHERE KTHREAD = :p";
        cmd.BindParam(":p", thread.ID);
        
        using IDataReader re = cmd.ExecuteReader();
        while(re.Read())
        {
            yield return _CreateObject(re);
        }
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [override] Repository<Entry>                                                                                     //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Refreshes an object from a data reader.</summary>
        /// <param name="re">Data reader.</param>
        /// <param name="rval">Object.</param>
        /// <returns>Returns the objevt that has been refreshed.</returns>
    protected override Entry _RefreshObject(IDataReader re, Entry obj)
    {
        obj.Text = re.GetString("TEXT");
        obj.Time = re.GetDateTime("TIME");
        obj.Thread = Thread.Get(re.GetInt("KTHREAD")) ?? throw new InvalidDataException("No such thread.");
        obj.Owner = re.GetString("OWNER");

        return obj;
    }


    /// <summary>Gets an object by its ID.</summary>
    /// <param name="id">Object ID.</param>
    /// <param name="session">Session.</param>
    /// <returns>Returns the requested object or NULL if no object was found.</returns>
    public override Entry? Get(object id, Session? session = null)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "SELECT ID, TEXT, TIME, KTHREAD, OWNER FROM ENTRIES WHERE ID = :id";
        cmd.BindParam(":id", id);

        using IDataReader re = cmd.ExecuteReader();
        if(re.Read())
        {
            return _CreateObject(re);
        }

        return null;
    }


    /// <summary>Gets all objects.</summary>
    /// <param name="session">Session.</param>
    /// <returns>Returns a set containing all objects for the repository type.</returns>
    public override IEnumerable<Entry> GetAll(Session? session = null)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "SELECT ID, TEXT, TIME, KTHREAD, OWNER FROM ENTRIES";
        
        using IDataReader re = cmd.ExecuteReader();
        while(re.Read())
        {
            yield return _CreateObject(re);
        }
    }


    /// <summary>Refreshes the object with persistant data.</summary>
    /// <param name="obj">Object.</param>
    public override void Refresh(Entry obj)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "SELECT ID, TEXT, TIME, KTHREAD, OWNER FROM ENTRIES WHERE ID = :id";
        cmd.BindParam(":id", obj.ID);
        
        using IDataReader re = cmd.ExecuteReader();
        if(re.Read())
        {
            _RefreshObject(re, obj);
        }
    }


    /// <summary>Saves the object to persistance layer.</summary>
    /// <param name="obj">Object.</param>
    public override void Save(Entry obj)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        if(((__IVerifiable) obj).__InternalID is null)
        {
            cmd.CommandText = "INSERT INTO ENTRIES (TEXT, TIME, KTHREAD, OWNER ) VALUES (:t, :m, :p, :o)";
        }
        else
        {
            cmd.CommandText = "UPDATE ENTRIES SET TEXT = :t, TIME = :m, KTHREAD = :p, OWNER = :o WHERE ID = :id";
            cmd.BindParam(":id", obj.ID);
        }

        cmd.BindParam(":t", obj.Text).BindParam(":m", obj.Time).BindParam(":p", obj.Thread.ID).BindParam(":o", obj.Owner)
           .ExecuteNonQuery();
    }


    /// <summary>Deletes the object from persistance layer.</summary>
    /// <param name="obj">Object.</param>
    public override void Delete(Entry obj)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "DELETE FROM ENTRIES WHERE ID = :id";
        cmd.BindParam(":id", obj.ID)
           .ExecuteNonQuery();
    }
}
