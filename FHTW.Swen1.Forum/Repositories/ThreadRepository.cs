namespace FHTW.Swen1.Forum.Repositories;

using global::System.Data;

using FHTW.Swen1.Forum.System;
using FHTW.Swen1.Forum.Domain;



/// <summary>This class provides a repository for thread objects.</summary>
public sealed class ThreadRepository: Repository<Thread>, IRepository<Thread>, IRepository
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
    public IEnumerable<Thread> For(string? owner = null, DateTime? from = null, DateTime? to = null, Session? session = null)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "SELECT ID, TITLE, TIME, OWNER FROM THREADS";
        
        if(owner is not null)
        {
            cmd.CommandText += " WHERE OWNER = :o";
            cmd.BindParam(":o", owner);
        }
        
        if(from is not null)
        {
            cmd.CommandText += " WHERE TIME >= :f";
            cmd.BindParam(":f", from);
        }
        
        if(to is not null)
        {
            cmd.CommandText += " WHERE TIME <= :t";
            cmd.BindParam(":t", to);
        }

        using IDataReader re = cmd.ExecuteReader();
        while(re.Read())
        {
            yield return _CreateObject(re);
        }
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [override] Repository<Thread>                                                                                    //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Refreshes an object from a data reader.</summary>
        /// <param name="re">Data reader.</param>
        /// <param name="rval">Object.</param>
        /// <returns>Returns the objevt that has been refreshed.</returns>
    protected override Thread _RefreshObject(IDataReader re, Thread obj)
    {
        obj.Title = re.GetString("TITLE");
        obj.Time = re.GetDateTime("TIME");
        obj.Owner = re.GetString("OWNER");

        return obj;
    }


    /// <summary>Gets an object by its ID.</summary>
    /// <param name="id">Object ID.</param>
    /// <param name="session">Session.</param>
    /// <returns>Returns the requested object or NULL if no object was found.</returns>
    public override Thread? Get(object id, Session? session = null)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "SELECT ID, TITLE, TIME, OWNER FROM THREADS WHERE ID = :id";
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
    public override IEnumerable<Thread> GetAll(Session? session = null)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "SELECT ID, TITLE, TIME, OWNER FROM THREADS";
        
        using IDataReader re = cmd.ExecuteReader();
        while(re.Read())
        {
            yield return _CreateObject(re);
        }
    }


    /// <summary>Refreshes the object with persistant data.</summary>
    /// <param name="obj">Object.</param>
    public override void Refresh(Thread obj)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "SELECT ID, TITLE, TIME, OWNER FROM THREADS WHERE ID = :id";
        cmd.BindParam(":id", obj.ID);
        
        using IDataReader re = cmd.ExecuteReader();
        if(re.Read())
        {
            _RefreshObject(re, obj);
        }
    }


    /// <summary>Saves the object to persistance layer.</summary>
    /// <param name="obj">Object.</param>
    public override void Save(Thread obj)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        if(((__IVerifiable) obj).__InternalID is null)
        {
            cmd.CommandText = "INSERT INTO THREADS (TITLE, TIME, OWNER) VALUES (:t, :m, :o)";
        }
        else
        {
            cmd.CommandText = "UPDATE THREADS SET TITLE = :t, TIME = :m, OWNER = :o WHERE ID = :id";
            cmd.BindParam(":id", obj.ID);
        }

        cmd.BindParam(":t", obj.Title).BindParam(":m", obj.Time).BindParam(":o", obj.Owner)
           .ExecuteNonQuery();

        if(((__IVerifiable) obj).__InternalID is null)
        {
            using IDbCommand rcmd = _Cn.CreateCommand();
            rcmd.CommandText = $"SELECT last_insert_rowid()";
            ((__IVerifiable) obj).__InternalID = Convert.ToInt32(rcmd.ExecuteScalar());
        }
    }


    /// <summary>Deletes the object from persistance layer.</summary>
    /// <param name="obj">Object.</param>
    public override void Delete(Thread obj)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "DELETE FROM THREADS WHERE ID = :id";
        cmd.BindParam(":id", obj.ID)
           .ExecuteNonQuery();
    }
}
