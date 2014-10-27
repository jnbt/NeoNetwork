using NUnit.Framework;
using System.Net;
using Neo.Network.Http;

namespace Tests.Neo.Neowork.Http{
  [TestFixture]
  public class TestHttpMethod{

    [Test]
    public void Get(){
      Assert.AreEqual("GET", HttpMethod.GET.ToString());
    }

    [Test]
    public void Post(){
      Assert.AreEqual("POST", HttpMethod.POST.ToString());
    }
  }
}