FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Expose port 6666
EXPOSE 6666

COPY --from=build-env /app/out .

# Set the application to listen on port 6666
ENV ASPNETCORE_URLS=http://+:6666

ENTRYPOINT [ "dotnet", "CommandsService.dll" ]