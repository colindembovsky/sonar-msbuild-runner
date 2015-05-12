﻿//-----------------------------------------------------------------------
// <copyright file="MockReportDownloader.cs" company="SonarSource SA and Microsoft Corporation">
//   Copyright (c) SonarSource SA and Microsoft Corporation.  All rights reserved.
//   Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SonarQube.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SonarQube.TeamBuild.Integration.Tests.Infrastructure
{
    internal class MockReportDownloader : ICoverageReportDownloader
    {
        private int callCount;
        private IList<string> requestedUrls = new List<string>();
        private IList<string> targetFileNames  = new List<string>();

        #region Test helpers

        public bool CreateFileOnDownloadRequest { get; set; }

        #endregion

        #region Assertions

        public void AssertExpectedDownloads(int expected)
        {
            Assert.AreEqual(expected, callCount, "DownloadReport called an unexpected number of times");
        }

        public void AssertDownloadNotCalled()
        {
            Assert.AreEqual(0, this.callCount, "Not expecting DownloadReport to have been called");
        }
        
        public void AssertExpectedUrlsRequested(params string[] urls)
        {
            CollectionAssert.AreEqual(urls, this.requestedUrls.ToArray(), "Unexpected urls requested");
        }

        public void AssertExpectedTargetFileNamesSupplied(params string[] urls)
        {
            CollectionAssert.AreEqual(urls, this.targetFileNames.ToArray(), "Unexpected target files names supplied");
        }

        #endregion

        #region ICoverageReportDownloader interface

        bool ICoverageReportDownloader.DownloadReport(string reportUrl, string newFullFileName, ILogger logger, string username, string password)
        {
            this.callCount++;
            this.requestedUrls.Add(reportUrl);
            this.targetFileNames.Add(newFullFileName);

            if (this.CreateFileOnDownloadRequest)
            {
                File.WriteAllText(newFullFileName, string.Empty);
            }

            return this.CreateFileOnDownloadRequest;
        }

        #endregion
    }
}
