FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FamousQuoteQuiz.MVC/FamousQuoteQuiz.MVC.csproj", "FamousQuoteQuiz.MVC/"]
COPY ["FamousQuoteQuiz.Dal/FamousQuoteQuiz.Dal.csproj", "FamousQuoteQuiz.Dal/"]
RUN dotnet restore "FamousQuoteQuiz.MVC/FamousQuoteQuiz.MVC.csproj"
COPY . .
WORKDIR "/src/FamousQuoteQuiz.MVC"
RUN dotnet build "FamousQuoteQuiz.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FamousQuoteQuiz.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FamousQuoteQuiz.MVC.dll"]
