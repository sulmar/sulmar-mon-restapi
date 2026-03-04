# Testy integracyjne

**Test jednostkowy** sprawdza pojedynczą jednostkę kodu w pełnej izolacji od innych komponentów i nie powinien korzystać z infrastruktury, natomiast **test integracyjny** sprawdza współpracę kilku komponentów i może, ale nie musi, korzystać z infrastruktury.

# **1. IAsyncLifetime**

IAsyncLifetime pozwala zrobić **asynchroniczną inicjalizację i sprzątanie testów**.


Interfejs:
```cs
public interface IAsyncLifetime
{
    Task InitializeAsync();
    Task DisposeAsync();
}
```

Przykład:
```cs
public class RedisTests : IAsyncLifetime
{
    private RedisContainer _redis;

    public async Task InitializeAsync()
    {
        _redis = new RedisBuilder().Build();
        await _redis.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _redis.DisposeAsync();
    }

    [Fact]
    public void Test1()
    {
    }

    [Fact]
    public void Test2()
    {
    }
}
```

---
## **Co robi xUnit**

Sekwencja wykonania:
```cs
InitializeAsync()

Test1
Test2

DisposeAsync()
```

---


## **Po co to istnieje?**

Bo konstruktor w xUnit **nie może być async**.

```cs
public RedisTests()
{
    await redis.StartAsync(); // ❌ nie można
}
```

Dlatego powstał `IAsyncLifetime`.

# **2. IClassFixture**

`IClassFixture<T>` służy do **współdzielenia jednego obiektu między testami w klasie**.

xUnit:
1. tworzy fixture
2. przekazuje ją do testów
3. niszczy po testach

## Przykład:

Fixture
```cs
public class RedisFixture
{
    public ConnectionMultiplexer Connection { get; }

    public RedisFixture()
    {
        Connection = ConnectionMultiplexer.Connect("localhost");
    }
}
```

Test
```cs
public class RedisTests : IClassFixture<RedisFixture>
{
    private readonly RedisFixture _fixture;

    public RedisTests(RedisFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Test1()
    {
        var db = _fixture.Connection.GetDatabase();
    }
}
```

---
## **Co robi xUnit**

```
create RedisFixture

Test1
Test2
Test3

dispose RedisFixture
```

Fixture jest **jedna dla całej klasy**.

# **3. WebApplicationFactory**

Normalnie aplikację uruchamiasz tak:
```
browser -> Kestrel -> ASP.NET Core
```

W testach:
```
HttpClient -> WebApplicationFactory -> ASP.NET Core (in-memory)
```


Czyli:

- aplikacja startuje
    
- routing działa
    
- middleware działa
    
- DI działa
    
- kontrolery działają


## **2. NuGet**
```
dotnet add package Microsoft.AspNetCore.Mvc.Testing
```


## **3. Minimalny przykład**

API:
```cs
var app = WebApplication.Create();

app.MapGet("/hello", () => "Hello");

app.Run();
```


Test:
```cs
public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_return_hello()
    {
        var response = await _client.GetAsync("/hello");

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal("Hello", body);
    }
}
```

## **4. Co się dzieje w tle**

`WebApplicationFactory`:

1. uruchamia Program.cs
    
2. buduje **Host**
    
3. konfiguruje **TestServer**
    
4. tworzy HttpClient

Schemat
```
Test
 ↓
WebApplicationFactory
 ↓
TestServer
 ↓
ASP.NET Core pipeline
 ↓
Controller / Minimal API
```

