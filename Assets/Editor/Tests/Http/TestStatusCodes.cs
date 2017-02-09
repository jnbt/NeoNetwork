using Neo.Network.Http;
using NUnit.Framework;

namespace Tests.Neo.Network.Http {
  [TestFixture]
  public class TestStatusCodes {
    [Test]
    public void ReturnsUnknownAsFallback() {
      Assert.AreEqual("Unknown", StatusCodes.MessageFromCode(999));
    }

    [Test]
    public void ReturnsKnownStatusCodes() {
      Assert.AreEqual("OK", StatusCodes.MessageFromCode(200));
    }
  }
}
