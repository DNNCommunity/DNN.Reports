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


namespace Components.Tests
{
    using DotNetNuke.Modules.Reports;
    using NUnit.Framework;

    internal class ReportsControllerTests : AssertionHelper
    {
        private ReportsController _sut;

        [SetUp]
        public void Setup()
        {
            this._sut = new ReportsController();
        }

        [Test]
        public void TestExportModule()
        {
            // Arrange
            var reportInfo = new ReportInfo
                                 {
                                     AutoRunReport = true,
                                     CacheDuration = 0,
                                     CreatedBy = 1,
                                     Title = "SEO Charcateristics",
                                     DataSourceClass = "DotNetNuke.Modules.DataSources.DotNetNuke.DotNetNukeDataSource",
                                     Description = ""
                                 };

            // Act
            var actual = this._sut.ExportModule(reportInfo);

            // Assert
            this.Expect(actual, Is.Not.Empty);
        }

        [Test]
        public void TestFormatRemoveSQL()
        {
            // Arrange
            var strSQL =
                //"SELECT T.Title AS Title, T.Description AS Description, T.KeyWords AS Keyword FROM {oQ}Tabs AS T WHERE T.Title <>";
            "; -- create drop insert delete update sp_ xp_";

            // Act
            var actual = this._sut.FormatRemoveSQL(strSQL);

            // Assert
            //this.Expect(actual, Is.Not.Empty);
            string[] check = { ";", "--", "create ", "drop ", "insert ", "delete ", "update ", "sp_", "xp_" };
            //this.Expect(actual, this.Contains(";"));
            this.Expect(actual, !this.Contains(";"));
        }

    }
}