FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./smarthome-api/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

EXPOSE 5000
ENTRYPOINT ["dotnet", "out/smarthome-api.dll"]

