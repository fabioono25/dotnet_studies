# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the code and publish
COPY . ./
RUN dotnet publish -c Release -o out

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Expose port 5555
EXPOSE 5555

# Copy the published app from the build stage
COPY --from=build-env /app/out .

# Set the application to listen on port 5555
ENV ASPNETCORE_URLS=http://+:5555

# Define the entry point for the container
ENTRYPOINT [ "dotnet", "PlatformService.dll" ]
