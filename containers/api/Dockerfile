FROM mcr.microsoft.com/dotnet/sdk:6.0
COPY . /app
WORKDIR /app
RUN dotnet restore
RUN dotnet build
RUN dotnet tool install --global dotnet-ef

ENV PATH="${PATH}:/root/.dotnet/tools"
 
RUN chmod +x containers/api/entrypoint.sh
CMD /bin/bash containers/api/entrypoint.sh

