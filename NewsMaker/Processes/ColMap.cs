using System;
using System.Data;
namespace NewsMaker
{
    public class ColMap
    {
        private readonly DataColumn Column;
        public ColMap(DataColumn column, byte length, bool link = false)
        {
            Length = length;
            Column = column;
            Link = link;
        }
        public int Index => Column == null ? 0 : Column.Ordinal;
        public Type DataType => Column.DataType;
        public string Caption => Column.Caption;
        public byte Length { get; set; }
        public bool Link { get; set; }
    }
}
