namespace FHTW.Swen1.Forum.System;

using global::System.Security.Cryptography;
using global::System.Text;

using FHTW.Swen1.Forum.Repositories;



/// <summary>This class represents a system user.</summary>
public sealed class User: Atom, IAtom, __IVerifiable, _IAuthentificable
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // private static members                                                                                           //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>User repository.</summary>
    private static UserRepository _Repository = new();



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // constructors                                                                                                     //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Creates a new instance of this class.</summary>
    /// <param name="session">Session.</param>
    public User(Session? session): base(session)
    {}


    /// <summary>Creates a new instance of this class.</summary>
    private User(): this(null)
    {}



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // private static methods                                                                                           //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Hashes a password.</summary>
    /// <param name="userName">User name.</param>
    /// <param name="password">Password.</param>
    /// <returns>Returns the password hash.</returns>
    internal static string? _HashPassword(string userName, string password)
    {
        if(string.IsNullOrWhiteSpace(password)) { return null; }

        StringBuilder rval = new();
        foreach(byte i in SHA256.HashData(Encoding.UTF8.GetBytes(userName + password)))
        {
            rval.Append(i.ToString("x2"));
        }
        return rval.ToString();
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public static methods                                                                                            //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Gets a user by user name.</summary>
    /// <param name="userName">User name.</param>
    /// <param name="session">Session.</param>
    /// <returns>Returns a user object or NULL if not found.</returns>
    public static User? Get(string userName, Session? session = null)
    {
        return _Repository.Get(userName, session);
    }


    /// <summary>Performs a user logon operation.</summary>
    /// <param name="username">User name.</param>
    /// <param name="password">Password.</param>
    /// <returns>Returns the user logged on or NULL if the operation was unsuccessful.</returns>
    public static User? Logon(string username, string password)
    {
        return _Repository.Logon(username, password);
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public properties                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Gets or sets the user name.</summary>
    public string UserName
    {
        get { return ((string?) ((__IVerifiable) this).__InternalID) ?? string.Empty; }
        set 
        {
            if(_InternalID is not null) 
            { 
                throw new InvalidOperationException("User name cannot be changed."); 
            }
            if(string.IsNullOrWhiteSpace(value)) { throw new ArgumentException("User name must not be empty."); }
            
            ((_IAuthentificable) this).__Username = value;
            ((_IAuthentificable) this).__PasswordHash = null;
        }
    }


    /// <summary>Gets the full name for the user.</summary>
    public string FullName
    {
        get; set;
    } = string.Empty;


    /// <summary>Gets the e-mail address for the user.</summary>

    public string EMail
    {
        get; set;
    } = string.Empty;


    /// <summary>Gets or sets if the user has administrative privileges.</summary>
    public bool IsAdmin 
    {
        get; set; 
    }


    
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public methods                                                                                                   //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Sets the user password.</summary>
    /// <param name="password">Password.</param>
    public void SetPassword(string password)
    {
        string? uname = (string?) _InternalID ?? ((_IAuthentificable) this).__Username;
        
        if(string.IsNullOrWhiteSpace(uname))
        {
            throw new InvalidOperationException("User name must be set before setting the password.");
        }

        ((_IAuthentificable) this).__PasswordHash = _HashPassword(uname, password);
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
        if(IsAdmin) { _EnsureAdmin(); }
        base.Save();
    }


    /// <summary>Deletes the object.</summary>
    public override void Delete()
    {
        _EnsureAdminOrOwner(UserName);
        base.Delete();
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [interface] _IAuthentificable                                                                                    //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Gets or sets the object user name.</summary>
    string? _IAuthentificable.__Username
    {
        get; set;
    }


    /// <summary>Gets or sets the object password hash.</summary>
    string? _IAuthentificable.__PasswordHash
    {
        get; set;
    }
}
