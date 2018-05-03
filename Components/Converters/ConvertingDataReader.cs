#region Copyright
// 
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2018
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//
#endregion


namespace DotNetNuke.Modules.Reports.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     A wrapper around an existing data reader that applies filters to the field values
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///     [anurse]	10/13/2007	Created
    /// </history>
    /// -----------------------------------------------------------------------------
    public class ConvertingDataReader : IDataReader
    {
        private readonly IDataReader adaptee;
        private IDictionary<string, IList<ConverterInstanceInfo>> converters;

        private bool disposedValue; // To detect redundant calls

        public ConvertingDataReader(IDataReader adaptee, IDictionary<string, IList<ConverterInstanceInfo>> converters)
        {
            this.adaptee = adaptee;
            this.converters = converters;
        }

        public void Close()
        {
            this.adaptee.Close();
        }

        public int Depth => this.adaptee.Depth;

        public DataTable GetSchemaTable()
        {
            return this.adaptee.GetSchemaTable();
        }

        public bool IsClosed => this.adaptee.IsClosed;

        public bool NextResult()
        {
            return this.adaptee.NextResult();
        }

        public bool Read()
        {
            return this.adaptee.Read();
        }

        public int RecordsAffected => this.adaptee.RecordsAffected;

        public int FieldCount => this.adaptee.FieldCount;

        public bool GetBoolean(int i)
        {
            return this.GetConverted(this.adaptee.GetName(i), this.adaptee.GetBoolean(i));
        }

        public byte GetByte(int i)
        {
            return this.GetConverted(this.adaptee.GetName(i), this.adaptee.GetByte(i));
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            var ret = this.adaptee.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
            buffer = this.GetConverted(this.adaptee.GetName(i), buffer);
            return ret;
        }

        public char GetChar(int i)
        {
            return this.GetConverted(this.adaptee.GetName(i), this.adaptee.GetChar(i));
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            var ret = this.adaptee.GetChars(i, fieldoffset, buffer.ToString().ToCharArray(), bufferoffset, length);
            buffer = this.GetConverted(this.adaptee.GetName(i), buffer);
            return ret;
        }

        public IDataReader GetData(int i)
        {
            return new ConvertingDataReader(this.adaptee.GetData(i), this.converters);
        }

        public string GetDataTypeName(int i)
        {
            return this.adaptee.GetDataTypeName(i);
        }

        public DateTime GetDateTime(int i)
        {
            return this.GetConverted(this.adaptee.GetName(i), this.adaptee.GetDateTime(i));
        }

        public decimal GetDecimal(int i)
        {
            return this.GetConverted(this.adaptee.GetName(i), this.adaptee.GetDecimal(i));
        }

        public double GetDouble(int i)
        {
            return this.GetConverted(this.adaptee.GetName(i), this.adaptee.GetDouble(i));
        }

        public Type GetFieldType(int i)
        {
            return this.adaptee.GetFieldType(i);
        }

        public float GetFloat(int i)
        {
            return this.GetConverted(this.adaptee.GetName(i), this.adaptee.GetFloat(i));
        }

        public Guid GetGuid(int i)
        {
            return this.GetConverted(this.adaptee.GetName(i), this.adaptee.GetGuid(i));
        }

        public short GetInt16(int i)
        {
            return this.GetConverted(this.adaptee.GetName(i), this.adaptee.GetInt16(i));
        }

        public int GetInt32(int i)
        {
            return this.GetConverted(this.adaptee.GetName(i), this.adaptee.GetInt32(i));
        }

        public long GetInt64(int i)
        {
            return this.GetConverted(this.adaptee.GetName(i), this.adaptee.GetInt64(i));
        }

        public string GetName(int i)
        {
            return this.adaptee.GetName(i);
        }

        public int GetOrdinal(string name)
        {
            return this.adaptee.GetOrdinal(name);
        }

        public string GetString(int i)
        {
            return this.GetConverted(this.adaptee.GetName(i), this.adaptee.GetString(i));
        }

        public dynamic GetValue(int i)
        {
            return this.GetConverted(this.adaptee.GetName(i), this.adaptee.GetValue(i));
        }

        public int GetValues(object[] values)
        {
            var ret = this.adaptee.GetValues(values);
            for (var i = 0; i <= values.Length - 1; i++)
            {
                values[i] = this.GetConverted(this.adaptee.GetName(i), values[i]);
            }
            return ret;
        }

        public bool IsDBNull(int i)
        {
            return this.adaptee.IsDBNull(i);
        }

        public dynamic this[int i] => this.GetValue(i);

        public dynamic this[string name] => this.GetValue(this.GetOrdinal(name));

        #region  IDisposable Support

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        private T GetConverted<T>(string fieldName, T value)
        {
            if (!this.converters.ContainsKey(fieldName))
            {
                return value;
            }

            var list = this.converters[fieldName];
            foreach (var converter in list)
            {
                value = (T) ReportsController.ApplyConverter(value, converter.ConverterName, converter.Arguments);
            }

            return value;
        }

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.adaptee.Dispose();
                }

                this.converters = null;
            }
            this.disposedValue = true;
        }
    }
}