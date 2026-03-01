## 🌐 Metody HTTP

| Metoda | Opis | Idempotentna | Typowe użycie |
|---------|------|--------------|----------------|
| GET | Pobranie zasobu | ✔ Tak | Odczyt danych |
| POST | Utworzenie zasobu | ❌ Nie | Tworzenie nowych danych |
| PUT | Nadpisanie całego zasobu | ✔ Tak | Aktualizacja całości |
| PATCH | Częściowa modyfikacja zasobu | ❌ Zwykle nie | Aktualizacja fragmentu |
| DELETE | Usunięcie zasobu | ✔ Tak | Usuwanie danych |


## ❗ Statusy błędów HTTP w REST API

| Status | Nazwa | Kiedy używać | Przykład w Order API |
|--------|--------|--------------|-----------------------|
| 400 | Bad Request | Niepoprawny format żądania (np. zły JSON, błędny enum) | `status=INVALID` w query |
| 401 | Unauthorized | Brak uwierzytelnienia (brak tokenu lub token nieważny) | Brak nagłówka Authorization |
| 403 | Forbidden | Użytkownik uwierzytelniony, ale brak uprawnień | Customer próbuje usunąć zamówienie |
| 404 | Not Found | Zasób nie istnieje | `GET /orders/{id}` dla nieistniejącego ID |
| 409 | Conflict | Konflikt aktualnego stanu zasobu | Usunięcie zamówienia w stanie Placed |
| 412 | Precondition Failed | Niespełniony warunek (np. ETag) | `If-Match` niezgodny z wersją |
| 422 | Unprocessable Entity | Naruszenie reguł biznesowych | Dodanie pozycji po złożeniu zamówienia |
| 500 | Internal Server Error | Nieobsłużony błąd serwera | Wyjątek w aplikacji |

## 📦 Problem Details (RFC 7807)

| Pole | Typ | Czy wymagane | Opis | Przykład |
|------|------|--------------|------|----------|
| type | string (URI) | ✔ Zalecane | Identyfikator typu problemu | `https://example.com/problems/invalid-state` |
| title | string | ✔ | Krótki opis błędu | `Invalid state transition` |
| status | number | ✔ | Kod statusu HTTP | `409` |
| detail | string | ✖ | Szczegółowy opis problemu | `Cannot change status from Paid to Draft.` |
| instance | string (URI) | ✖ | URI żądania, które spowodowało błąd | `/orders/123` |
| extensions | object | ✖ | Dodatkowe dane (np. traceId, errorCode) | `{ "traceId": "abc123" }` |

## ⚖️ 400 vs 422 vs 409 — różnice

| Status | Kiedy używać | Co oznacza w praktyce | Przykład w Order API |
|--------|--------------|-----------------------|-----------------------|
| 400 Bad Request | Błąd techniczny w żądaniu | Serwer nie może przetworzyć żądania, bo jest niepoprawne syntaktycznie lub strukturalnie | `status=INVALID` w query, niepoprawny JSON |
| 422 Unprocessable Entity | Reguła biznesowa została naruszona | Żądanie jest poprawne technicznie, ale nie spełnia zasad domeny | Dodanie pozycji do zamówienia po jego złożeniu |
| 409 Conflict | Konflikt aktualnego stanu zasobu | Operacja nie może być wykonana, bo stan zasobu na to nie pozwala | Próba usunięcia zamówienia w stanie `Placed` |