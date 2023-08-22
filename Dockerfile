# escape=`
FROM hub.nexdev.net/base/rocketor/buildservice AS builder

ARG TRANSFORM_CONFIG
ARG ROCKETOR_API
ARG REDMINE_IDS

SHELL ["cmd", "/S", "/C"]

WORKDIR /dev

# Copy files *.sln, *.csproj, packages.config to restore nuget packages
COPY *.sln .
COPY src src\
COPY packages packages\

RUN `
	# verify nuget packages
	nuget.exe restore Alpha.Reports.Site.sln `
	# build project
	&& MSBuild.exe "src\Alpha.Reports.Site\Alpha.Reports.Site.csproj" `
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
		App_Data\SPNames\Docker\DBTXN.bodb02.BODBDownlineNet.xml, `
		App_Data\SPNames\Internal\DBACC.bodb02.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBAGE.bodb02.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBARC.bodb_Archive.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBBAH.bodb02.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBBOF.bodb02.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBCust.CustDB.DownlineNetView.xml, `
		App_Data\SPNames\Internal\DBCust.IPDB.xml, `
		App_Data\SPNames\Internal\DBLOG.bodb_CustomerLog.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBONUSER.ON_USER.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBRPT.bodb_report.BODBDownlineNetView.xml, `
		App_Data\SPNames\Internal\DBTXN.bodb02.BODBDownlineNetView.xml `
    ) `
    DO curl --fail -o C:\sp-result.xml --form file=@"%P" --form "redmineIds=%REDMINE_IDS%" -X POST %ROCKETOR_API% `
        && xcopy C:\sp-result.xml "%P" /Y

# Transform config
RUN echo "%TRANSFORM_CONFIG%" `
    && xcopy "Web.%TRANSFORM_CONFIG%.config" "Web.config" /Y `
    && xcopy "Web.ConnectionStrings.%TRANSFORM_CONFIG%.config" "Web.ConnectionStrings.config" /Y `
	&& xcopy "Web.AppSettings.%TRANSFORM_CONFIG%.config" "Web.AppSettings.config" /Y `
	&& xcopy "App_Data\securitycode.%TRANSFORM_CONFIG%.json" "App_Data\securitycode.json" /Y `
	&& xcopy "App_Data\ProductSection.%TRANSFORM_CONFIG%.config" "App_Data\ProductSection.config" /Y

FROM hub.nexdev.net/base/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019 AS base

WORKDIR C:\inetpub\wwwroot

COPY --from=builder C:\Compiled .

RUN dir .\bin\

ENTRYPOINT ["C:\\LogMonitor\\LogMonitor.exe", "C:\\ServiceMonitor.exe", "w3svc"]