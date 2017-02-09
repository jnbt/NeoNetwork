using Neo.Collections;
using Neo.Network.Http;
using NUnit.Framework;

namespace Tests.Neo.Network.Http {
  [TestFixture]
  public class TestClient {

    private class DummyPerformer : IRequestPerformer {
      public Request Request { get; private set; }
      public FinishCallback Callback { get; private set; }
      public void Perform(Request request, FinishCallback callback) {
        Request = request;
        Callback = callback;
      }
    }

    private class DummyFactory : IRequestPerformerFactory {
      public DummyPerformer Performer { get; set; }
      public IRequestPerformer Build() {
        return Performer;
      }
    }

    private DummyPerformer performer;
    private DummyFactory factory;
    private Client client;

    private const string Url = "http://www.neopoly.com";
    private static readonly Response DummyResponse = new Response(
      "body",
      200, "OK",
      new Dictionary<string, string>(),
      null
    );

    [SetUp]
    public void SetUp() {
      performer = new DummyPerformer();
      factory = new DummyFactory { Performer = performer };
      client = new Client(factory);
    }

    [Test]
    public void EmptyConstructor() {
      Assert.NotNull(client.Cookies);
    }

    [Test]
    public void GetSimple() {
      Response response = null;
      client.Get(Url, r => response = r);

      Assert.AreSame(Url, performer.Request.Url);
      Assert.IsNull(performer.Request.Parameters);
      Assert.IsNull(performer.Request.Headers);
      Assert.AreEqual(HttpMethod.GET, performer.Request.Method);
      performer.Callback(DummyResponse);
      Assert.AreSame(DummyResponse, response);
    }

    [Test]
    public void GetWithParameters() {
      Response response = null;
      Dictionary<string, string> p = new Dictionary<string, string> {
        {"p1", "1"},
        {"p2", "2"}
      };

      client.Get(Url, p, r => response = r);
      Assert.AreEqual(Url + "/?p1=1&p2=2", performer.Request.Url);
      Assert.IsNull(performer.Request.Parameters);
      Assert.IsNull(performer.Request.Headers);
      Assert.AreEqual(HttpMethod.GET, performer.Request.Method);
      performer.Callback(DummyResponse);
      Assert.AreSame(DummyResponse, response);
    }

    [Test]
    public void GetWithParametersAndHeaders() {
      Response response = null;
      Dictionary<string, string> p = new Dictionary<string, string> {
        {"p1", "1"},
        {"p2", "2"}
      };
      Dictionary<string, string> h = new Dictionary<string, string> {
        {"h1", "1"},
        {"h2", "2"}
      };

      client.Get(Url, p, h, r => response = r);
      Assert.AreEqual(Url + "/?p1=1&p2=2", performer.Request.Url);
      Assert.IsNull(performer.Request.Parameters);
      Assert.AreSame(h, performer.Request.Headers);
      Assert.AreEqual(HttpMethod.GET, performer.Request.Method);
      performer.Callback(DummyResponse);
      Assert.AreSame(DummyResponse, response);
    }

    [Test]
    public void PostSimple() {
      Response response = null;
      Dictionary<string, string> p = new Dictionary<string, string> {
        {"p1", "1"},
        {"p2", "2"}
      };

      client.Post(Url, p, r => response = r);

      Assert.AreSame(Url, performer.Request.Url);
      Assert.IsNull(performer.Request.Body);
      Assert.AreSame(p, performer.Request.Parameters);
      Assert.IsNull(performer.Request.Headers);
      Assert.AreEqual(HttpMethod.POST, performer.Request.Method);
      performer.Callback(DummyResponse);
      Assert.AreSame(DummyResponse, response);
    }

    [Test]
    public void PostWithHeaders() {
      Response response = null;
      Dictionary<string, string> p = new Dictionary<string, string> {
        {"p1", "1"},
        {"p2", "2"}
      };
      Dictionary<string, string> h = new Dictionary<string, string> {
        {"h1", "1"},
        {"h2", "2"}
      };

      client.Post(Url, p, h, r => response = r);

      Assert.AreSame(Url, performer.Request.Url);
      Assert.IsNull(performer.Request.Body);
      Assert.AreSame(p, performer.Request.Parameters);
      Assert.AreSame(h, performer.Request.Headers);
      Assert.AreEqual(HttpMethod.POST, performer.Request.Method);
      performer.Callback(DummyResponse);
      Assert.AreSame(DummyResponse, response);
    }

    [Test]
    public void PostBody() {
      Response response = null;
      byte[] data = new byte[0];

      client.Post(Url, data, r => response = r);

      Assert.AreSame(Url, performer.Request.Url);
      Assert.AreSame(data, performer.Request.Body);
      Assert.IsNull(performer.Request.Headers);
      Assert.AreEqual(HttpMethod.POST, performer.Request.Method);
      performer.Callback(DummyResponse);
      Assert.AreSame(DummyResponse, response);
    }

    [Test]
    public void PostBodyWithHeaders() {
      Response response = null;
      byte[] data = new byte[0];
      Dictionary<string, string> h = new Dictionary<string, string> {
        {"h1", "1"},
        {"h2", "2"}
      };

      client.Post(Url, data, h, r => response = r);

      Assert.AreSame(Url, performer.Request.Url);
      Assert.AreSame(data, performer.Request.Body);
      Assert.AreSame(h, performer.Request.Headers);
      Assert.AreEqual(HttpMethod.POST, performer.Request.Method);
      performer.Callback(DummyResponse);
      Assert.AreSame(DummyResponse, response);
    }
  }
}
