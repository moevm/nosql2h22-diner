FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet publish Diner -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
ENV ASPNETCORE_ENVIRONMENT Docker
ENV DOTNET_ENVIRONMENT Docker
RUN apt-get -y update && apt-get install -y libgdiplus libc6-dev && \
    ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
WORKDIR /app/out
COPY --from=build /app/ /app
EXPOSE 5000
ENTRYPOINT ["dotnet"]
CMD ["Diner.dll", "--urls", "http://0.0.0.0:5000"]