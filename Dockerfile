FROM mcr.microsoft.com/dotnet/aspnet:6.0

COPY bin/Release/net6.0/publish/ App/
COPY *.csv App/
WORKDIR /App
ENTRYPOINT ["dotnet", "csv-parser.dll"]
CMD [ "arg0", "arg1" ]