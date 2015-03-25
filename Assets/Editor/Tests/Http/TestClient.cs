using NUnit.Framework;
using System.Net;
using Neo.Network.Http;
using Neo.Collections;

namespace Tests.Neo.Neowork.Http{
  [TestFixture]
  public class TestClient{

    private class DummyPerformer : IRequestPerformer{
      public Request Request{ get; private set; }
      public FinishCallback Callback{ get; private set; }
      public void Perform(Request request, FinishCallback callback){
        this.Request = request;
        this.Callback = callback;
      }
    }

    private class DummyFactory : IRequestPerformerFactory{
      public DummyPerformer Performer{ get; set; }
      public IRequestPerformer Build(){
        return Performer;
      }
    }

    private DummyPerformer performer;
    private DummyFactory   factory;
    private Client         client;

    private readonly string url             = "http://www.neopoly.com";
    private readonly Response dummyResponse = new Response("body", HttpStatusCode.OK, "message", null);

    [SetUp]
    public void SetUp(){
      performer = new DummyPerformer();
      factory   = new DummyFactory(){ Performer = performer };
      client    = new Client(){
        PerformerFactory = factory,
        Cookies          = new CookieJar()
      };
    }

    [Test]
    public void EmptyConstructor(){
      Assert.NotNull(client.Cookies);
    }

    [Test]
    public void GetSimple(){
      Response response = null;
      client.Get(url, (r) => response = r);

      Assert.AreSame(url, performer.Request.Url);
      Assert.IsNull(performer.Request.Parameters);
      Assert.IsNull(performer.Request.Headers);
      Assert.AreEqual(HttpMethod.GET, performer.Request.Method);
      performer.Callback(dummyResponse);
      Assert.AreSame(dummyResponse, response);
    }

    [Test]
    public void GetWithParameters() {
      Response response = null;
      Dictionary<string, string> p = new Dictionary<string, string>(){
        {"p1", "1"},
        {"p2", "2"}
      };

      client.Get(url, p, (r) => response = r);
      Assert.AreSame(url, performer.Request.Url);
      Assert.AreSame(p, performer.Request.Parameters);
      Assert.IsNull(performer.Request.Headers);
      Assert.AreEqual(HttpMethod.GET, performer.Request.Method);
      performer.Callback(dummyResponse);
      Assert.AreSame(dummyResponse, response);
    }

    [Test]
    public void GetWithParametersAndHeaders() {
      Response response = null;
      Dictionary<string, string> p = new Dictionary<string, string>(){
        {"p1", "1"},
        {"p2", "2"}
      };
      Dictionary<string, string> h = new Dictionary<string, string>(){
        {"h1", "1"},
        {"h2", "2"}
      };

      client.Get(url, p, h, (r) => response = r);
      Assert.AreSame(url, performer.Request.Url);
      Assert.AreSame(p, performer.Request.Parameters);
      Assert.AreSame(h, performer.Request.Headers);
      Assert.AreEqual(HttpMethod.GET, performer.Request.Method);
      performer.Callback(dummyResponse);
      Assert.AreSame(dummyResponse, response);
    }

    [Test]
    public void PostSimple() {
      Response response = null;
      Dictionary<string, string> p = new Dictionary<string, string>(){
        {"p1", "1"},
        {"p2", "2"}
      };

      client.Post(url, p, (r) => response = r);

      Assert.AreSame(url, performer.Request.Url);
      Assert.IsNull(performer.Request.Body);
      Assert.AreSame(p, performer.Request.Parameters);
      Assert.IsNull(performer.Request.Headers);
      Assert.AreEqual(HttpMethod.POST, performer.Request.Method);
      performer.Callback(dummyResponse);
      Assert.AreSame(dummyResponse, response);
    }

    [Test]
    public void PostWithHeaders() {
      Response response = null;
      Dictionary<string, string> p = new Dictionary<string, string>(){
        {"p1", "1"},
        {"p2", "2"}
      };
      Dictionary<string, string> h = new Dictionary<string, string>(){
        {"h1", "1"},
        {"h2", "2"}
      };

      client.Post(url, p, h, (r) => response = r);

      Assert.AreSame(url, performer.Request.Url);
      Assert.IsNull(performer.Request.Body);
      Assert.AreSame(p, performer.Request.Parameters);
      Assert.AreSame(h, performer.Request.Headers);
      Assert.AreEqual(HttpMethod.POST, performer.Request.Method);
      performer.Callback(dummyResponse);
      Assert.AreSame(dummyResponse, response);
    }

    [Test]
    public void PostBody() {
      Response response = null;
      byte[] data = new byte[0];

      client.Post(url, data, (r) => response = r);

      Assert.AreSame(url, performer.Request.Url);
      Assert.AreSame(data, performer.Request.Body);
      Assert.IsNull(performer.Request.Headers);
      Assert.AreEqual(HttpMethod.POST, performer.Request.Method);
      performer.Callback(dummyResponse);
      Assert.AreSame(dummyResponse, response);
    }

    [Test]
    public void PostBodyWithHeaders() {
      Response response = null;
      byte[] data = new byte[0];
      Dictionary<string, string> h = new Dictionary<string, string>(){
        {"h1", "1"},
        {"h2", "2"}
      };


      client.Post(url, data, h, (r) => response = r);

      Assert.AreSame(url, performer.Request.Url);
      Assert.AreSame(data, performer.Request.Body);
      Assert.AreSame(h, performer.Request.Headers);
      Assert.AreEqual(HttpMethod.POST, performer.Request.Method);
      performer.Callback(dummyResponse);
      Assert.AreSame(dummyResponse, response);
    }
  }
}
