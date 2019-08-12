using NUnit.Framework;
using Neo.Network.Http;

namespace Tests.Neo.Network.Http {
  [TestFixture]
  public class TestHttpMethod {

    [Test]
    public void Get() {
      Assert.AreEqual("GET", HttpMethod.GET.ToString());
    }

    [Test]
    public void Post() {
      Assert.AreEqual("POST", HttpMethod.POST.ToString());
    }
  }
}
