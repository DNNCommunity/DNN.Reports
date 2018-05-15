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
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Components;

    /// <summary>
    ///     Base class for exceptions which include information for localizing their text,
    ///     through the use of external resource files
    /// </summary>
    [Serializable]
    public abstract class ReportsModuleException : ApplicationException
    {
        #region  Private Fields

        private readonly LocalizedText _localizedMessage;

        #endregion

        public ReportsModuleException()
        { }

        public ReportsModuleException(LocalizedText localizedMessage)
        {
            this._localizedMessage = localizedMessage;
        }

        public ReportsModuleException(LocalizedText localizedMessage, Exception innerException) : base(
            string.Empty, innerException)
        {
            this._localizedMessage = localizedMessage;
        }

        public ReportsModuleException(LocalizedText localizedMessage, string message) : base(message)
        {
            this._localizedMessage = localizedMessage;
        }

        public ReportsModuleException(LocalizedText localizedMessage, string message,
                                      Exception innerException) : base(message, innerException)
        {
            this._localizedMessage = localizedMessage;
        }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public ReportsModuleException(string resourceKey) : this(new LocalizedText(resourceKey))
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public ReportsModuleException(string resourceKey, params string[] formatArguments) : this(
            new LocalizedText(resourceKey, formatArguments))
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public ReportsModuleException(string resourceKey, string resourceFile) : this(
            new LocalizedText(resourceKey, resourceFile))
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public ReportsModuleException(string resourceKey, string resourceFile, params string[] formatArguments) : this(
            new LocalizedText(resourceKey, resourceFile, formatArguments))
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public ReportsModuleException(string resourceKey, Exception innerException) : this(
            new LocalizedText(resourceKey), innerException)
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public ReportsModuleException(string resourceKey, Exception innerException, params string[] formatArguments) :
            this(new LocalizedText(resourceKey, formatArguments), innerException)
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public ReportsModuleException(string resourceKey, string resourceFile, Exception innerException) : this(
            new LocalizedText(resourceKey, resourceFile), innerException)
        { }

        [Obsolete("This constructor is obsolete, use one of the constructors which accept a LocalizedText object")]
        public ReportsModuleException(string resourceKey, string resourceFile, Exception innerException,
                                      params string[] formatArguments) : this(
            new LocalizedText(resourceKey, resourceFile, formatArguments), innerException)
        { }

        public ReportsModuleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this._localizedMessage =
                info.GetValue(ReportsConstants.SER_KEY_LocalizedText, typeof(LocalizedText)) as LocalizedText;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(ReportsConstants.SER_KEY_LocalizedText, this._localizedMessage, typeof(LocalizedText));
        }

        #region  Public Properties

        public string LocalizedMessage
        {
            get
                {
                    if (this._localizedMessage != null)
                    {
                        return this._localizedMessage.GetLocalizedText();
                    }
                    return null;
                }
        }

        /// <summary>
        ///     Gets the resource key used to localize the exception message
        /// </summary>
        public string ResourceKey
        {
            get
                {
                    if (this._localizedMessage != null)
                    {
                        return this._localizedMessage.ResourceKey;
                    }
                    return null;
                }
        }

        /// <summary>
        ///     Gets the resource file used to localize the exception message, defaults
        ///     to the application's global resource file
        /// </summary>
        public string ResourceFile
        {
            get
                {
                    if (this._localizedMessage != null)
                    {
                        return this._localizedMessage.ResourceFile;
                    }
                    return null;
                }
        }

        /// <summary>
        ///     Gets a read only list of the arguments used to format the localized exception
        ///     message
        /// </summary>
        public IEnumerable<string> FormatArgs
        {
            get
                {
                    if (this._localizedMessage != null)
                    {
                        return this._localizedMessage.FormatArguments;
                    }
                    return new string[] { };
                }
        }

        #endregion
    }
}