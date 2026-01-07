namespace FHTW.Swen1.Forum.Tests;

using global::System.Data;
using global::System.Reflection;

using FHTW.Swen1.Forum.Repositories;
using FHTW.Swen1.Forum.System;

using NSubstitute;



[TestFixture]
public class UserTest
{
    private IDbConnection _Cn;
    private IDbCommand _Cmd;
    private IDataReader _Re;


    [SetUp]
    public void Setup()
    {
        _Cn = Substitute.For<IDbConnection>();
        _Cmd = Substitute.For<IDbCommand>();
        _Re = Substitute.For<IDataReader>();

        _Cn.CreateCommand().Returns(_Cmd);
        _Cmd.ExecuteReader().Returns(_Re);
        
        _Re.Read().Returns(true, false);

        _Re.GetString(0).Returns("test");
        _Re.GetString(1).Returns("test");
        _Re.GetString(2).Returns("test@test.test");
        _Re.GetBoolean(3).Returns(true);

        typeof(Repository<User>).GetField("_DbConnection", BindingFlags.Static | BindingFlags.NonPublic)!.SetValue(null, _Cn);
    }


    [Test]
    public void Test()
    {
        User? u = User.Logon("test", "test");

        Assert.That(u, Is.Not.Null);
        Assert.That(u!.UserName, Is.EqualTo("test"));

        _Cmd.Received().CommandText = "SELECT USERNAME, NAME, EMAIL, HADMIN FROM USERS WHERE USERNAME = :u AND PASSWD = :p";

        Received.InOrder(() =>
        {
            _Cn.CreateCommand();
            _Cmd.ExecuteReader();
            _Re.Read();
        });

        _Cmd.Received(0).ExecuteNonQuery();
        _Cmd.Received().Dispose();
    }


    [TearDown]
    public void Cleanup()
    {
        typeof(Repository<User>).GetField("_DbConnection", BindingFlags.Static | BindingFlags.NonPublic)!.SetValue(null, null);

        _Cn.Dispose();
        _Cmd.Dispose();
        _Re.Dispose();
    }
}
