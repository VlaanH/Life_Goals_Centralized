# Life_Goals_Centralized
Социальная сеть, где вы можете обмениваться сообщениями, ставить цели и собирать для них средства. Деньги собираются в криптовалюте Ethereum.

<p align="left">
  <img src="https://cdn.discordapp.com/attachments/504344062485069828/981014894809858048/unknown.png" >
</p>

# Dockerfile
```sh
ARG REPO=mcr.microsoft.com/dotnet/aspnet
FROM $REPO:5.0.17-buster-slim-amd64

ENV \
    # Unset ASPNETCORE_URLS from aspnet base image
    ASPNETCORE_URLS= \
    # Do not generate certificate
    DOTNET_GENERATE_ASPNET_CERTIFICATE=false \
    # SDK version
    DOTNET_SDK_VERSION=5.0.408 \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps performance
    NUGET_XMLDOC_MODE=skip \
    # PowerShell telemetry for docker image usage
    POWERSHELL_DISTRIBUTION_CHANNEL=PSDocker-DotnetSDK-Debian-10

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
        git \
        procps \
        wget \
    && rm -rf /var/lib/apt/lists/*

# Install .NET SDK
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz \
    && dotnet_sha512='abbf22c420df2d8398d1616efa3d31e1b8f96130697746c45ad68668676d12e65ec3b4dd75f28a5dc7607da58b6e369693c0e658def15ce2431303c28e99db55' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet ./packs ./sdk ./templates ./LICENSE.txt ./ThirdPartyNotices.txt \
    && rm dotnet.tar.gz \
    # Trigger first run experience by running arbitrary cmd
    && dotnet help




#
RUN dotnet dev-certs https --trust

EXPOSE 8000
EXPOSE 8001
EXPOSE 80
EXPOSE 443

RUN wget https://github.com/VlaanH/Life_Goals_Centralized/releases/download/mas/LifeGoals.tar.gz && tar -xvzf LifeGoals.tar.gz


WORKDIR /LifeGoals
ENTRYPOINT [ "dotnet", "LifeGoals.dll"]


```
# 1
```sh
docker build -t lifeg .  
```
# 2
```sh
docker run --rm -it -p 5000:80 -p 5001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=5001 -e ASPNETCORE_ENVIRONMENT=Development -v $Env:APPDATA\microsoft\UserSecrets\:/root/.microsoft/usersecrets -v $Env:USERPROFILE\.aspnet\https:/root/.aspnet/https/ -e ASPNETCORE_Kestrel__Certificates__Default__Password -e ASPNETCORE_Kestrel__Certificates__Default__Path lifeg
```
# 3
```sh
https://localhost:5001/
```
# Или с docker hub
```sh
docker run --rm -it -p 5000:80 -p 5001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=5001 -e ASPNETCORE_ENVIRONMENT=Development -v $Env:APPDATA\microsoft\UserSecrets\:/root/.microsoft/usersecrets -v $Env:USERPROFILE\.aspnet\https:/root/.aspnet/https/ -e ASPNETCORE_Kestrel__Certificates__Default__Password -e ASPNETCORE_Kestrel__Certificates__Default__Path vlanh/lifeg
```
# Для сбора средств используется тестовая сеть эфириум - rinkeby

# Аккаунты для теста(заполненные)
Test1@gmail.com;m15Cd#m15Cd#

Test2@gmail.com;m15Cd#m15Cd#

# База данных
В проекте используется ORM библиотека Entity Framework. По этой причине выбор конкретной базы данных не так важен, для удобства тут используется SQLite.

# Почему не alpine linux?
В alpine linux у меня возникли проблемы с https
