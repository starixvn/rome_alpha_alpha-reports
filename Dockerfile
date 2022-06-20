# escape=`
FROM hub.nexdev.net/base/rocketor/buildservice AS builder

ARG DEPLOY_ENV
ARG ROCKETOR_API
ARG REDMINE_IDS

SHELL ["cmd", "/S", "/C"]

WORKDIR /dev

# Copy files *.sln, *.csproj, packages.config to restore nuget packages
COPY *.sln .
COPY src\Alpha.Reports\*.csproj src\Alpha.Reports\
COPY src\Alpha.Reports\packages.config src\Alpha.Reports\
COPY src\Alpha.Reports.Site\*.csproj src\Alpha.Reports.Site\
COPY src\Alpha.Reports.Site\packages.config src\Alpha.Reports.Site\
COPY tests\Alpha.Reports.StaticData\*.csproj tests\Alpha.Reports.StaticData\
COPY tests\Alpha.Reports.StaticData\packages.config tests\Alpha.Reports.StaticData\
COPY SonarQube\ SonarQube\

RUN nuget.exe restore Alpha.Reports.Site.sln

# Copy source codes
COPY src src\
COPY build build\
COPY tools tools\
COPY tests tests\

RUN MSBuild.exe "src\Alpha.Reports.Site\Alpha.Reports.Site.csproj" `
    /t:Build;WebPublish /m `
    /p:DebugType=None `
    /p:Configuration=Release `
    /p:WebPublishMethod=FileSystem `
    /p:TransformWebConfigEnabled=False `
    /p:LocalizedBuild=true `
    /p:AutoParameterizationWebConfigConnectionStrings=False `
    /p:MarkWebConfigAssistFilesAsExclude=false `
    /p:publishUrl="C:\Compiled" `
    /consoleloggerparameters:ErrorsOnly

WORKDIR C:\Compiled

# Change SP Version
RUN `
    if defined REDMINE_IDS `
    FOR %P IN ( `
        App_Data\SPNames\%DEPLOY_ENV%\DBACC.bodb02.BODBDownlineNet.xml, `
		App_Data\SPNames\%DEPLOY_ENV%\DBAGE.bodb02.BODBDownlineNet.xml, `
		App_Data\SPNames\%DEPLOY_ENV%\DBARC.bodb_Archive.BODBDownlineNet.xml, `
		App_Data\SPNames\%DEPLOY_ENV%\DBBAH.bodb02.BODBDownlineNet.xml, `
		App_Data\SPNames\%DEPLOY_ENV%\DBBOF.bodb02.BODBDownlineNet.xml, `
		App_Data\SPNames\%DEPLOY_ENV%\DBCust.CustDB.DownlineNet.xml, `
		App_Data\SPNames\%DEPLOY_ENV%\DBCust.IPDB.xml, `
		App_Data\SPNames\%DEPLOY_ENV%\DBLOG.bodb_CustomerLog.BODBDownlineNet.xml, `
		App_Data\SPNames\%DEPLOY_ENV%\DBONUSER.ON_USER.BODBDownlineNet.xml, `
		App_Data\SPNames\%DEPLOY_ENV%\DBRPT.bodb_report.BODBDownlineNet.xml, `
		App_Data\SPNames\%DEPLOY_ENV%\DBTXN.bodb02.BODBDownlineNet.xml `
    ) `
    DO curl --fail -o C:\sp-result.xml --form file=@"%P" --form "redmineIds=%REDMINE_IDS%" -X POST %ROCKETOR_API% `
        && xcopy C:\sp-result.xml "%P" /Y

# Transform config
RUN echo "%DEPLOY_ENV%" `
    && xcopy "Web.%DEPLOY_ENV%.config" "Web.config" /Y `
    && xcopy "Web.ConnectionStrings.%DEPLOY_ENV%.config" "Web.ConnectionStrings.config" /Y `
	&& xcopy "Web.AppSettings.%DEPLOY_ENV%.config" "Web.AppSettings.config" /Y `
	&& xcopy "App_Data\activitytracking.%DEPLOY_ENV%.json" "App_Data\activitytracking.json" /Y `
	&& xcopy "App_Data\ProductSection.%DEPLOY_ENV%.config" "App_Data\ProductSection.config" /Y

FROM hub.nexdev.net/base/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019 AS base

WORKDIR C:\inetpub\wwwroot

COPY --from=builder C:\Compiled .

RUN dir .\bin\

ENTRYPOINT ["C:\\LogMonitor\\LogMonitor.exe", "C:\\ServiceMonitor.exe", "w3svc"]