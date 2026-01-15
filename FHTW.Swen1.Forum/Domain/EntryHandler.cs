using System.Net;
using System.Text.Json.Nodes;

using FHTW.Swen1.Forum.Handlers;
using FHTW.Swen1.Forum.Server;

namespace FHTW.Swen1.Forum.Domain;


/// <summary>This class implements a handler for entries.</summary>
public sealed class EntryHandler: Handler, IHandler
{
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [override] Handler                                                                                               //
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>Handles an incoming HTTP request.</summary>
    /// <param name="e">Event arguments.</param>
    public override void Handle(HttpRestEventArgs e)
    {
        if(e.Path.TrimEnd('/', ' ', '\t') == "/entries" && e.Method == HttpMethod.Post)
        {
            _Create(e);
        }
        else if(e.Path.StartsWith("/entries/"))
        {
            if(e.Method == HttpMethod.Get)
            {
                _Query(e);
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

    /// <summary>Creates an entry.</summary>
    /// <param name="e">Event arguments.</param>
    private static void _Create(HttpRestEventArgs e)
    {
        JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
        HttpStatusCode status = HttpStatusCode.BadRequest;

        try
        {
            Entry en = new(e.Session, Thread.Get((int?) e.Content["thread"] ?? -1)!) { Text = (string?) e.Content["text"] ?? string.Empty };
            en.Save();

            status = HttpStatusCode.OK;
            reply = new JsonObject() { ["success"] = true, ["id"] = en.ID };
        }
        catch(Exception ex)
        {
            reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
        }

        e.Respond(status, reply);
    }


    /// <summary>Edits an entry.</summary>
    /// <param name="e">Event arguments.</param>
    private static void _Edit(HttpRestEventArgs e)
    {
        JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
        HttpStatusCode status = HttpStatusCode.BadRequest;

        try
        {
            Entry? en = Entry.Get(Convert.ToInt32(e.Path[9..]));

            if(en is null)
            {
                status = HttpStatusCode.NotFound;
                reply = new JsonObject() { ["success"] = false, ["message"] = "Entry not found." };
            }
            else
            {
                en.BeginEdit(e.Session!);
                en.Text = (string?) e.Content["text"] ?? string.Empty;
                en.Save();

                status = HttpStatusCode.OK;
                reply = new JsonObject() { ["success"] = true, ["message"] = "Entry edited." };
            }
        }
        catch(Exception ex)
        {
            reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
        }

        e.Respond(status, reply);
    }


    /// <summary>Deletes an entry.</summary>
    /// <param name="e">Event arguments.</param>
    private static void _Delete(HttpRestEventArgs e)
    {
        JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
        HttpStatusCode status = HttpStatusCode.BadRequest;

        try
        {
            Entry? en = Entry.Get(Convert.ToInt32(e.Path[9..]));
            if(en is null)
            {
                status = HttpStatusCode.NotFound;
                reply = new JsonObject() { ["success"] = false, ["message"] = "Entry not found." };
            }
            else
            {
                en.BeginEdit(e.Session!);
                en.Delete();

                status = HttpStatusCode.OK;
                reply = new JsonObject() { ["success"] = true, ["message"] = "Entry deleted." };
            }
        }
        catch(Exception ex)
        {
            reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
        }

        e.Respond(status, reply);
    }


    /// <summary>Queries an entry.</summary>
    /// <param name="e">Event arguments.</param>
    private static void _Query(HttpRestEventArgs e)
    {
        JsonObject? reply = new JsonObject() { ["success"] = false, ["message"] = "Invalid request." };
        HttpStatusCode status = HttpStatusCode.BadRequest;

        try
        {
            Entry? en = Entry.Get(Convert.ToInt32(e.Path[9..]));

            if(en is null)
            {
                status = HttpStatusCode.NotFound;
                reply = new JsonObject() { ["success"] = false, ["message"] = "Entry not found." };
            }
            else
            {
                status = HttpStatusCode.OK;
                reply = new JsonObject() { ["success"] = true, ["id"] = en.ID, ["text"] = en.Text, ["owner"] = en.Owner, ["thread"] = en.Thread?.ID ?? -1, ["time"] = en.Time };
            }
        }
        catch(Exception ex)
        {
            reply = new JsonObject() { ["success"] = false, ["message"] = ex.Message };
        }

        e.Respond(status, reply);
    }
}
