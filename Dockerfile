FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WhatToEat.csproj", "./"]
RUN dotnet restore "WhatToEat.csproj"
COPY . .
RUN dotnet build "WhatToEat.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WhatToEat.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final

RUN apt update && \
    apt install --no-install-recommends -y openssh-server && \
    echo "root:Docker!" | chpasswd

COPY appservice.ssh.conf /etc/ssh/sshd_config.d/
RUN service ssh start

WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80 2222

ENTRYPOINT dotnet WhatToEat.dll 2>&1 | tee log.txt