# Backend

## Start project

- Run project using _vs studio_ or _console_

```console
dotnet run --project *nameOfProject.WebAPI*
```

## Migrations

Use the dotnet cli to make migration.

- Before that install 'dotnet tool install --global dotnet-ef' if it is not installed before

```console
dotnet-ef migrations add *MigrationName* --startup-project Educational.Core.WebAPI --project Educational.Core.DAL

dotnet-ef database update --startup-project Educational.Core.WebAPI --project Educational.Core.DAL

dotnet-ef migrations remove --startup-project Educational.Core.WebAPI --project Educational.Core.DAL
```
