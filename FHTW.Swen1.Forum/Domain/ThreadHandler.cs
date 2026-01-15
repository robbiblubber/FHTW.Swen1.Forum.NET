namespace FHTW.Swen1.Forum.Domain;

using global::System.Net;
using global::System.Text.Json.Nodes;

using FHTW.Swen1.Forum.Handlers;
using FHTW.Swen1.Forum.Server;



/// <summary>This class implements a handler for threads.</summary>
public sealed class ThreadHandler: Handler, IHandler
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [override] Handler                                                                                               //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Handles an incoming HTTP request.</summary>
    /// <param name="e">Event arguments.</param>
    public override void Handle(HttpRestEventArgs e)
    {
        if(e.Path.TrimEnd('/', ' ', '\t') == "/threads" && e.Method == HttpMethod.Post)
        {
            _Create(e);
        }
        else if(e.Path.StartsWith("/threads/"))
        {
            if(e.Method == HttpMethod.Get)
            {
                if(e.Path.TrimEnd('/').EndsWith("entries"))
                {
                    _GetEntries(e);
                }
                else { _Query(e); }
            }
            else if(e.Method == HttpMethod.Put)
            {
                _Edit(e);
            }
            else if(e.Method == HttpMethod.Delete) 
            {
                _Delete(e);
            }
        }
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // private static methods                                                                                           //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Creates a thread.</summary>
    /// <param name="e">Event arguments.</param>
    private static void _Create(HttpRestEventArgs e)
    {
        JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
        HttpStatusCode status = HttpStatusCode.BadRequest;

        try
        {
            Thread th = new(e.Session) { Title = (string?) e.Content["title"] ?? string.Empty };
            th.Save();

            status = HttpStatusCode.OK;
            reply = new JsonObject() { ["success"] = true, ["id"] = th.ID };
        }
        catch(Exception ex)
        {
            reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
        }
            
        e.Respond(status, reply);
    }


    /// <summary>Edits a thread.</summary>
    /// <param name="e">Event arguments.</param>
    private static void _Edit(HttpRestEventArgs e)
    {
        JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
        HttpStatusCode status = HttpStatusCode.BadRequest;

        try
        {
            Thread? th = Thread.Get(Convert.ToInt32(e.Path[9..]));

            if(th == null)
            {
                status = HttpStatusCode.NotFound;
                reply = new JsonObject() { ["success"] = false, ["message"] = "Thread not found." };
            }
            else
            {
                th.BeginEdit(e.Session!);

                th.Title = (string?) e.Content["title"] ?? string.Empty;
                th.Save();

                status = HttpStatusCode.OK;
                reply = new JsonObject() { ["success"] = true, ["message"] = "Thread edited." };
            }
        }
        catch(Exception ex)
        {
            reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
        }

        e.Respond(status, reply);
    }


    /// <summary>Deletes a thread.</summary>
    /// <param name="e">Event arguments.</param>
    private static void _Delete(HttpRestEventArgs e)
    {
        JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
        HttpStatusCode status = HttpStatusCode.BadRequest;

        try
        {
            Thread? th = Thread.Get(Convert.ToInt32(e.Path[9..]));
            if(th == null)
            {
                status = HttpStatusCode.NotFound;
                reply = new JsonObject() { ["success"] = false, ["message"] = "Thread not found." };
            }
            else
            {
                th.BeginEdit(e.Session!);
                th.Delete();

                status = HttpStatusCode.OK;
                reply = new JsonObject() { ["success"] = true, ["message"] = "Thread deleted." };
            }
        }
        catch(Exception ex)
        {
            reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
        }

        e.Respond(status, reply);
    }


    /// <summary>Queries a thread.</summary>
    /// <param name="e">Event arguments.</param>
    private static void _Query(HttpRestEventArgs e)
    {
        JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
        HttpStatusCode status = HttpStatusCode.BadRequest;

        try
        {
            Thread? th = Thread.Get(Convert.ToInt32(e.Path[9..]));
            if(th is null)
            {
                status = HttpStatusCode.NotFound;
                reply = new JsonObject() { ["success"] = false, ["message"] = "Thread not found." };
            }
            else
            {
                status = HttpStatusCode.OK;
                reply = new JsonObject() { ["success"] = true, ["id"] = th.ID, ["title"] = th.Title, ["owner"] = th.Owner, ["time"] = th.Time };
            }
        }
        catch(Exception ex)
        {
            reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
        }

        e.Respond(status, reply);
    }


    /// <summary>Gets the entries for a thread.</summary>
    /// <param name="e">Event arguments.</param>
    private static void _GetEntries(HttpRestEventArgs e)
    {
        JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
        HttpStatusCode status = HttpStatusCode.BadRequest;

        try
        {
            Thread? th = Thread.Get(Convert.ToInt32(e.Path.TrimEnd('/')[..^8][9..]));
            if(th is null)
            {
                status = HttpStatusCode.NotFound;
                reply = new JsonObject() { ["success"] = false, ["message"] = "Thread not found." };
            }
            else
            {
                status = HttpStatusCode.OK;
                JsonArray arr = new JsonArray();
                foreach(Entry i in th.Entries)
                {
                    arr.Add(new JsonObject() { ["id"] = i.ID, ["text"] = i.Text, ["owner"] = i.Owner, ["thread"] = th.ID, ["time"] = i.Time });
                }
                reply = new JsonObject() { ["success"] = true, ["elements"] = arr };
            }
        }
        catch(Exception ex)
        {
            reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
        }

        e.Respond(status, reply);
    }
}