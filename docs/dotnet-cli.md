# .NET CLI

Najczęściej używane polecenia podczas pracy z .NET i REST API.

---

# Środowisko

```bash
dotnet --version
dotnet --list-sdks
```

# global.json (wersja SDK)

```bash
dotnet new globaljson
dotnet new globaljson --sdk-version {version}
```

# Tworzenie projektów

```bash
dotnet new --list
dotnet new {template}
dotnet new {template} -o {folder}
```

# Rozwiązania (Solution)
```bash
dotnet new sln
dotnet sln add {project.csproj}
dotnet sln remove {project.csproj}
```

# Referencje między projektami
```bash
dotnet add {project.csproj} reference {library.csproj}
dotnet remove {project.csproj} reference {library.csproj}
```

# Pakiety NuGet
```bash
dotnet add package {package-name}
dotnet remove package {package-name}
dotnet restore
```

# Budowanie i uruchamianie
```bash
dotnet build
dotnet run
dotnet watch run
```

# Testy
```bash
dotnet test
dotnet watch test
```

# Publikacja
```bash
dotnet publish -c Release -r win10-x64
dotnet publish -c Release -r linux-x64
dotnet publish -c Release -r osx-x64
```