using System.Collections;
using System.Data;
using System.Data.SQLite;

using FHTW.Swen1.Forum.System;

namespace FHTW.Swen1.Forum.Repositories;


public abstract class Repository<T>: IRepository<T>, IRepository where T: IAtom, __IVerifiable, new()
{
    private static IDbConnection? _DbConnection;
    

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


    protected virtual T _CreateObject(IDataReader re)
    {
        T rval = new();
        ((__IVerifiable) rval).__InternalID = re.GetInt("ID");
        
        return _RefreshObject(re, rval);
    }

    protected abstract T _RefreshObject(IDataReader re, T rval);


    public abstract T? Get(object id, Session? session = null);

    public abstract IEnumerable<T> GetAll(Session? session = null);

    public abstract void Refresh(T obj);

    public abstract void Save(T obj);

    public abstract void Delete(T obj);


    object? IRepository.Get(object id, Session? session)
    {
        return Get(id, session);
    }

    IEnumerable IRepository.GetAll(Session? session)
    {
        return GetAll(session);
    }

    void IRepository.Refresh(object obj)
    {
        Refresh((T) obj);
    }

    void IRepository.Save(object obj)
    {
        Save((T) obj);
    }

    void IRepository.Delete(object obj)
    {
        Delete((T) obj);
    }
}
