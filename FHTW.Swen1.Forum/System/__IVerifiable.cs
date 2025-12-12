namespace FHTW.Swen1.Forum.System;



public interface __IVerifiable
{
    public object? __InternalID { get; set; }

    public void __VerifySession(Session? session = null);

    public void __EndEdit();

    public void __EnsureAdmin();

    public void __EnsureAdminOrOwner(string? owner);
}
