# Order API – Source Code

Katalog `src/` zawiera implementację aplikacji Order Management API.

Projekt został przygotowany jako materiał szkoleniowy do kursu:

> Tworzenie usług REST API w .NET 9

---

# Struktura katalogu

```
src/
│
├── OrderApi.Minimal/        → Implementacja Minimal API
├── OrderApi.Mvc/            → Implementacja MVC
├── OrderApi.Domain/         → Model domenowy (reguły biznesowe)
├── OrderApi.Application/    → Warstwa przypadków użycia (opcjonalnie)
└── OrderApi.Infrastructure/ → Repozytoria i integracje
```

---

# Założenia architektoniczne

- REST traktujemy jako kontrakt HTTP
- Logika biznesowa znajduje się w modelu domenowym
- Warstwa HTTP mapuje wyjątki domenowe na statusy HTTP
- Repozytorium abstrahuje warstwę przechowywania danych
- Minimal API i MVC implementują ten sam kontrakt

---

# Odpowiedzialności warstw

## Domain

Zawiera:

- agregat `Order`
- `OrderItem`
- `OrderStatus`
- wyjątki domenowe
- reguły biznesowe

---

## Minimal / MVC

Odpowiadają za:

- mapowanie endpointów
- obsługę żądań HTTP
- walidację wejścia
- mapowanie wyjątków domenowych na HTTP
- obsługę ETag

---

## Infrastructure

Zawiera:

- implementacje repozytoriów
- integracje z bazą danych


---

# Uruchomienie aplikacji

Z poziomu katalogu głównego projektu:

```bash
dotnet run --project src/OrderApi.Minimal
```

lub

```bash
dotnet run --project src/OrderApi.Mvc
```


