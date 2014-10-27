using NUnit.Framework;
using Neo.Collections;
using Neo.Network;

namespace Tests.Neo.Network {
  [TestFixture]
  public class TestUriBuilder {
    [Test]
    public void TestParseQueryString() {
      string simple =  "a=b&c=d";
      string complex = "ignore?a=b%201&a=c+2&d=x";

      NameValueCollection c1 = UriBuilder.ParseQueryString(simple);
      Assert.AreEqual(2, c1.Count);
      Assert.AreEqual("b", c1["a"]);
      Assert.AreEqual("d", c1["c"]);

      NameValueCollection c2 = UriBuilder.ParseQueryString(complex);
      Assert.AreEqual(2, c2.Count);
      Assert.AreEqual("b 1,c 2", c2["a"]);
      Assert.AreEqual("x", c2["d"]);
    }

    [Test]
    public void StringifyQueryString() {
      NameValueCollection simple = new NameValueCollection();
      simple.Add("a", "b");
      simple.Add("c", "d");

      NameValueCollection complex = new NameValueCollection();
      complex.Add("a", "b 1");
      complex.Add("a", "c");
      complex.Add("d", "x");

      Assert.AreEqual("a=b&c=d", UriBuilder.ToQueryString(simple));
      Assert.AreEqual("a=b+1&a=c&d=x", UriBuilder.ToQueryString(complex));
    }

    [Test]
    public void GettersAndSetters() {
      UriBuilder b = new UriBuilder("http://user:pass@www.neopoly.de:8080/the/path/index.html?a=b#v");
      Assert.AreEqual("http", b.Scheme);
      Assert.AreEqual("www.neopoly.de", b.Host);
      Assert.AreEqual(8080, b.Port);
      Assert.AreEqual("/the/path/index.html", b.Path);
      Assert.AreEqual("?a=b", b.Query);
      Assert.AreEqual("#v", b.Fragment);
      Assert.AreEqual("http://user:pass@www.neopoly.de:8080/the/path/index.html?a=b#v", b.ToString());

      b.Scheme   = "ftp";
      b.Host     = "other.host.de";
      b.Port     = 3000;
      b.Path     = "/a/b/c";
      b.Query    = "c=1";
      b.Fragment = "foo";
      Assert.AreEqual("ftp", b.Scheme);
      Assert.AreEqual("other.host.de", b.Host);
      Assert.AreEqual(3000, b.Port);
      Assert.AreEqual("/a/b/c", b.Path);
      Assert.AreEqual("?c=1", b.Query);
      Assert.AreEqual("#foo", b.Fragment);
      Assert.AreEqual("ftp://user:pass@other.host.de:3000/a/b/c?c=1#foo", b.ToString());
    }

    [Test]
    public void ManipuateParameters() {
      UriBuilder b = new UriBuilder("http://user:pass@www.neopoly.de:8080/the/path/index.html?a=b#v");
      b.SetParam("a", "c");
      Assert.AreEqual("http://user:pass@www.neopoly.de:8080/the/path/index.html?a=c#v", b.ToString());
      b.AddParam("b", "x");
      Assert.AreEqual("http://user:pass@www.neopoly.de:8080/the/path/index.html?a=c&b=x#v", b.ToString());
      b.AddParam("b", "y");
      Assert.AreEqual("http://user:pass@www.neopoly.de:8080/the/path/index.html?a=c&b=x&b=y#v", b.ToString());

      b.Query = null;
      Assert.AreEqual("http://user:pass@www.neopoly.de:8080/the/path/index.html#v", b.ToString());
    }

    [Test]
    public void UrlEscapeAndUrlUnescape() {
      string s = "this is&escaped";
      Assert.AreEqual("this+is%26escaped", UriBuilder.UrlEscape(s));
      Assert.AreEqual(s, UriBuilder.UrlUnescape(UriBuilder.UrlEscape(s)));
    }
  }
}
