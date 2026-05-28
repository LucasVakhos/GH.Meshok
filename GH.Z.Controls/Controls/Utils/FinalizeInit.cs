using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GH.Components
{
    //public interface IFinalizeInit
    //{
    //    void FinalizeInit();
    //}

    //public interface IFinalizeList : IFinalizeInit
    //{
    //    //void Register(object wait);
    //    void NotifyWaits();
    //}

    //public interface IFinalizeControl: IFinalizeList
    //{
    //    void Register(object wait);
    //    //FinalizeList List { get; }
    //}
    //public interface IFinalizeHolder
    //{
    //    void Register(object wait);
    //    Control Owner { get; set; }
    //}


    //public class FinalizeList : IFinalizeControl
    //{
    //    List<object> waits = null;
    //    IFinalizeControl Client = null;

    //    //public FinalizeList List
    //    //{
    //    //    get
    //    //    {
    //    //        if (Client == null)
    //    //            return null;
    //    //        return Client.List;
    //    //    }
    //    //}

    //    public event EventHandler OnFinalizeInit;

    //    public FinalizeList(object client)
    //    {
    //        Client = client as IFinalizeControl;
    //        waits = new List<object>();
    //    }

    //    public void Register(object wait)
    //    {
    //        if (wait != null)
    //            if (waits.IndexOf(wait) == -1)
    //                waits.Add(wait);
    //    }
                
    //    public void NotifyWaits()
    //    {
    //        while (waits.Count > 0)
    //        {
    //            object wait = waits.Last();
    //            (wait as IFinalizeList)?.NotifyWaits();
    //            (wait as IFinalizeInit)?.FinalizeInit();
    //            waits.Remove(wait);
    //        }
    //        waits.Clear();
    //    }

    //    public void FinalizeInit()
    //    {
    //        OnFinalizeInit?.Invoke(Client, EventArgs.Empty);
    //    }


    //}

    //public static class FinalizeInitWaiter
    //{
    //    static List<IFinalizeList> waits = null;
    //    public static void Register(IFinalizeList wait)
    //    {
    //        if (waits == null)
    //            waits = new List<IFinalizeList>();

    //        if (waits.IndexOf(wait) == -1 )
    //            waits.Add(wait);
    //    }
    //    public static void NotifyWaits()
    //    {
    //        while (waits.Count > 0)
    //        {
    //            waits.Last().FinalizeInit();
    //            waits.Remove(waits.Last());
    //        }
    //        waits.Clear();
    //        waits = null;
    //    }

    //}
}
