using System;
using System.Collections.Generic;

using Machine.Container.Model;
using Machine.Container.Services;

namespace Machine.Container.Lifestyles
{
  public class SingletonLifestyle : TransientLifestyle
  {
    #region Member Data
    private object _instance;
    #endregion

    #region SingletonLifestyle()
    public SingletonLifestyle(IActivatorStrategy activatorStrategy, ServiceEntry entry)
     : base(activatorStrategy, entry)
    {
    }
    #endregion

    #region ILifestyle Members
    public override bool CanActivate(IResolutionServices services)
    {
      if (_instance == null)
      {
        return base.CanActivate(services);
      }
      return true;
    }

    public override object Activate(IResolutionServices services)
    {
      if (_instance == null)
      {
        _instance = base.Activate(services);
      }
      return _instance;
    }
    #endregion
  }
}
