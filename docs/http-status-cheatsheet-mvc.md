

# HTTP Status & ASP.NET Core MVC – Cheat Sheet

Tabela pomocnicza używana podczas szkolenia  
Tworzenie usług REST API w .NET 9

---

## ControllerBase → HTTP Status

| Controller Method            | HTTP Status | Znaczenie |
|-----------------------------|-------------|------------|
| `Ok()`                     | 200 OK      | Zwracamy dane |
| `Created()`                | 201 Created | Utworzenie zasobu |
| `CreatedAtAction()`        | 201 Created | Utworzenie zasobu + Location |
| `NoContent()`              | 204 No Content | Sukces bez body |
| `BadRequest()`             | 400 Bad Request | Błąd formatu / walidacja techniczna |
| `NotFound()`               | 404 Not Found | Zasób nie istnieje |
| `Conflict()`               | 409 Conflict | Konflikt stanu zasobu |
| `UnprocessableEntity()`    | 422 Unprocessable Entity | Naruszenie reguły biznesowej |
| `StatusCode(412)`          | 412 Precondition Failed | Konflikt wersji (ETag) |
| `Unauthorized()`           | 401 Unauthorized | Brak uwierzytelnienia |
| `Forbid()`                 | 403 Forbidden | Brak uprawnień |
| `Problem()`                | dowolny     | Zwrócenie ProblemDetails |

---

## Przykłady

### 200 OK

```csharp
return Ok(orderDto);
```

---

### 201 Created

```csharp
return CreatedAtAction(
    nameof(GetById),
    new { id = order.Id },
    orderDto);
```

---

### 204 No Content

```csharp
return NoContent();
```

---

### 404 Not Found

```csharp
return NotFound();
```

---

### 409 Conflict

```csharp
return Conflict(new ProblemDetails
{
    Title = "Invalid state transition",
    Detail = ex.Message,
    Status = StatusCodes.Status409Conflict
});
```

---

### 422 Unprocessable Entity

```csharp
return UnprocessableEntity(new ProblemDetails
{
    Title = "Business rule violation",
    Detail = ex.Message,
    Status = StatusCodes.Status422UnprocessableEntity
});
```

---

### 412 Precondition Failed

```csharp
return StatusCode(StatusCodes.Status412PreconditionFailed);
```

---

## 400 vs 422 vs 409 – Różnice

| Status | Kiedy używać | Przykład |
|--------|--------------|-----------|
| 400 Bad Request | Niepoprawny format żądania | Brak pola w JSON |
| 422 Unprocessable Entity | Reguła biznesowa naruszona | quantity = 0 |
| 409 Conflict | Konflikt aktualnego stanu zasobu | Draft → Paid |

---

## 409 vs 412 – Różnice

| Status | Znaczenie | Przykład |
|--------|-----------|-----------|
| 409 Conflict | Konflikt logiki domenowej | Niedozwolone przejście stanu |
| 412 Precondition Failed | Konflikt wersji | Błędny ETag |

---

## Najczęściej używane w tym projekcie

| Sytuacja | Status |
|----------|--------|
| Zamówienie nie istnieje | 404 |
| Niedozwolone przejście stanu | 409 |
| quantity = 0 | 422 |
| Błędny ETag | 412 |
| Sukces bez body | 204 |

---

> REST to nie routing.  
> REST to poprawne użycie semantyki HTTP.