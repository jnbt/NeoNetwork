using NUnit.Framework;
using System.Net;
using Neo.Network.Http;

namespace Tests.Neo.Neowork.Http{
  [TestFixture]
  public class TestResponse{
    private CookieJar jar = new CookieJar();

    [Test]
    public void Constructor(){
      Response resp = new Response("body", HttpStatusCode.OK, "message", jar);
      Assert.AreEqual("body", resp.Body);
      Assert.AreEqual("message", resp.Message);
      Assert.AreEqual(HttpStatusCode.OK, resp.Status);
      Assert.AreSame(jar, resp.Cookies);
    }

    [Test]
    public void IsSuccess(){
      Response resp = new Response("body", HttpStatusCode.OK, "message", jar);
      Assert.IsTrue(resp.IsSuccess);
      Response other = new Response("body", HttpStatusCode.NotFound, "message", jar);
      Assert.IsFalse(other.IsSuccess);
    }

    [Test]
    public void BuildError(){
      Response resp =  Response.BuildError("message");
      Assert.AreEqual("", resp.Body);
      Assert.AreEqual("message", resp.Message);
      Assert.AreEqual(HttpStatusCode.Conflict, resp.Status);
      Assert.NotNull(resp.Cookies);
    }
  }
}