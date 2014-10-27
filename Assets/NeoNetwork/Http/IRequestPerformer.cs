using System;

namespace Neo.Network.Http{
  public interface IRequestPerformer{
    void Perform(Request request, FinishCallback callback);
  }
}