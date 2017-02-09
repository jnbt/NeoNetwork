using UnityEngine;
using Neo.Async;
using Neo.Collections;

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

      WWW www = buildWWW();
      if(www != null) {
#if !UNITY_WEBGL
        www.threadPriority = ThreadPriority.Low;
#endif
        performWWW(www);
      }
    }

    private void performWWW(WWW www) {
      CoroutineStarter.Instance.Add(waitForRequest(www, request, callback));
    }

    private System.Collections.IEnumerator waitForRequest(WWW www, Request request, FinishCallback callback) {
      using(www) {
        yield return www;

        if(string.IsNullOrEmpty(www.error)) {
          Dictionary<string, string> headers = new Dictionary<string, string>(www.responseHeaders);
          if(request.Cookies != null) request.Cookies.Update(headers);
          callback(new Response(www.text, 200, "OK", headers, request.Cookies));
        } else {
          callback(Response.BuildError(www.error));
        }
      }
    }

    private WWW buildWWW() {
      if(request.Method == HttpMethod.GET) return buildWWWGet();
      if(request.Method == HttpMethod.POST) return buildWWWPost();
      return null;
    }

    private WWW buildWWWGet() {
      return new WWW(buildGetUrl(), null, buildHeaders());
    }

    private WWW buildWWWPost() {
      if(request.Body == null) return new WWW(request.Url, buildPostForm().data, buildHeaders());
      else return new WWW(request.Url, request.Body, buildHeaders());
    }

    private System.Collections.Generic.Dictionary<string, string> buildHeaders() {
      System.Collections.Generic.Dictionary<string, string> table = new System.Collections.Generic.Dictionary<string, string>();
      if(request.Cookies != null && !request.Cookies.IsEmpty) {
        table[CookieHeaderField] = request.Cookies.ToString();
      }
      if(request.Headers != null) {
        request.Headers.ForEach((key, value) => table[key] = value);
      }
      return table;
    }

    private string buildGetUrl() {
      if(request.Parameters == null || request.Parameters.IsEmpty) return request.Url;
      UriBuilder b = new UriBuilder(request.Url);
      b.AddParams(request.Parameters);
      return b.ToString();
    }

    private WWWForm buildPostForm() {
      WWWForm form = new WWWForm();
      request.Parameters.ForEach((key, value) => {
        if(!string.IsNullOrEmpty(key)) {
          form.AddField(key, value ?? string.Empty);
        }
      });
      return form;
    }
  }
}
