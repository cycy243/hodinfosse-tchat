# Build Stage
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /source

# Copy the solution file and project files
COPY *.sln .
COPY ./Tchat.Api.Services/*.csproj ./Tchat.Api.Services/
COPY ./Tchat.Api.Service.Auth/*.csproj ./Tchat.Api.Service.Auth/
COPY ./Tchat.Api.Services.Contact/*.csproj ./Tchat.Api.Services.Contact/
COPY ./Tchat.Api.Services.Messages/*.csproj ./Tchat.Api.Services.Messages/
COPY ./Tchat.Api.Services.User/*.csproj ./Tchat.Api.Services.User/
COPY ./Tchat.Api.Services.Utils/*.csproj ./Tchat.Api.Services.Utils/
COPY ./Tchat.API/*.csproj ./Tchat.API/
COPY ./Tchat.API.Persistence/*.csproj ./Tchat.API.Persistence/
COPY ./Tchat.Api.Data.Repository/*.csproj ./Tchat.Api.Data.Repository/
COPY ./Tchat.Api.Data.Repository.DataBase/*.csproj ./Tchat.Api.Data.Repository.DataBase/
COPY ./Tchat.Api.Domain/*.csproj ./Tchat.Api.Domain/
COPY ./Tchat.Api.Exceptions/*.csproj ./Tchat.Api.Exceptions/
COPY ./Tchat.Api.Mappers/*.csproj ./Tchat.Api.Mappers/
COPY ./Tchat.Api.Models/*.csproj ./Tchat.Api.Models/
COPY ./Tchat.API.Args/*.csproj  ./Tchat.API.Args/

RUN dotnet restore "Tchat.API/Tchat.API.csproj" -a $TARGETARCH

# copy and publish app and libraries
COPY ./Tchat.Api.Services/. ./Tchat.Api.Services/
COPY ./Tchat.Api.Service.Auth/. ./Tchat.Api.Service.Auth/
COPY ./Tchat.Api.Services.Contact/. ./Tchat.Api.Services.Contact/
COPY ./Tchat.Api.Services.Messages/. ./Tchat.Api.Services.Messages/
COPY ./Tchat.Api.Services.User/. ./Tchat.Api.Services.User/
COPY ./Tchat.Api.Services.Utils/. ./Tchat.Api.Services.Utils/
COPY ./Tchat.API/. ./Tchat.API/
COPY ./Tchat.API.Persistence/. ./Tchat.API.Persistence/
COPY ./Tchat.Api.Data.Repository/. ./Tchat.Api.Data.Repository/
COPY ./Tchat.Api.Data.Repository.DataBase/. ./Tchat.Api.Data.Repository.DataBase/
COPY ./Tchat.Api.Domain/. ./Tchat.Api.Domain/
COPY ./Tchat.Api.Exceptions/. ./Tchat.Api.Exceptions/
COPY ./Tchat.Api.Mappers/. ./Tchat.Api.Mappers/
COPY ./Tchat.Api.Models/. ./Tchat.Api.Models/
COPY ./Tchat.API.Args/. ./Tchat.API.Args/

RUN dotnet build "Tchat.API/Tchat.API.csproj" -a $TARGETARCH
RUN dotnet publish "Tchat.API/Tchat.API.csproj" -a $TARGETARCH -c Release --no-build --no-restore -o /app

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "Tchat.API.dll"]
