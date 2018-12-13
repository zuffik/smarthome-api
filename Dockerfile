FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

RUN apt-get update && apt-get install -y bluez

# Copy csproj and restore as distinct layers
COPY ./smarthome-api/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

ENV ASPNETCORE_URLS http://+:8000

EXPOSE 8000
ENTRYPOINT ["dotnet", "smarthome-api/out/smarthome-api.dll"]