# Tworzenie usług REST API w .NET 9

Szkolenie koncentruje się na projektowaniu i budowie REST API jako **kontraktu HTTP i modelu zachowania**, niezależnego od sposobu implementacji (MVC vs Minimal APIs).

REST w tym szkoleniu oznacza:

- Modelowanie zasobów
- Wykorzystanie semantyki HTTP
- Świadome użycie kodów statusu
- Mapowanie reguł biznesowych na odpowiedzi HTTP
- Oddzielenie kontraktu od implementacji

---

# 🎯 Cel szkolenia

Uczestnik po szkoleniu:

- Rozumie REST jako kontrakt komunikacyjny
- Potrafi zaprojektować poprawne HTTP API
- Rozróżnia 400 / 409 / 412 / 422
- Rozumie różnicę między konfliktem stanu a błędem biznesowym
- Potrafi zaimplementować API w MVC i Minimal APIs
- Wie jak testować API na poziomie HTTP

---

# 🧩 Scenariusz biznesowy

Budujemy system **Order Management API**, który:

- pozwala tworzyć zamówienia
- umożliwia zarządzanie pozycjami
- obsługuje cykl życia zamówienia
- egzekwuje reguły biznesowe
- wspiera optymistyczną kontrolę współbieżności (ETag)

---

# 🏗 Struktura repozytorium

```
sulmar-mon-restapi/
│
├── docs/
│   ├── order-api-http-contract.md
│   ├── domain-model.md
│   └── architecture-notes.md
│
├── http/
│   ├── orders.http
│   ├── error-scenarios.http
│   └── concurrency.http
│
├── src/
│   ├── OrderApi.Mvc/
│   ├── OrderApi.Minimal/
│   ├── OrderApi.Application/
│   ├── OrderApi.Domain/
│   └── OrderApi.Infrastructure/
│
└── tests/
├── OrderApi.Domain.Tests/
└── OrderApi.IntegrationTests/
```


---

# 📄 Dokumentacja kontraktu

Kontrakt HTTP znajduje się w: `docs/order-api-http-contract.md`

To dokument referencyjny – implementacja musi być z nim zgodna.


# Cykl życia zamówienia

Dozwolone przejścia:

```
Draft → Placed
Placed → Paid
Paid → Shipped
Draft → Cancelled
Placed → Cancelled
```

Niedozwolone przejścia skutkują `409 Conflict`.

---

# ⚠️ Statusy HTTP używane w projekcie

| Status | Znaczenie |
|--------|-----------|
| 200 | Sukces |
| 201 | Utworzenie zasobu |
| 204 | Sukces bez treści |
| 400 | Błąd formatu |
| 404 | Zasób nie istnieje |
| 409 | Konflikt stanu |
| 412 | Konflikt wersji (ETag) |
| 422 | Naruszenie reguły biznesowej |
| 500 | Błąd serwera |

---

# Kluczowe założenia architektoniczne

- REST ≠ framework
- HTTP to semantyka, nie tylko routing
- Reguły biznesowe nie są błędami technicznymi
- Logika domenowa nie należy do kontrolerów
- Kontrakt powinien być stabilny, implementacja może ewoluować

---

# 🚀 Jak uruchomić projekt

```bash
dotnet build
dotnet run –project src/OrderApi.Mvc
```

lub

```bash
dotnet run –project src/OrderApi.Minimal
```


---

# 📌 Ważne

To repozytorium jest materiałem szkoleniowym.  
Jego celem jest pokazanie poprawnego modelowania REST API, a nie dostarczenie gotowego systemu produkcyjnego.

---

# 👨‍🏫 Autor szkolenia

Szkolenie: Tworzenie usług REST API w .NET 9  
Tematyka: REST, HTTP, architektura, projektowanie API