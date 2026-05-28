using FirebirdSql.Data.FirebirdClient;
using System;
namespace MeshokBrowser
{
    public class ReaderHelperBase : IDisposable
    {
        protected FbDataReader recs;
        private bool _dataRead = false;
        public bool HasData => recs != null && recs.HasRows && recs.FieldCount > 0;
        public bool DataRead => recs != null && _dataRead;
        public ReaderHelperBase(FbDataReader recs)
        {
            this.recs = recs;
        }
        public virtual bool Read()
        {
            _dataRead = recs.Read();
            return _dataRead;
        }
        public void Dispose()
        {
            recs?.Close();
        }
    }
}
