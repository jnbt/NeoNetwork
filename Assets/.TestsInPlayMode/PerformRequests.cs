using Neo.Collections;
using Neo.Logging;
using Neo.Network.Http;
using UnityEngine;
using Logger = Neo.Logging.Logger;

public class PerformRequests : MonoBehaviour {
  public class AssertionException : System.Exception {
    public AssertionException(string message) : base(message) { }
  }

  private IClient client;

  void Awake() {
    Logger.LogLevel = Level.LOG;
    client = new Client(new UnityRequestPerformerFactory());
    testGet();
  }

  private void testGet() {
    var parameters = new Dictionary<string, string> {
      { "userId", "1" }
    };
    client.Get("https://jsonplaceholder.typicode.com/posts", parameters, response => {
      if(!response.IsSuccess) throw new AssertionException("Request should be successful");
      if(response.Status != 200) throw new AssertionException("Should have a status code 200");
      if(!response.Body.StartsWith("[")) throw new AssertionException("Request should be return JSON");

      testPost();
    });
  }

  private void testPost() {
    var parameters = new Dictionary<string, string> {
      { "title", "foo" }
    };
    client.Post("http://jsonplaceholder.typicode.com/posts", parameters, response => {
      if(!response.IsSuccess) throw new AssertionException("Request should be successful");
      if(response.Status != 201) throw new AssertionException("Should have a status code 200");
      if(!response.Body.StartsWith("{")) throw new AssertionException("Request should be return JSON");

      testJsonPost();
    });
  }

  private void testJsonPost(){
    var json = "{\"title\": \"foo\"}";
    var body = System.Text.UTF8Encoding.UTF8.GetBytes(json);
    var headers = new Dictionary<string, string>(){
      { "Content-Type", "application/json" }
    };
    client.Post("http://jsonplaceholder.typicode.com/posts", body, headers, response => {
      if(!response.IsSuccess) throw new AssertionException("Request should be successful");
      if(response.Status != 201) throw new AssertionException("Should have a status code 200");
      if(!response.Body.StartsWith("{")) throw new AssertionException("Request should be return JSON");
    });
  }
}
