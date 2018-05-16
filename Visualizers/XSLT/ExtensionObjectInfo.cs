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

namespace DotNetNuke.Modules.Reports.Visualizers.Xslt
{
    using System;
    using System.Data;
    using DotNetNuke.Entities.Modules;

    [Serializable]
    public class ExtensionObjectInfo : IHydratable
    {
        private string _clrType;

        private int _extensionObjectId;
        private string _xmlNamespace;

        public string XmlNamespace
        {
            get { return this._xmlNamespace; }
            set { this._xmlNamespace = value; }
        }

        public string ClrType
        {
            get { return this._clrType; }
            set { this._clrType = value; }
        }

        public void Fill(IDataReader dr)
        {
            this._extensionObjectId = (int) dr["ExtensionObjectId"];
            this._xmlNamespace = (string) dr["XmlNamespace"];
            this._clrType = (string) dr["ClrType"];
        }

        public int KeyID
        {
            get { return this._extensionObjectId; }
            set { this._extensionObjectId = value; }
        }
    }
}