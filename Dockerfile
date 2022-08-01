# escape=`
FROM hub.nexdev.net/base/rocketor/buildservice AS builder

ARG ROCKETOR_API
ARG REDMINE_IDS

SHELL ["cmd", "/S", "/C"]

WORKDIR /dev

# Copy files *.sln, *.csproj, packages.config to restore nuget packages
COPY *.sln .
COPY src\Alpha.Reports.Site\*.csproj src\Alpha.Reports.Site\
COPY src\Alpha.Reports.Site\packages.config src\Alpha.Reports.Site\

COPY src\Alpha.Reports\*.csproj src\Alpha.Reports\
COPY src\Alpha.Reports\packages.config src\Alpha.Reports\

RUN nuget.exe restore Alpha.Reports.Site.sln

COPY src src\

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
        App_Data\SPNames\Docker\DBACC.bodb02.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBAGE.bodb02.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBARC.bodb_Archive.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBBAH.bodb02.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBBOF.bodb02.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBCust.CustDB.DownlineNet.xml, `
		App_Data\SPNames\Docker\DBCust.IPDB.xml, `
		App_Data\SPNames\Docker\DBLOG.bodb_CustomerLog.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBONUSER.ON_USER.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBRPT.bodb_report.BODBDownlineNet.xml, `
		App_Data\SPNames\Docker\DBTXN.bodb02.BODBDownlineNet.xml `
    ) `
    DO curl --fail -o C:\sp-result.xml --form file=@"%P" --form "redmineIds=%REDMINE_IDS%" -X POST %ROCKETOR_API% `
        && xcopy C:\sp-result.xml "%P" /Y

FROM hub.nexdev.net/base/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019 AS base

WORKDIR C:\inetpub\wwwroot

COPY --from=builder C:\Compiled .

RUN dir .\bin\

ENTRYPOINT ["C:\\LogMonitor\\LogMonitor.exe", "C:\\ServiceMonitor.exe", "w3svc"]