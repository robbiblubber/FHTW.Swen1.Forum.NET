namespace FHTW.Swen1.Forum.Repositories;

using global::System.Data;



/// <summary>This class provides extension methods for database operations in repositories.</summary>
internal static class _RepositoryExtensions
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // extension methods                                                                                                //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /// <summary>Binds a parameter and value to a command object.</summary>
    /// <param name="cmd">Command.</param>
    /// <param name="param">Parameter name.</param>
    /// <param name="value">Parameter value.</param>
    /// <returns>Returns the command object for chaining.</returns>
    public static IDbCommand BindParam(this IDbCommand cmd, string param, object? value)
    {
        IDataParameter p = cmd.CreateParameter();
        p.ParameterName = param;
        p.Value = value;
        cmd.Parameters.Add(p);

        return cmd;
    }


    /// <summary>Gets the string value for the field specified.</summary>
    /// <param name="re">Data reader.</param>
    /// <param name="fieldName">Field name.</param>
    /// <returns>Returns the string value for the field.</returns>
    public static string GetString(this IDataReader re, string fieldName)
    {
        int idx = re.GetOrdinal(fieldName);
        if(re.IsDBNull(idx)) { return string.Empty; }
        return re.GetString(idx);
    }


    /// <summary>Gets the integer value for the field specified.</summary>
    /// <param name="re">Data reader.</param>
    /// <param name="fieldName">Field name.</param>
    /// <returns>Returns the integer value for the field.</returns>
    public static int GetInt(this IDataReader re, string fieldName)
    {
        return re.GetInt32(re.GetOrdinal(fieldName));
    }


    /// <summary>Gets the Boolean value for the field specified.</summary>
    /// <param name="re">Data reader.</param>
    /// <param name="fieldName">Field name.</param>
    /// <returns>Returns the Boolean value for the field.</returns>
    public static bool GetBool(this IDataReader re, string fieldName)
    {
        return re.GetBoolean(re.GetOrdinal(fieldName));
    }


    /// <summary>Gets the DateTime value for the field specified.</summary>
    /// <param name="re">Data reader.</param>
    /// <param name="fieldName">Field name.</param>
    /// <returns>Returns the Boolean value for the field.</returns>
    public static DateTime GetDateTime(this IDataReader re, string fieldName)
    {
        return re.GetDateTime(re.GetOrdinal(fieldName));
    }
}
