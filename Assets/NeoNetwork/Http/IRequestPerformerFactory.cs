using System;

namespace Neo.Network.Http{
  public interface IRequestPerformerFactory{
    IRequestPerformer Build();
  }
}