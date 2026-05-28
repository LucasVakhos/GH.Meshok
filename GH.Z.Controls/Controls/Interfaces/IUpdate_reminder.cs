using System;

namespace GH.Interfaces
{
    public interface IUpdate_reminder : IUpdateAlarm
    {
        int? old_upd_id { get; set; }
        int? new_upd_id { get; set; }
        DateTime? upd_date { get; set; }
    }

}
