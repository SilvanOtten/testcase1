FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
ARG source
WORKDIR /app
COPY ${source:-./bin/Release/netcoreapp3.0} .
ENTRYPOINT ["dotnet", "WebAPI.dll"]