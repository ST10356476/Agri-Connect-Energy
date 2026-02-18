# Official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy everything and build
COPY . ./
RUN dotnet restore "./Agri-Energy Connect.csproj"
RUN dotnet publish "./Agri-Energy Connect.csproj" -c Release -o /app/publish

# Use the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port and start
EXPOSE 80
ENTRYPOINT ["dotnet", "Agri-Energy Connect.dll"]
