FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

RUN apt install glib2.0
RUN wget http://www.kernel.org/pub/linux/bluetooth/bluez-4.101.tar.gz && tar xvzf bluez-4.101.tar.gz
RUN cd bluez-4.101 && ./configure --prefix=/usr --mandir=/usr/share/man --sysconfdir=/etc --localstatedir=/var \
    --libexecdir=/lib && make && make install

# Copy csproj and restore as distinct layers
COPY ./smarthome-api/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

ENV ASPNETCORE_URLS http://+:8000

EXPOSE 8000
ENTRYPOINT ["dotnet", "smarthome-api/out/smarthome-api.dll"]