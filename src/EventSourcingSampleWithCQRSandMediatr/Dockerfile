FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /app
 
# Copy csproj and restore as distinct layers
COPY EventSourcingSampleWithCQRSandMediatr.csproj ./
RUN dotnet restore
 
# Copy everything else
COPY . ./

# build and run tests
WORKDIR /app
RUN dotnet build


# publish
WORKDIR /app

RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/out .

RUN mkdir -p /log
EXPOSE 5001/tcp
ENV ASPNETCORE_URLS http://*:5001

CMD ["dotnet", "EventSourcingSampleWithCQRSandMediatr.dll"]
