using NUnit.Framework;
using Neo.Collections;
using Neo.Network.Http;

namespace Tests.Neo.Network.Http {
  [TestFixture]
  public class TestCookieJar {

    [Test]
    public void StartsEmpty() {
      CookieJar jar = new CookieJar();
      Assert.IsTrue(jar.IsEmpty);
    }

    [Test]
    public void UpdateFromAllHeaderVariants() {
      CookieJar jar = new CookieJar();
      Dictionary<string,string> headers = new Dictionary<string, string> {
        {"Set-Cookie", "A1=B1; C1=D1 "},
        {"SET-COOKIE", "A2=B2;C2=D2"},
        {"set-cookie", "A3=B3"},
        {"Set-cookie", "A4=B4; C4=D4 "},
      };
      jar.Update(headers);
      Assert.AreEqual(7, jar.Store.Count);
      Assert.AreEqual("B1", jar.Store["A1"]);
      Assert.AreEqual("D1 ", jar.Store[" C1"]);
      Assert.AreEqual("B2", jar.Store["A2"]);
      Assert.AreEqual("B3", jar.Store["A3"]);
      Assert.AreEqual("B4", jar.Store["A4"]);
      Assert.AreEqual("D4 ", jar.Store[" C4"]);
    }

    [Test]
    public void ClearAndIsEmpty() {
      CookieJar jar = new CookieJar();
      jar.Store["a"] = "b";
      Assert.IsFalse(jar.IsEmpty);
      jar.Clear();
      Assert.IsTrue(jar.IsEmpty);
    }

    [Test]
    public void BuildWithToString() {
      CookieJar jar = new CookieJar();
      jar.Store["A1"] = "B1";
      jar.Store[" C1"] = "D1";
      jar.Store[" C4 "] = " D4 ";

      Assert.AreEqual("A1=B1; C1=D1; C4 = D4 ", jar.ToString());
    }
  }
}
