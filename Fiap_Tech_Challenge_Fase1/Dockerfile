FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["Fiap_Tech_Challenge_Fase1/ConsultaContato.csproj", "Fiap_Tech_Challenge_Fase1/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "Fiap_Tech_Challenge_Fase1/ConsultaContato.csproj"

COPY . .

WORKDIR "/src"
RUN dotnet build "Fiap_Tech_Challenge_Fase1/ConsultaContato.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app


EXPOSE 80

COPY --from=build /app/build /app


ENTRYPOINT ["dotnet", "Fiap_Tech_Challenge_Fase1.dll"]
