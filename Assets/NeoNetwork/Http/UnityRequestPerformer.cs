using System.Collections;
using Neo.Async;
using Neo.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Neo.Network.Http {
  /// <summary>
  /// A HTTP request performer using Unity's build in WWW class
  /// </summary>
  public sealed class UnityRequestPerformer : IRequestPerformer {
    private const string CookieHeaderField = "Cookie";

    private Request request;
    private FinishCallback callback;

    /// <summary>
    /// Perform the actual HTTP request
    /// </summary>
    /// <param name="request">to perform</param>
    /// <param name="callback">to call once finished</param>
    public void Perform(Request request, FinishCallback callback) {
      this.request = request;
      this.callback = callback;

      UnityWebRequest www = buildUnityWebRequest(request);
      if(www != null) startPerformRequest(www, request, callback);
    }

    private static void startPerformRequest(UnityWebRequest www, Request request, FinishCallback callback) {
      CoroutineStarter.Instance.Add(waitForRequest(www, request, callback));
    }

    private static IEnumerator waitForRequest(UnityWebRequest www, Request request, FinishCallback callback) {
      using(www) {
        yield return www.SendWebRequest();

        if(string.IsNullOrEmpty(www.error)) {
          Dictionary<string, string> headers = new Dictionary<string, string>(www.GetResponseHeaders());
          if(request.Cookies != null) request.Cookies.Update(headers);
          int code = (int) www.responseCode;
          UnityWebRequest.ClearCookieCache();
          callback(new Response(www.downloadHandler.text, code, StatusCodes.MessageFromCode(code), headers, request.Cookies));
        } else {
          callback(Response.BuildError(www.error));
        }
      }
    }

    private static UnityWebRequest buildUnityWebRequest(Request request) {
      UnityWebRequest www = request.Method == HttpMethod.GET ? buildGetRequest(request) : buildPostRequest(request);
      addHeaders(www, request);
      return www;
    }

    private static UnityWebRequest buildGetRequest(Request request) {
      return UnityWebRequest.Get(request.Url);
    }

    private static UnityWebRequest buildPostRequest(Request request) {
      if(request.Body == null) return UnityWebRequest.Post(request.Url, request.Parameters);
      UnityWebRequest www = UnityWebRequest.Put(request.Url, request.Body);
      www.method = request.Method.ToString();
      return www;
    }

    private static void addHeaders(UnityWebRequest www, Request request) {
      if (request.Cookies != null && !request.Cookies.IsEmpty && !isWebGL) {
        www.SetRequestHeader(CookieHeaderField, request.Cookies.ToString());
      }

      if (request.Headers != null) {
        request.Headers.ForEach(www.SetRequestHeader);
      }
    }

    private static bool isWebGL {
      get {
        return Application.platform == RuntimePlatform.WebGLPlayer;
      }
    }
  }
}
