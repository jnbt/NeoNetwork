using NUnit.Framework;
using System.Net;
using Neo.Network.Http;

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
      performer.Callback(dummyResponse);
      Assert.AreSame(dummyResponse, response);
    }
  }
}