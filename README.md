# Update DB:
- Update DefaultConnection trong file \IotClassifier.API\appsettings.json
- Sau do chay lenh: dotnet ef database update --project "IotClassifier.Infrastructure" --startup-project "IotClassifier.API"
