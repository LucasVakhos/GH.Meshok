using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using NHibernate;
using NHibernate.Criterion;
using GH.Utils;

namespace GH.NHibernate
{

    public class FbNHRepository<T> : NHRepository<T> where T : BaseEntity
    {
        public FbNHRepository()
        {
            DbServerType = DbServerType.FireBird;
        }
    }

}

