

# Problem Details (RFC 7807)

## 🎯 Cel dokumentu

Dokument opisuje sposób obsługi błędów w API zgodnie ze standardem **RFC 7807 – Problem Details for HTTP APIs**.

Celem jest:

- standaryzacja odpowiedzi błędów
- oddzielenie logiki biznesowej od transportu HTTP
- zapewnienie spójnego kontraktu dla klientów API

---

# 📘 Czym jest RFC 7807?

RFC 7807 definiuje ustandaryzowany format odpowiedzi błędów HTTP w postaci JSON.

Zamiast zwracać niestandardowe struktury:

```json
{
  "error": "Something went wrong"
}
```

API powinno zwracać:

```json
{
  "type": "https://example.com/problems/invalid-state-transition",
  "title": "Invalid state transition",
  "status": 409,
  "detail": "Cannot change status from Paid to Draft.",
  "instance": "/orders/123"
}
```

---

# 📦 Struktura ProblemDetails

| Pole | Opis |
|------|------|
| `type` | URI identyfikujące typ problemu |
| `title` | Krótki opis problemu |
| `status` | Kod statusu HTTP |
| `detail` | Szczegółowy opis |
| `instance` | URI żądania |

Dodatkowo można rozszerzyć strukturę o własne pola (np. `traceId`, `errorCode`).

---

# 🔎 Rola kodu HTTP vs ProblemDetails

| Element | Co opisuje |
|----------|------------|
| Status HTTP | Rodzaj błędu na poziomie protokołu |
| ProblemDetails | Kontekst biznesowy i szczegóły |

Przykład:

- `409 Conflict` → konflikt stanu zasobu  
- `detail` → konkretna przyczyna konfliktu  

---

# 🧩 Przykłady w projekcie Order API

## 409 – Konflikt stanu

```csharp
return Results.Problem(
    title: "Invalid state transition",
    detail: ex.Message,
    statusCode: StatusCodes.Status409Conflict);
```

---

## 422 – Naruszenie reguły biznesowej

```csharp
return Results.Problem(
    title: "Business rule violation",
    detail: ex.Message,
    statusCode: StatusCodes.Status422UnprocessableEntity);
```

---

## 404 – Zasób nie istnieje

```csharp
return Results.Problem(
    title: "Order not found",
    statusCode: StatusCodes.Status404NotFound);
```

---

## 412 – Konflikt wersji (ETag)

```csharp
return Results.Problem(
    title: "Version conflict",
    detail: "The resource has been modified by another request.",
    statusCode: StatusCodes.Status412PreconditionFailed);
```

---

# 🏗 Jedno podejście do błędów: Results.Problem(...)

Zamiast używać wielu metod:

- `Results.NotFound()`
- `Results.BadRequest()`
- `Results.Conflict()`
- `Results.UnprocessableEntity()`

możesz używać jednej:

```csharp
Results.Problem(..., statusCode: ...)
```

---

## 🎯 Co to daje?

### 1️⃣ Zawsze RFC 7807

Zawsze dostajesz:

- `Content-Type: application/problem+json`
- body w formacie Problem Details
- spójny kontrakt dla klienta

Nie ma sytuacji, że raz zwracasz pustą odpowiedź, raz JSON, a raz coś zupełnie innego.

---

### 2️⃣ Jeden spójny mechanizm

Całe API używa jednej metody do zwracania błędów.

Nie zastanawiasz się:

> Czy tu dać Conflict()? Czy UnprocessableEntity()? Czy ręcznie StatusCode()?

Zawsze używasz `Results.Problem(...)` i ustawiasz tylko `statusCode`.

To upraszcza kod i uczy konsekwencji.

---

### 3️⃣ Czytelny kontrakt

Frontend i integracje zewnętrzne wiedzą:

> Jeśli status >= 400 → zawsze dostanę Problem Details.

To bardzo upraszcza obsługę błędów po stronie klienta.

---

## 🔎 Przykład – 409

Zamiast:

```csharp
return Results.Conflict(new ProblemDetails
{
    Title = "Invalid state transition",
    Detail = ex.Message,
    Status = 409
});
```

Możesz napisać:

```csharp
return Results.Problem(
    title: "Invalid state transition",
    detail: ex.Message,
    statusCode: StatusCodes.Status409Conflict);
```

Efekt ten sam, ale mechanizm jeden i spójny.

---

Metody takie jak `Results.NotFound()` nadal są przydatne, gdy:

- nie chcesz body
- API jest bardzo proste
- świadomie rezygnujesz z Problem Details

Jeśli jednak budujesz spójne, przewidywalne API — `Results.Problem(...)` powinno być Twoim domyślnym wyborem.

# 🧠 Gdzie generować ProblemDetails?

## ❌ Nie w modelu domenowym

Model domenowy:

- nie zna HTTP
- nie zna ProblemDetails
- rzuca wyjątki domenowe

---

## ✅ W warstwie HTTP

ProblemDetails powinno być generowane:

- w endpointach (Minimal API)
- w kontrolerach (MVC)
- w globalnym middleware

---

# 🔥 Rekomendowane podejście – Globalna obsługa wyjątków

Zamiast używać `try/catch` w każdym endpointcie, można użyć middleware:

```csharp
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features
            .Get<IExceptionHandlerFeature>()?
            .Error;

        var problem = exception switch
        {
            InvalidStateTransitionException ex =>
                new ProblemDetails
                {
                    Title = "Invalid state transition",
                    Detail = ex.Message,
                    Status = 409
                },

            BusinessRuleViolationException ex =>
                new ProblemDetails
                {
                    Title = "Business rule violation",
                    Detail = ex.Message,
                    Status = 422
                },

            _ =>
                new ProblemDetails
                {
                    Title = "Unexpected error",
                    Status = 500
                }
        };

        context.Response.StatusCode = problem.Status ?? 500;
        await context.Response.WriteAsJsonAsync(problem);
    });
});
```

Zalety:

- Czyste endpointy
- Jedno miejsce mapowania wyjątków
- Spójna odpowiedź błędów

---

# 📌 400 vs 422 vs 409

| Status | Znaczenie |
|--------|------------|
| 400 | Błąd formatu żądania (np. brak pola, zły JSON) |
| 422 | Reguła biznesowa naruszona |
| 409 | Konflikt aktualnego stanu zasobu |

---

# 🔐 Bezpieczeństwo

ProblemDetails:

- nie powinno ujawniać szczegółów technicznych
- nie powinno zawierać stack trace
- powinno być bezpieczne dla klienta zewnętrznego

W środowisku produkcyjnym szczegóły błędów powinny być logowane, a nie zwracane klientowi.

---

# 🏁 Podsumowanie

Problem Details:

- Standaryzuje obsługę błędów
- Ułatwia integrację klientom
- Wprowadza profesjonalny kontrakt API
- Oddziela domenę od transportu HTTP

---

> REST to nie tylko endpointy.  
> REST to również sposób komunikowania błędów.