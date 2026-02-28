# Warsztat: Implementacja pozycji zamówienia

## 🎯 Cel ćwiczenia

Celem zadania jest samodzielne zaimplementowanie obsługi pozycji zamówienia zgodnie z przygotowanym kontraktem HTTP.

Ćwiczenie rozwija umiejętność:

- czytania kontraktu REST API
- mapowania reguł biznesowych na HTTP
- projektowania logiki domenowej
- rozróżniania 409 i 422
- testowania API przy użyciu plików .http

---

## 📘 Kontekst

System Order API umożliwia:

- tworzenie zamówienia
- pobieranie zamówienia
- zmianę statusu zamówienia
- usunięcie zamówienia

Twoim zadaniem jest rozszerzenie systemu o obsługę pozycji zamówienia.

---

## 🧩 Zadanie 1 – Dodanie pozycji

Zaimplementuj endpoint:

```
POST /orders/{id}/items
```

**Request**

```json
{
  "productId": "guid",
  "quantity": 3,
  "unitPrice": 50
}
```

### 📌 Wymagania biznesowe

1. Pozycję można dodać tylko w stanie Draft
2. `quantity` musi być większe od 0
3. `unitPrice` nie może być ujemne
4. Po dodaniu pozycji:
   - zwiększa się Version
   - przelicza się TotalAmount

### 🌐 Odpowiedzi HTTP

- **204 No Content** – sukces
- **404 Not Found** – zamówienie nie istnieje
- **409 Conflict** – zamówienie nie jest w stanie Draft
- **422 Unprocessable Entity** – naruszenie reguł biznesowych

---

## 🔎 Zadanie 2 – Testowanie scenariuszy błędnych

Sprawdź następujące sytuacje:

1. Dodanie pozycji przy statusie Placed
2. Dodanie pozycji z `quantity = 0`
3. Dodanie pozycji z ujemną ceną

Dla każdego przypadku odpowiedz:

- Jaki status HTTP został zwrócony?
- Dlaczego?
- Czy odpowiedź jest zgodna z kontraktem?

---

## ➕ Zadanie 3 – Usunięcie pozycji (rozszerzenie)

Zaimplementuj:

```
DELETE /orders/{id}/items/{productId}
```

### Wymagania

- Usuwanie tylko w stanie Draft
- **204** – sukces
- **404** – brak zamówienia lub brak pozycji
- **409** – zamówienie nie jest w stanie Draft
