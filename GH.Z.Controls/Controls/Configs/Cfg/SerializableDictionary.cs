namespace GH.Configs
{
    using GH.NHibernate;
    using GH.Utils;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISupportInitialize,  IXmlSerializable 
    {
        
        private const string DefaultItemTag = "item";

        
        private const string DefaultKeyTag = "key";

        
        private const string DefaultValueTag = "value";

        
        private static readonly XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));

        
        private static readonly XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
        #region Поля и реки
        private int _init = 0;

        public AbstractEntity Owner { get => _owner; set => _owner = value; }
        private AbstractEntity _owner;

        public new TValue this[TKey key]
        {
            get
            {
                try
                {
                    return base[key];
                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex);
                    throw;
                }
            }
            set
            {
                if (_init == 0 && _owner != null)
                    _owner.BeginEdit();
                base[key] = value;
            }
        }

        protected virtual string ItemTagName
        {
            get
            {
                return DefaultItemTag;
            }
        }


        protected virtual string KeyTagName
        {
            get
            {
                return DefaultKeyTag;
            }
        }


        protected virtual string ValueTagName
        {
            get
            {
                return DefaultValueTag;
            }
        }
        #endregion


        public SerializableDictionary()
        {
            BeginInit();
            Init();
            EndInit();
        }

        protected virtual void Init()
        {
            throw new NotImplementedException();
        }

        public SerializableDictionary(AbstractEntity owner): this()
        {
            _owner = owner;
        }

        protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var wasEmpty = reader.IsEmptyElement;

            reader.Read();
            if (wasEmpty)
            {
                return;
            }

            BeginInit();
            try
            {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    ReadItem(reader);
                    reader.MoveToContent();
                }
            }
            finally
            {
                EndInit();
                reader.ReadEndElement();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var keyValuePair in this)
            {
                WriteItem(writer, keyValuePair);
            }
        }

        private void ReadItem(XmlReader reader)
        {
            reader.ReadStartElement(ItemTagName);
            try
            {
                TKey key = ReadKey(reader);
                if (key != null)
                    this[key] = ReadValue(reader);
            }
            finally
            {
                reader.ReadEndElement();
            }
        }

        private TKey ReadKey(XmlReader reader)
        {
            reader.ReadStartElement(KeyTagName);
            try
            {
                return (TKey)keySerializer.Deserialize(reader);
            }
            finally
            {
                reader.ReadEndElement();
            }
        }

        private TValue ReadValue(XmlReader reader)
        {
            reader.ReadStartElement(ValueTagName);
            try
            {
                return (TValue)valueSerializer.Deserialize(reader);
            }
            finally
            {
                reader.ReadEndElement();
            }
        }

        private void WriteItem(XmlWriter writer, KeyValuePair<TKey, TValue> keyValuePair)
        {
            writer.WriteStartElement(ItemTagName);
            try
            {
                WriteKey(writer, keyValuePair.Key);
                WriteValue(writer, keyValuePair.Value);
            }
            finally
            {
                writer.WriteEndElement();
            }
        }

        private void WriteKey(XmlWriter writer, TKey key)
        {
            writer.WriteStartElement(KeyTagName);
            try
            {
                keySerializer.Serialize(writer, key);
            }
            finally
            {
                writer.WriteEndElement();
            }
        }

        private void WriteValue(XmlWriter writer, TValue value)
        {
            writer.WriteStartElement(ValueTagName);
            try
            {
                valueSerializer.Serialize(writer, value);
            }
            finally
            {
                writer.WriteEndElement();
            }
        }


        public void BeginInit()
        {
            _init++;            
        }

        public void EndInit()
        {
            _init--;
            if (_init < 0)
                _init = 0;
        }

        
    }



}

