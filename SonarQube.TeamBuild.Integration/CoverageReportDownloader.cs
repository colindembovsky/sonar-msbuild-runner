//-----------------------------------------------------------------------
// <copyright file="CoverageReportDownloader.cs" company="SonarSource SA and Microsoft Corporation">
//   Copyright (c) SonarSource SA and Microsoft Corporation.  All rights reserved.
//   Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//-----------------------------------------------------------------------

using SonarQube.Common;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SonarQube.TeamBuild.Integration
{
    internal class CoverageReportDownloader : ICoverageReportDownloader
    {
		public bool DownloadReport(string reportUrl, string newFullFileName, ILogger logger, string username = null, string password = null)
        {
            if (string.IsNullOrWhiteSpace(reportUrl))
            {
                throw new ArgumentNullException("reportUrl");
            }
            if (string.IsNullOrWhiteSpace(newFullFileName))
            {
                throw new ArgumentNullException("newFullFileName");
            }
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            string downloadDir = Path.GetDirectoryName(newFullFileName);
            if (!Directory.Exists(downloadDir))
            {
                Directory.CreateDirectory(downloadDir);
            }

			if (string.IsNullOrEmpty(username))
			{
				using (WebClient myWebClient = new WebClient())
				{
					myWebClient.UseDefaultCredentials = true;

					logger.LogMessage(Resources.DOWN_DIAG_DownloadCoverageReportFromTo, reportUrl, newFullFileName);

					myWebClient.DownloadFile(reportUrl, newFullFileName);
				}
			}
			else
			{
				DownloadFileUsingBasicAuth(reportUrl, newFullFileName, logger, username, password);
			}

            return true;
        }

		private void DownloadFileUsingBasicAuth(string reportUrl, string newFullFileName, ILogger logger, string username, string password)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Add(
					new MediaTypeWithQualityHeaderValue("application/json"));

				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
					Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
							string.Format("{0}:{1}", username, password))));

				logger.LogMessage(Resources.DOWN_DIAG_DownloadCoverageReportFromTo, reportUrl, newFullFileName);
				using (HttpResponseMessage response = client.GetAsync(reportUrl).Result)
				{
					response.EnsureSuccessStatusCode();
					string responseBody = response.Content.ReadAsStringAsync().Result;
					File.WriteAllText(newFullFileName, responseBody);
				}
			}
		}
	}
}
