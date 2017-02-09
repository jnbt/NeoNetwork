using NUnit.Framework;
using Neo.Network;

namespace Tests.Neo.Network {
  [TestFixture]
  public class TestUserAgentBuilder {
    private UserAgentBuilder subject;

    [SetUp]
    public void SetUp() {
      subject = new UserAgentBuilder();
    }

    [Test]
    public void AsString() {
      Assert.AreEqual(string.Empty, subject.ToString());

      subject.Append("Unity");
      Assert.AreEqual("Unity", subject.ToString());

      subject.Append("Adrenalyn", "1.1.1");
      Assert.AreEqual("Unity Adrenalyn/1.1.1", subject.ToString());

      subject.Append("Foo", "3", "Only Testing");
      Assert.AreEqual("Unity Adrenalyn/1.1.1 Foo/3 (Only Testing)", subject.ToString());

      subject.Append("Bar", comment: "Without Version");
      Assert.AreEqual("Unity Adrenalyn/1.1.1 Foo/3 (Only Testing) Bar (Without Version)", subject.ToString());
    }

    [Test]
    public void RealWorldExample() {
      UserAgentBuilder uab = new UserAgentBuilder("Unity", "2.1.0", "iOS")
        .Append("Unity", "5.2.3")
        .Append("mobile_hires");
      Assert.AreEqual("Unity/2.1.0 (iOS) Unity/5.2.3 mobile_hires", uab.ToString());
    }
  }
}
