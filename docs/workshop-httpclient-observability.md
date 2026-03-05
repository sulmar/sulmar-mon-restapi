# Workshop: HttpClient Observability

## Cel warsztatu

W systemach mikroserwisowych często pojawia się problem z diagnozowaniem błędów oraz analizą wydajności.  
Gdy jedno żądanie przechodzi przez kilka usług, trudno ustalić:

- które logi dotyczą tego samego requestu
- ile czasu zajmuje wywołanie między usługami
- gdzie powstają opóźnienia

Celem tego warsztatu jest wprowadzenie **podstawowej obserwowalności (observability)** dla komunikacji między usługami HTTP.

Po ukończeniu ćwiczenia:

- każde żądanie będzie posiadało **Correlation ID**
- identyfikator będzie **propagowany między usługami**
- Service A będzie **mierzył czas wywołań HTTP**
- logi będą umożliwiały powiązanie zdarzeń między usługami

---

# Scenariusz

System składa się z dwóch usług:

- **Service A – Ordering Service**
- **Service B – Warehouse Service**

Service A odpowiada za składanie zamówień.  
Podczas składania zamówienia musi zarezerwować produkty w magazynie.

W tym celu wywołuje usługę **Warehouse Service**.

```
Client
|
v
Ordering Service (A)
|
v
Warehouse Service (B)
```


1. Klient wysyła żądanie do **Service A**
2. **Service A** wywołuje endpoint w **Service B** przy użyciu `HttpClient`
3. **Service B** zwraca odpowiedź

Warehouse Service udostępnia interfejs:

```csharp
public interface IWarehouseService
{
    Task<bool> Reserve(string productId, int quantity);
}
```

Rezerwacja oznacza zablokowanie określonej liczby produktów w magazynie.

Endpoint może symulować opóźnienie systemu magazynowego:
```cs
await Task.Delay(Random.Shared.Next(50, 400));
```

---

# Problem

Aktualnie system posiada dwa problemy:

1. W logach **nie da się powiązać żądań między Service A i Service B**
2. Nie wiadomo **ile czasu zajmują wywołania HTTP między usługami**

Twoim zadaniem jest poprawienie diagnostyki systemu.

---

# Zadanie

Zaimplementuj mechanizm, który:

1. Zapewnia obecność identyfikatora żądania w nagłówku:
```
X-Correlation-Id
```

2. Jeśli klient wysyła nagłówek `X-Correlation-Id`, system powinien go użyć.

3. Jeśli nagłówek nie istnieje:

- należy wygenerować nowy identyfikator
- identyfikator powinien być przekazany dalej między usługami

4. **Service A musi logować czas wywołania Service B**

Log powinien zawierać:

- metodę HTTP
- adres URL
- czas wykonania w ms
- kod odpowiedzi
- `X-Correlation-Id`

Przykładowy log:
```
[7f23d3b4] POST https://warehouse-api/products/{guid}/reservations -> 82 ms (200)
```


---

# Wymagania architektoniczne

Rozwiązanie musi spełniać następujące warunki:

- nie wolno ręcznie dodawać nagłówków w każdym miejscu wywołania `HttpClient`
- mechanizm powinien działać **automatycznie dla wszystkich requestów**
- implementacja powinna respektować zasadę **Single Responsibility Principle**

---

# Wskazówki

Możesz wykorzystać:

- `DelegatingHandler`
- `HttpClientFactory`
- `IHttpContextAccessor`
- nagłówki HTTP
- `Stopwatch`

---

