# # Stage 1: Build ứng dụng
# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# WORKDIR /app

# # Copy toàn bộ solution và restore package
# COPY . . 
# RUN dotnet restore "MISA.NTTrungWeb05.GD2.sln"

# # # Build và publish API
# RUN dotnet publish "MISA.NTTrungWeb05.GD2/MISA.NTTrungWeb05.GD2.csproj" -c Release -o /app/out

# # Stage 2: Chạy ứng dụng với runtime nhẹ
# FROM mcr.microsoft.com/dotnet/aspnet:6.0
# WORKDIR /app
# COPY --from=build /app/out .
# EXPOSE 80
# ENTRYPOINT ["dotnet", "MISA.NTTrungWeb05.GD2.dll"]

# ----------------------------------------------
# Build bằng dll

# Stage 1: Build ứng dụng
# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# WORKDIR /app

# # Copy toàn bộ solution và restore package
# COPY . . 
# RUN dotnet restore "MISA.NTTrungWeb05.GD2.sln"

# # Build và publish API
# RUN dotnet publish "MISA.NTTrungWeb05.GD2/MISA.NTTrungWeb05.GD2.csproj" -c Release -o /app/out

# Stage 2: Chạy ứng dụng với runtime nhẹ
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
# COPY --from=build /app/out .
COPY ./MISA.NTTrungWeb05.GD2/bin/Release/net6.0 ./
EXPOSE 80
ENTRYPOINT ["dotnet", "MISA.NTTrungWeb05.GD2.dll"]

