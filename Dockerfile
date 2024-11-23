# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expose the port for the application
EXPOSE 80

# Specify the entry point for the application
ENTRYPOINT ["dotnet", "Presentation.dll"]
