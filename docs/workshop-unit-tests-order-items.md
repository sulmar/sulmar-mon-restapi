# Warsztat: Testy jednostkowe 

## 🎯 Cel ćwiczenia

Celem zadania jest samodzielne pisanie testów jednostkowych dla logiki zamówienia i pozycji zamówienia (Order, OrderItem) w warstwie domeny.

Ćwiczenie rozwija umiejętność:

- stosowania wzorca Arrange-Act-Assert (AAA)
- stosowania konwencji nazewnictwa testów (`{Metoda}_{Scenariusz}_{OczekiwaneZachowanie}`)
- weryfikacji stanu obiektu i wartości wyliczanych (np. Total, LineTotal)
- pisania testów w xUnit (Fact, Theory, InlineData)

---

## 📘 Kontekst

W projekcie **OrderApi.Domain.UnitTests** znajdują się już testy dla `Order`:

- tworzenie zamówienia (`Order.Create`) – stan Draft, Version 1, pusta lista Items
- dodanie pozycji w stanie Draft (`AddItem`) – wzrost Version, jedna pozycja z poprawnym LineTotal
- dodanie pozycji przy statusie innym niż Draft – `InvalidStateTransitionException`

Kod domeny: `Order` (metoda `AddItem`, właściwość `Total`), `OrderItem` (właściwość `LineTotal`).

Twoim zadaniem jest uzupełnienie zestawu testów o poniższe scenariusze.

---

## 🧩 Zadanie 1 – Test Total przy wielu pozycjach

Napisz test jednostkowy, który weryfikuje, że po dodaniu **kilku** pozycji do zamówienia (np. 2–3 wywołania `AddItem`) wartość `order.Total` jest równa sumie `LineTotal` wszystkich pozycji.

### 📌 Wskazówki

- Zamówienie musi być w stanie Draft.
- W Arrange: utwórz zamówienie, przygotuj kilka par (productId, quantity, unitPrice).
- W Act: wywołaj `AddItem` dla każdej pozycji.
- W Assert: sprawdź `order.Total` oraz ewentualnie liczbę pozycji w `order.Items`.

### 🌐 Konwencja nazewnictwa

Przykład: `AddItem_MultipleItems_TotalEqualsSumOfLineTotals` (dostosuj nazwę do swojej konwencji).

---

## 🧩 Zadanie 2 – Testy stanów zamówienia (Order)

Napisz testy jednostkowe weryfikujące **przejścia stanów** zamówienia (metoda `TransitionTo`). Cykl życia: `Draft → Placed → Paid` (oraz Cancel z Draft lub Placed).

### 📌 Wymagania

- **Przejście poprawne:** Np. zamówienie w stanie Draft po `TransitionTo(OrderStatus.Placed)` ma `Status == OrderStatus.Placed`. Analogicznie Placed → Paid.
- **Przejście niepoprawne:** Np. wywołanie `TransitionTo(OrderStatus.Paid)` gdy zamówienie jest w Draft (bez wcześniejszego Placed) powinno rzucać wyjątek (np. `InvalidStateTransitionException` lub `InvalidOperationException` – sprawdź implementację).
- W Assert weryfikuj `order.Status` po poprawnym przejściu oraz typ wyjątku przy niepoprawnym.

### 🌐 Konwencja nazewnictwa

Przykłady: `TransitionTo_FromDraftToPlaced_StatusIsPlaced`, `TransitionTo_FromDraftToPaid_ThrowsException`.

---

## 🧩 Zadanie 3 – Test usunięcie pozycji
Gdy w agregacie `Order` pojawi się metoda `RemoveItem(productId)`:

- Napisz test: usunięcie pozycji w stanie Draft – pozycja znika z `Items`, `Version` się zwiększa.
- Napisz test: wywołanie `RemoveItem` przy statusie innym niż Draft – oczekiwany wyjątek (np. `InvalidStateTransitionException`).

