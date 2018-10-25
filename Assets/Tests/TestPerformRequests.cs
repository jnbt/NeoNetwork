using System.Collections;
using System.Text;
using Neo.Collections;
using Neo.Logging;
using Neo.Network.Http;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests {
  [TestFixture]
  public class TestPerformRequests {
    private IClient client;

    [SetUp]
    public void SetUp() {
      Logger.LogLevel = Level.LOG;
      client = new Client(new UnityRequestPerformerFactory());
    }

    [UnityTest]
    public IEnumerator TestGet() {
      Response response = null;
      var parameters = new Dictionary<string, string> {
        {"userId", "1"}
      };
      client.Get("https://jsonplaceholder.typicode.com/posts", parameters, resp => response = resp);
      while (response == null) yield return null;
      Assert.IsTrue(response.IsSuccess, "Response should be successful");
      Assert.AreEqual(200, response.Status, "Response should be 200");
      Assert.IsTrue(response.Body.StartsWith("["), "Response should contain JSON");
    }

    [UnityTest]
    public IEnumerator TestPost() {
      Response response = null;
      var parameters = new Dictionary<string, string> {
        {"title", "foo"}
      };
      client.Post("http://jsonplaceholder.typicode.com/posts", parameters, resp => response = resp);
      while (response == null) yield return null;
      Assert.IsTrue(response.IsSuccess, "Response should be successful");
      Assert.AreEqual(201, response.Status, "Response should be 200");
      Assert.IsTrue(response.Body.StartsWith("{"), "Response should contain JSON");
    }

    [UnityTest]
    public IEnumerator TestJsonPost() {
      Response response = null;
      var json = "{\"title\": \"foo\"}";
      var body = Encoding.UTF8.GetBytes(json);
      var headers = new Dictionary<string, string>() {
        {"Content-Type", "application/json"}
      };
      client.Post("http://jsonplaceholder.typicode.com/posts", body, headers, resp => response = resp);
      while (response == null) yield return null;
      Assert.IsTrue(response.IsSuccess, "Response should be successful");
      Assert.AreEqual(201, response.Status, "Response should be 200");
      Assert.IsTrue(response.Body.StartsWith("{"), "Response should contain JSON");
    }
  }
}
