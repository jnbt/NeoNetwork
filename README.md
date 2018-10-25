# NeoNetwork: A class library very simple network operations for `Unity3D`

NeoNetwork is a class library to provide a simple access to Unity3D's networking stack.

## Installation

You can either use to copy the source files of this project into your Unity3D project or use Visual Studio to compile a DLL-file to be included in your project.

### Using Unity3D

* Clone the repository
* Copy the files from `Assets\NeoNetwork` into your project
  * This folder also includes an Assembly definition file

### Using VisualStudio

* Clone the repository
* Open `NeoNetwork.sln` with Visual Studio
* Build the solution using "Build -> Build NeoNetwork"
* Import the DLL (`obj/Release/NeoNetwork.dll`) into your Unity3D project

Hint: Unity currently always reset the LangVersion to "7.3" which isn't supported by Visual Studio. Therefor you need to manually
set / revert the `LangVersion` to `6` in `NeoNetwork.csproj`:

    <LangVersion>6</LangVersion>

## Dependencies

* [NeoCollections](https://github.com/jnbt/NeoCollections)
* [NeoLogging](https://github.com/jnbt/NeoLogging)
* [NeoAsync](https://github.com/jnbt/NeoAsync)

## Usage

This library separates between the concept of a HTTP client and a HTTP request performer. The client acts as a facade to simplify the complexity of HTTP communication. The actual request performer must implement the `IRequestPerformer` interface.
In the simplest usage one you simple instrument the provided `UnityRequestPerformer` which uses the native Unity provided [`UnityEngine.UnityWebRequest`](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html) class.

```csharp
var factory = new UnityRequestPerformerFactory();

var client = new Client(factory);

client.Get("http://www.neopoly.com", response => UnityEngine.Debug.Log(response.Body));
```

As the `UnityEngine.UnityWebRequest` doesn't support the concept of cookies (outside of WebGL) the `Client` will create an own `ICookieJar` to support them. You can also use a shared
CookieJar if needed:

```csharp
var factory = new UnityRequestPerformerFactory();
var cookies = new CookieJar();

var client1 = new Client(factory, cookies);
var client2 = new Client(factory, cookies);

// Getting a session via a cookie
var data = new Dictionary<string, string>(){
  {"username", "foo"},
  {"password", "boo"}
};
client1.Post("http://www.neopoly.com/login", data, response => UnityEngine.Debug.Log(response.Body));

// Use the same cookie for a second client
client2.Get("http://www.neopoly.com", response => UnityEngine.Debug.Log(response.Body));
```

## Limitations

* At the moment cookies are parsed from the response's header but don't respect the bounded domain. So they will be sent to any host.
* Due the decision from Unity to return the HTTP response headers as a `Dictionary<string, string>` only one `Set-Cookie` field can be respected per response.


## Testing

Use Unity's embedded Test Runner via `Window -> General -> Test Runner`.

## TODO

* Support domain-based cookies

## Licenses

For the license of this project please have a look at LICENSE.txt
