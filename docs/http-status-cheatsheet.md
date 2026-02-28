# HTTP Status & Minimal API – Cheat Sheet

Tabela pomocnicza używana podczas szkolenia  
Tworzenie usług REST API w .NET 9

---

## Results.* → HTTP Status

| Results Method                     | HTTP Status | Znaczenie |
|------------------------------------|-------------|------------|
| `Results.Ok()`                    | 200 OK      | Zwracamy dane |
| `Results.Created()`               | 201 Created | Utworzenie zasobu |
| `Results.NoContent()`             | 204 No Content | Sukces bez body |
| `Results.BadRequest()`            | 400 Bad Request | Błąd formatu / walidacja techniczna |
| `Results.NotFound()`              | 404 Not Found | Zasób nie istnieje |
| `Results.Conflict()`              | 409 Conflict | Konflikt stanu zasobu |
| `Results.UnprocessableEntity()`   | 422 Unprocessable Entity | Naruszenie reguły biznesowej |
| `Results.StatusCode(412)`         | 412 Precondition Failed | Konflikt wersji (ETag) |
| `Results.Unauthorized()`          | 401 Unauthorized | Brak uwierzytelnienia |
| `Results.Forbid()`                | 403 Forbidden | Brak uprawnień |
| `Results.Problem()`               | dowolny     | Zwrócenie ProblemDetails |

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