FROM microsoft/dotnet:2.1-sdk
RUN mkdir /app
RUN mkdir -p /root/.kube
WORKDIR /app

COPY ./config /root/.kube/config
COPY . .
RUN dotnet publish -c Release -o out

EXPOSE 5000/tcp
CMD ["dotnet", "out/openhack.k8s.api.dll"]