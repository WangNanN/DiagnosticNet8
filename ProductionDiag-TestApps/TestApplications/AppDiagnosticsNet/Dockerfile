ARG BaseTag=6.0
FROM mcr.microsoft.com/dotnet/aspnet:$BaseTag
EXPOSE 80

ARG CLib=gnu

#Since the image is dynamic, the native package requirements are also different.
#This means the if block only works for alpine images and the else block only works for gnu
RUN if [ $CLib = 'musl' ] ; then apk update && apk add libxml2 libuuid libunwind ; else apt-get update && apt-get install -y --no-install-recommends libxml2 libunwind8 ; fi

WORKDIR /app
#TODO Should exclude Dockerfile
COPY . .

# In order to have dotnet run as the actual entry point process, the Docker image needs to use the
# exec entry point sytax (with the square brackets) instead of the shell entry point. The exec entry point
# does not support environment variable replacement, so its executable and arguments must be literals.
# Thus, if we need to support building Docker containers for more than one application where we want dotnet
# to be the entry point process (instead of sh), we must make a copy of the Dockerfile for each application.
ENTRYPOINT ["dotnet", "AppDiagnosticsNet.dll"]