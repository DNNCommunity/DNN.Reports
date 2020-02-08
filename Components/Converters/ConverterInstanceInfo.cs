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

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     Stores information about a single Converter instance
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///     [anurse]	10/13/2007	Created
    /// </history>
    /// -----------------------------------------------------------------------------
    [Serializable]
    public class ConverterInstanceInfo
    {
        #region  Public Constructor

        #endregion

        #region  Private Fields

        private string _FieldName;
        private string _ConverterName;
        private string[] _Arguments;

        #endregion

        #region  Public Properties

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name of the field affected by this Converter
        /// </summary>
        /// <returns>The name of the field affected by this Converter</returns>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name of the Converter to be applied
        /// </summary>
        /// <returns>The name of the Converter to be applied</returns>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public string ConverterName
        {
            get { return _ConverterName; }
            set { _ConverterName = value; }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the arguments to be passed to the Converter
        /// </summary>
        /// <returns>The arguments to be passed to the Converter</returns>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------
        public string[] Arguments
        {
            get { return _Arguments; }
            set { _Arguments = value; }
        }

        #endregion
    }
}