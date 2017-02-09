using System.Text;
using Neo.Collections;

namespace Neo.Network {
  public sealed class UserAgentBuilder {
    private const string JoinWith = " ";
    private readonly List<Component> components = new List<Component>();

    public UserAgentBuilder() { }

    public UserAgentBuilder(string name, string version = null, string comment = null) {
      Append(name, version, comment);
    }

    public UserAgentBuilder Append(string name, string version = null, string comment = null) {
      components.Add(new Component {
        Name = name,
        Version =  version,
        Comment = comment
      });
      return this;
    }

    public override string ToString() {
      return components.Join(JoinWith);
    }

    private sealed class Component {
      private const string Separator = "/";
      private const string CommentIntro = " (";
      private const string CommentOutro = ")";

      public string Name { get; set; }
      public string Version { get; set; }
      public string Comment { get; set; }

      public override string ToString() {
        StringBuilder sb = new StringBuilder(Name ?? string.Empty);
        if(!string.IsNullOrEmpty(Version)) sb.Append(Separator).Append(Version);
        if(!string.IsNullOrEmpty(Comment)) sb.Append(CommentIntro).Append(Comment).Append(CommentOutro);
        return sb.ToString();
      }
    }
  }
}
