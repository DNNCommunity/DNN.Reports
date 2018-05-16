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


namespace DotNetNuke.Modules.Reports.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    ///     Exception thrown when an error occurs during the execution of a report because the
    ///     data source is not properly configured
    /// </summary>
    public class DataSourceNotConfiguredException : DataSourceException
    {
        public DataSourceNotConfiguredException()
        { }

        public DataSourceNotConfiguredException(LocalizedText localizedMessage) : base(localizedMessage)
        { }

        public DataSourceNotConfiguredException(LocalizedText localizedMessage, Exception innerException) : base(
            localizedMessage, innerException)
        { }

        public DataSourceNotConfiguredException(LocalizedText localizedMessage, string message) : base(
            localizedMessage, message)
        { }

        public DataSourceNotConfiguredException(LocalizedText localizedMessage, string message,
                                                Exception innerException) : base(localizedMessage, innerException)
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public DataSourceNotConfiguredException(string resourceKey) : base(resourceKey)
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public DataSourceNotConfiguredException(string resourceKey, params string[] formatArguments) : base(
            resourceKey, formatArguments)
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public DataSourceNotConfiguredException(string resourceKey, string resourceFile) :
            base(resourceKey, resourceFile)
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public DataSourceNotConfiguredException(string resourceKey, string resourceFile,
                                                params string[] formatArguments) : base(
            resourceKey, resourceFile, formatArguments)
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public DataSourceNotConfiguredException(string resourceKey, Exception innerException) : base(
            resourceKey, innerException)
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public DataSourceNotConfiguredException(string resourceKey, Exception innerException,
                                                params string[] formatArguments) : base(
            resourceKey, innerException, formatArguments)
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public DataSourceNotConfiguredException(string resourceKey, string resourceFile, Exception innerException) :
            base(resourceKey, resourceFile, innerException)
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public DataSourceNotConfiguredException(string resourceKey, string resourceFile, Exception innerException,
                                                params string[] formatArguments) : base(
            resourceKey, resourceFile, innerException, formatArguments)
        { }

        public DataSourceNotConfiguredException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}