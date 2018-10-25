using Neo.Collections;
using NUnit.Framework;
using Neo.Network.Http;

namespace Tests.Neo.Network.Http {
  [TestFixture]
  public class TestResponse {
    private readonly CookieJar jar = new CookieJar();
    private readonly Dictionary<string,string> headers = new Dictionary<string, string>();

    [Test]
    public void Constructor() {
      Response resp = new Response("body", 200, "OK", headers, jar);
      Assert.AreEqual("body", resp.Body);
      Assert.AreEqual("OK", resp.Message);
      Assert.AreEqual(200, resp.Status);
      Assert.AreSame(headers, resp.Headers);
      Assert.AreSame(jar, resp.Cookies);
    }

    [Test]
    public void IsSuccess() {
      Response resp = new Response("body", 200, "OK", headers, jar);
      Assert.IsTrue(resp.IsSuccess);
      Response other = new Response("body", 404, "NotFound", headers, jar);
      Assert.IsFalse(other.IsSuccess);
    }

    [Test]
    public void BuildError() {
      Response resp =  Response.BuildError("message");
      Assert.AreEqual("", resp.Body);
      Assert.AreEqual("message", resp.Message);
      Assert.AreEqual(409, resp.Status);
      Assert.NotNull(resp.Cookies);
      Assert.IsTrue(resp.Headers.IsEmpty);
    }
  }
}
