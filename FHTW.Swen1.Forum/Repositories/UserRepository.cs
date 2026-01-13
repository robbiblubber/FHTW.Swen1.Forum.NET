namespace FHTW.Swen1.Forum.Repositories;

using global::System.Data;

using FHTW.Swen1.Forum.System;



/// <summary>This class provides a repository for user objects.</summary>
public sealed class UserRepository: Repository<User>, IRepository<User>, IRepository
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public methods                                                                                                   //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Performs a logon operation for a user.</summary>
    /// <param name="username">User name.</param>
    /// <param name="password">Password.</param>
    /// <returns>Returns a user object for successful logon, otherwise returns NULL.</returns>
    public User? Logon(string username, string password)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "SELECT USERNAME, NAME, EMAIL, HADMIN FROM USERS WHERE USERNAME = :u AND PASSWD = :p";
        cmd.BindParam(":u", username)
           .BindParam(":p", User._HashPassword(username, password));

        using IDataReader re = cmd.ExecuteReader();
        if(re.Read())
        {
            return _CreateObject(re);
        }

        return null;
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [override] Repository<User>                                                                                      //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Creates an objevt from a data reader.</summary>
    /// <param name="re">Data reader.</param>
    /// <returns>Returns an object.</returns>
    protected override User _CreateObject(IDataReader re)
    {
        User rval = new();
        ((__IVerifiable) rval).__InternalID = re.GetString("USERNAME");

        return _RefreshObject(re, rval);
    }


    /// <summary>Refreshes an object from a data reader.</summary>
    /// <param name="re">Data reader.</param>
    /// <param name="rval">Object.</param>
    /// <returns>Returns the objevt that has been refreshed.</returns>
    protected override User _RefreshObject(IDataReader re, User obj)
    {
        obj.FullName = re.GetString("NAME");
        obj.EMail = re.GetString("EMAIL");
        obj.IsAdmin = re.GetBool("HADMIN");

        return obj;
    }


    /// <summary>Gets an object by its ID.</summary>
    /// <param name="id">Object ID.</param>
    /// <param name="session">Session.</param>
    /// <returns>Returns the requested object or NULL if no object was found.</returns>
    public override User? Get(object id, Session? session = null)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "SELECT USERNAME, NAME, EMAIL, HADMIN FROM USERS WHERE USERNAME = :u";
        cmd.BindParam(":u", id);

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
    public override IEnumerable<User> GetAll(Session? session = null)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "SELECT USERNAME, NAME, EMAIL, HADMIN FROM USERS";

        using IDataReader re = cmd.ExecuteReader();
        while(re.Read())
        {
            yield return _CreateObject(re);
        }
    }


    /// <summary>Refreshes the object with persistant data.</summary>
    /// <param name="obj">Object.</param>
    public override void Refresh(User obj)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "SELECT NAME, EMAIL, HADMIN FROM USERS WHERE USERNAME = :u";
        cmd.BindParam(":u", obj.UserName);

        using IDataReader re = cmd.ExecuteReader();
        if(re.Read())
        {
            _RefreshObject(re, obj);
        }
    }


    /// <summary>Saves the object to persistance layer.</summary>
    /// <param name="obj">Object.</param>
    public override void Save(User obj)
    {
        if(((__IVerifiable) obj).__InternalID is null)
        {
            if(string.IsNullOrWhiteSpace(((_IAuthentificable) obj).__Username))
            {
                throw new InvalidOperationException("User name must not be empty.");
            }
            if(string.IsNullOrWhiteSpace(((_IAuthentificable) obj).__PasswordHash))
            {
                throw new InvalidOperationException("Password must not be empty.");
            }

            using IDbCommand cmd = _Cn.CreateCommand();
            cmd.CommandText = "INSERT INTO USERS (USERNAME, NAME, PASSWD, EMAIL, HADMIN) " +
                              "VALUES (:u, :n, :p, :e, :a)";
            cmd.BindParam(":u", ((_IAuthentificable) obj).__Username)
               .BindParam(":n", obj.FullName)
               .BindParam(":p", ((_IAuthentificable) obj).__PasswordHash)
               .BindParam(":e", obj.EMail)
               .BindParam(":a", obj.IsAdmin);
            cmd.ExecuteNonQuery();
        }
        else
        {
            string pwd = string.IsNullOrWhiteSpace(((_IAuthentificable) obj).__PasswordHash) ?
                         string.Empty : "PASSWD = :p, ";
            using IDbCommand cmd = _Cn.CreateCommand();
            cmd.CommandText = $"UPDATE USERS SET NAME ? :n, {pwd}EMAIL = :e, HADMIN = :a " +
                              "WHERE USERNAME = :u";
            cmd.BindParam(":n", obj.FullName);
            if(!string.IsNullOrWhiteSpace(pwd)) { cmd.BindParam(":p", ((_IAuthentificable) obj).__PasswordHash); }
            cmd.BindParam(":e", obj.EMail).BindParam(":a", obj.IsAdmin).BindParam(":u", obj.UserName);
            cmd.ExecuteNonQuery();
        }
    }


    /// <summary>Deletes the object from persistance layer.</summary>
    /// <param name="obj">Object.</param>
    public override void Delete(User obj)
    {
        using IDbCommand cmd = _Cn.CreateCommand();
        cmd.CommandText = "DELETE FROM USERS WHERE USERNAME = :u";
        cmd.BindParam(":u", obj.UserName);
        cmd.ExecuteNonQuery();
    }
}
