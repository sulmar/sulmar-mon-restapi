# 🧪 Zadanie warsztatowe — ograniczenie liczby żądań

System Order API jest używany przez różne aplikacje klienckie.
Aby zapobiec przeciążeniu systemu, należy ograniczyć liczbę żądań wykonywanych przez jednego klienta.

Twoim zadaniem jest wprowadzenie mechanizmu ograniczającego liczbę żądań HTTP.

---

## Wymagania

1. Każde żądanie HTTP powinno mieć identyfikator:
```
X-Correlation-Id
```

2. Jeśli klient przekaże nagłówek `X-Correlation-Id`, system powinien użyć tej wartości.

3. Jeśli nagłówek nie istnieje — system powinien wygenerować nowy identyfikator.

4. System powinien ograniczyć liczbę żądań dla jednego identyfikatora.

Limit:
```
maksymalnie 5 żądań na minutę
```

5.  Jeśli limit zostanie przekroczony, API powinno zwrócić:

```
429 Too Many Requests
```

---

## Przykład

Request:
```
GET /orders
X-Correlation-Id: client-123
```

Jeśli klient przekroczy limit:
```
429 Too Many Requests
```

## Wskazówki
- rozwiązanie powinno działać dla wszystkich endpointów
- identyfikator żądania powinien być dostępny dla mechanizmu ograniczeń
- zastanów się gdzie przechowywać informacje o liczbie żądań
- zastanów się gdzie powinno odbywać się sprawdzanie limitu

---

## 💡 **Wskazówka techniczna**

Jeśli potrzebujesz wygenerować identyfikator żądania, możesz użyć np.:

- `Guid.NewGuid()`
- lub biblioteki **NanoId** (generuje krótsze identyfikatory)

Przykład:

```csharp
Nanoid.Generate()
```

Wynik:
```
a8F2xK91Lm
```

---

## Cel ćwiczenia
Zrozumienie:
- pipeline HTTP
- middleware
- współdzielenie danych pomiędzy komponentami aplikacji
- projektowanie mechanizmów ochrony API