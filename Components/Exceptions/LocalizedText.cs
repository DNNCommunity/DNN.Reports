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
    using DotNetNuke.Services.Localization;

    [Serializable]
    public class LocalizedText : ISerializable
    {
        public LocalizedText(string resourceKey) : this(resourceKey, RESX_ModuleCommon)
        { }

        public LocalizedText(string resourceKey, string[] formatArguments) : this(
            resourceKey, RESX_ModuleCommon, formatArguments)
        { }

        public LocalizedText(string resourceKey, string resourceFile, params string[] formatArguments)
        {
            this.ResourceKey = resourceKey;
            this.ResourceFile = resourceFile;
            this.FormatArguments = formatArguments;
        }

        public LocalizedText(SerializationInfo info, StreamingContext context)
        {
            this.ResourceKey = info.GetString(SER_KEY_ResourceKey);
            this.ResourceFile = info.GetString(SER_KEY_ResourceFile);

            var count = info.GetInt32(SER_KEY_FormatArgsCount);
            this.FormatArguments = new string[count + 1];
            for (var i = 0; i <= count; i++)
            {
                this.FormatArguments[i] = info.GetString(string.Concat(SER_KEY_FormatArgs, i.ToString()));
            }
        }

        public string ResourceKey { get; }

        public string ResourceFile { get; }

        public string[] FormatArguments { get; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(SER_KEY_ResourceKey, this.ResourceKey);
            info.AddValue(SER_KEY_ResourceFile, this.ResourceFile);
            info.AddValue(SER_KEY_FormatArgsCount, this.FormatArguments.Length);
            for (var i = 0; i <= this.FormatArguments.Length; i++)
            {
                info.AddValue(string.Concat(SER_KEY_FormatArgs, i.ToString()), this.FormatArguments[i]);
            }
        }

        public string GetLocalizedText()
        {
            var localized = string.Empty;
            if (!string.IsNullOrEmpty(this.ResourceFile))
            {
                localized = Localization.GetString(this.ResourceKey, this.ResourceFile);
            }
            else
            {
                localized = Localization.GetString(this.ResourceKey);
            }
            if (this.FormatArguments != null && this.FormatArguments.Length > 0)
            {
                return string.Format(localized, this.FormatArguments);
            }
            return localized;
        }

        #region  Constants

        private const string SER_KEY_ResourceKey = "ResourceKey";
        private const string SER_KEY_ResourceFile = "ResourceFile";
        private const string SER_KEY_FormatArgs = "FormatArg";
        private const string SER_KEY_FormatArgsCount = "FormatArgsCount";
        private const string RESX_ModuleCommon = "~/DesktopModules/Reports/App_LocalResources/SharedResources.resx";

        #endregion
    }
}