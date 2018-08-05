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


namespace DotNetNuke.Modules.Reports
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Components;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Modules.Reports.Converters;
    using DotNetNuke.Services.FileSystem;

    public sealed class ArrayUtils
    {
        public static bool Contains(Array arr, object elem)
        {
            return Array.IndexOf(arr, elem) != -1;
        }
    }

    public sealed class ConverterUtils
    {
        public static void AddConverter(IDictionary<string, IList<ConverterInstanceInfo>> Converters,
                                        ConverterInstanceInfo Converter)
        {
            if (string.IsNullOrEmpty(Converter.FieldName.Trim()))
            {
                return;
            }
            if (!Converters.ContainsKey(Converter.FieldName) || ReferenceEquals(Converters[Converter.FieldName], null))
            {
                Converters[Converter.FieldName] = new List<ConverterInstanceInfo>();
            }
            Converters[Converter.FieldName].Add(Converter);
        }
    }

    public sealed class DropDownUtils
    {
        public static void TrySetValue(DropDownList DropDown, string TryValue, string DefValue)
        {
            if (ReferenceEquals(DropDown.Items.FindByValue(TryValue), null))
            {
                DropDown.SelectedValue = DefValue;
            }
            else
            {
                DropDown.SelectedValue = TryValue;
            }
        }
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The SettingsUtil Module provides utilities for accessing settings
    /// </summary>
    /// <history>
    ///     [anurse]     08/31/2006    Documented
    /// </history>
    /// -----------------------------------------------------------------------------
    public sealed class SettingsUtil
    {
        public static bool FieldIsNotSet(Dictionary<string, string> settings, string setting)
        {
            return !settings.ContainsKey(setting) || string.IsNullOrEmpty(settings[setting]);
        }

        // Gets a setting from the dictionary provided, or returns defValue if the setting is null
        public static T GetDictionarySetting<T>(Dictionary<string, string> settings, string key, T defValue)
        {
            if (!settings.ContainsKey(key))
            {
                return defValue;
            }
            var sVal = settings[key];
            var retValue = defValue;
            if (!string.IsNullOrEmpty(sVal))
            {
                try
                {
                    retValue = (T) Convert.ChangeType(sVal, typeof(T));
                }
                catch (Exception)
                {
                    retValue = defValue;
                }
            }
            return retValue;
        }

        // Gets a setting from the hashtable provided, or returns defValue if the setting is null
        public static T GetHashtableSetting<T>(IDictionary settings, string key, T defValue)
        {
            if (!settings.Contains(key))
            {
                return defValue;
            }
            var o = settings[key];
            var retValue = defValue;
            if (o != null)
            {
                try
                {
                    retValue = (T) Convert.ChangeType(o, typeof(T));
                }
                catch (Exception)
                {
                    retValue = defValue;
                }
            }
            return retValue;
        }
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     The SettingsUtil Module provides general utilities
    /// </summary>
    /// <history>
    ///     [anurse]     08/31/2006    Documented
    /// </history>
    /// -----------------------------------------------------------------------------
    public sealed class Utilities
    {
        public static string RemoveSpaces(string Str)
        {
            // Regex lets us remove all whitespace, not just " "
            return Regex.Replace(Str, "\\s+", "");
        }

        public static string MapFileIdPath(PortalSettings PortalSettings, string FileId)
        {
            if (!string.IsNullOrEmpty(FileId))
            {
                // There are 2 different ways the filename is returned, can start file fileid= and a filenumber
                if (FileId.ToLower().StartsWith("fileid=", StringComparison.OrdinalIgnoreCase))
                {
                    var intFileId = int.Parse(FileId.Substring(7));
                    return MapFileIdPath(PortalSettings, intFileId);
                }
                // Or is is a real filename
                return string.Concat(PortalSettings.HomeDirectoryMapPath, FileId).Replace("/", "\\");
            }
            return string.Empty;
        }

        public static string MapFileIdPath(PortalSettings PortalSettings, int FileId)
        {
            var objFile = FileManager.Instance.GetFile(FileId);

            var sProtectedExtension = string.Empty;
            if (objFile.StorageLocation == (int) FolderController.StorageLocationTypes.DatabaseSecure)
            {
                return string.Empty;
            }
            if (objFile.StorageLocation == (int) FolderController.StorageLocationTypes.SecureFileSystem)
            {
                sProtectedExtension = Globals.glbProtectedExtension;
            }
            return string.Concat(PortalSettings.HomeDirectoryMapPath, objFile.Folder, objFile.FileName,
                                 sProtectedExtension);
        }

        public static Color ParseColorString(string colorString)
        {
            var bIsShortString = false;
            if (string.IsNullOrEmpty(colorString))
            {
                return Color.Black;
            }
            if (colorString.StartsWith("#"))
            {
                colorString = colorString.Substring(1);
            }
            if (colorString.Length == 3)
            {
                bIsShortString = true;
            }
            else if (colorString.Length != 6)
            {
                return Color.Black;
            }

            var clr = Color.Black;
            try
            {
                var iR = ParseHexCode(colorString, bIsShortString, 0);
                var iG = ParseHexCode(colorString, bIsShortString, 1);
                var iB = ParseHexCode(colorString, bIsShortString, 2);
                clr = Color.FromArgb(iR, iG, iB);
            }
            catch (FormatException)
            {
                return Color.Black;
            }
            return clr;
        }

        private static int ParseHexCode(string colorString, bool
                                            bIsShortString, int Position)
        {
            var len = 2;
            if (bIsShortString)
            {
                len = 1;
            }
            else
            {
                Position *= 2;
            }
            return int.Parse(colorString.Substring(Position, len), NumberStyles.HexNumber);
        }

        internal static IEnumerable<string> GetExtensions(string rootPhysicalPath, string extensionFolder)
        {
            // Initialize the list
            var exts = new List<string>();

            // Get the extension sub-directory and verify its existance
            var extensionDir = new DirectoryInfo(Path.Combine(rootPhysicalPath, extensionFolder));
            if (!extensionDir.Exists)
            {
                return exts; // No extensions to load
            }

            // Iterate across the subdirectories
            foreach (var dir in extensionDir.GetDirectories())
            {
                exts.Add(dir.Name);
            }

            return exts;
        }

        internal static string GetExtensionFile(string rootPhysicalPath, string extensionFolder,
                                                string extensionName, string path)
        {
            // Get the extension sub-directory and verify its existance
            var extensionDir =
                new DirectoryInfo(Path.Combine(Path.Combine(rootPhysicalPath, extensionFolder), extensionName));
            if (!extensionDir.Exists)
            {
                return null; // Extension doesn't exist
            }

            // Find the path
            return Path.Combine(extensionDir.FullName, path);
        }


        internal static TControlType LoadExtensionControl<TControlType>(string rootPhysicalPath, string extensionFolder,
                                                                        string extensionName, string controlName,
                                                                        TemplateControl parentControl)
            where TControlType : class
        {
            // Find the control path and verify its existance
            var ctrlPath = GetExtensionFile(rootPhysicalPath, extensionFolder, extensionName, "Settings.ascx");
            if (!File.Exists(ctrlPath))
            {
                return null; // Control doesn't exist
            }

            // Load the control and return it
            return parentControl.LoadControl(ctrlPath) as TControlType;
        }

        public static GridLines GetGridLinesSetting(Dictionary<string, string> VisualizerSettings)
        {
            var gridLines =
                Convert.ToString(SettingsUtil.GetDictionarySetting(VisualizerSettings,
                                                                   ReportsConstants.SETTING_Grid_GridLines,
                                                                   ReportsConstants.DEFAULT_Grid_GridLines));
            if (bool.TrueString.Equals(gridLines, StringComparison.InvariantCultureIgnoreCase))
            {
                return GridLines.Both;
            }
            if (bool.FalseString.EndsWith(gridLines, StringComparison.InvariantCultureIgnoreCase))
            {
                return GridLines.None;
            }
            return (GridLines) Enum.Parse(typeof(GridLines), gridLines);
        }
    }
}