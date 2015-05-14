# SonarQube Test Integration for VSO
If you're wanting to integrate TeamBuild into SonarQube, then you can use the official integration, [sonar-msbuild-runner](https://github.com/SonarSource/sonar-msbuild-runner).

However, if you want test coverage analysis and are on VSO, then the post-processing will fail since the agent cannot authenticate against VSO to get the build coverage results.

Use this version of the SonarQube runner instead.

The official team should be releasing a better official solution soon, so this is just temporary.

## Changes from the official runner
The biggest caveat for this version is that you need to supply username and password to connect to VSO using alternate credentials. This is a security risk!

In order to use this version, follow these steps:

1. Download the zip with the updated runner from [here](http://1drv.ms/1FfVFya) to your SonarQube server. This file contains the compiled version of the source code from this repo.
2. Unblock and unzip the file.
3. Delete the `sonar-csharp-plugin-xx.jar` from the `extensions\plugins` folder of your SonarQube server, and copy the jar file from the zip file to this folder.
4. Copy `SonarQube.Common.dll` and `SonarQube.MSBuild.Runner.exe` over the corresponding files from the old version in the folder you copied these to previously.
5. Restart SonarQube (if running interactively) or the SonarQube service (if running as a service).

### Edit the build definition
You will need to edit your build definition. Also, you will have to enable alternate credentials for a user on the VSO account. It is this alternate username and password that is required for post-processing.

1. Open your build definition.
2. Add `/t:pre` to the call to the runner for pre-processing (typically this is in the Pre-Build script settings). Your arguments should now look something like this: `/t:pre /k:key /n:project /v:version`
3. Add `/t:post /u:username /p:password` to the argument for post-processing (typically in the Post-Test script settings). There was no need to pass in post-processing arguments before, so you will be adding them to the empty `args` setting.
where _username_ is the alternate credentials username and _password_ is the alternate credentials password.