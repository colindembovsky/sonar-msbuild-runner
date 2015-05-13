//-----------------------------------------------------------------------
// <copyright file="ProcessedArgs.cs" company="SonarSource SA and Microsoft Corporation">
//   Copyright (c) SonarSource SA and Microsoft Corporation.  All rights reserved.
//   Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//-----------------------------------------------------------------------

namespace SonarQube.Common
{
	public enum ProcessingType
	{
		Unknown = 1,
		Pre,
		Post
	}

    /// <summary>
    /// Data class to hold validated command line arguments
    /// </summary>
    public class ProcessedArgs
    {
        private readonly ProcessingType processingType;
        private readonly string key;
		private readonly string version;
        private readonly string name;
        private readonly string propertiesPath;
        private readonly string username;
        private readonly string password;

		public ProcessedArgs(ProcessingType type, string key, string name, string version, string propertiesPath, string username, string password)
        {
			this.processingType = type;
            this.key = key;
			this.name = name;
            this.version = version;
            this.propertiesPath = propertiesPath;
            this.username = username;
			this.password = password;
		}

        public ProcessingType ProcessingType { get { return this.processingType; } }

        public string ProjectKey { get { return this.key; } }

        public string ProjectName { get { return this.name; } }

        public string ProjectVersion { get { return this.version; } }

        public string RunnerPropertiesPath { get { return this.propertiesPath; } }

        public string Username { get { return this.username; } }

        public string Password { get { return this.password; } }
    }
}
