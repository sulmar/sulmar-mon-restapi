# Order Management API – Specyfikacja Kontraktu HTTP

Wersja: 1.0  
Status: Draft  
Cel: Szkolenie – Projektowanie REST API w .NET 9  

---

# 1. Wprowadzenie

Dokument definiuje kontrakt HTTP dla systemu Order Management API.

API modeluje zachowanie biznesowe przy użyciu semantyki HTTP.  
Jest niezależne od sposobu implementacji (MVC lub Minimal APIs).

W kontekście tego projektu REST oznacza:

- Jawne modelowanie zasobów
- Wykorzystanie semantyki metod HTTP
- Traktowanie kodów statusu jako części kontraktu
- Mapowanie reguł biznesowych na odpowiedzi HTTP

---

# 2. Przegląd domeny

System zarządza zamówieniami klientów.

Zamówienie:

- Jest tworzone przez klienta
- Zawiera pozycje zamówienia
- Posiada cykl życia (maszynę stanów)
- Egzekwuje reguły biznesowe
- Wspiera optymistyczną kontrolę współbieżności


## 2.1 Optymistyczna kontrola współbieżności

System wspiera optymistyczną kontrolę współbieżności, co oznacza, że:

- Nie blokuje zasobu podczas jego odczytu.
- Zakłada, że konflikty zapisu zdarzają się rzadko.
- Wykrywa konflikt dopiero w momencie modyfikacji.

Każde zamówienie posiada pole `version`, które jest zwiększane przy każdej zmianie stanu.

Podczas pobierania zasobu serwer zwraca nagłówek:

ETag: "version"

Podczas operacji modyfikujących klient może przesłać nagłówek:

If-Match: "version"

Jeżeli wersja przesłana przez klienta nie zgadza się z aktualną wersją zasobu, serwer zwraca:

- 412 Precondition Failed

Dzięki temu system zapobiega nadpisaniu zmian wykonanych przez innego użytkownika.

---

# 3. Reprezentacja zasobu Order

```
{
  "id": "guid",
  "customerId": "guid",
  "status": "Draft | Placed | Paid | Shipped | Cancelled",
  "createdAt": "2026-03-01T10:00:00Z",
  "version": 3,
  "items": [
    {
      "productId": "guid",
      "quantity": 2,
      "unitPrice": 100,
      "lineTotal": 200
    }
  ],
  "totalAmount": 200
}
```

---

# 4. Cykl życia zamówienia

Dozwolone przejścia stanów:

Draft → Placed  
Placed → Paid  
Paid → Shipped  
Draft → Cancelled  
Placed → Cancelled  

Stany końcowe:
- Shipped
- Cancelled

Niedozwolone przejścia skutkują odpowiedzią **409 Conflict**.

---

# 5. Endpointy

## 5.1 Utworzenie zamówienia

POST /orders

Request:

```
{
  "customerId": "guid"
}
```

Odpowiedzi:

- 201 Created (z nagłówkiem Location)
- 400 Bad Request

---

## 5.2 Pobranie zamówienia

GET /orders/{id}

Odpowiedzi:

- 200 OK
- 404 Not Found

---

## 5.3 Lista zamówień

GET /orders?page=1&pageSize=20&status=Placed

Odpowiedzi:

- 200 OK

---

## 5.4 Dodanie pozycji do zamówienia

POST /orders/{id}/items

Request:

```
{
  "productId": "guid",
  "quantity": 3,
  "unitPrice": 50
}
```

Odpowiedzi:

- 204 No Content
- 404 Not Found
- 409 Conflict (zamówienie nie jest w stanie Draft)
- 422 Unprocessable Entity (naruszenie reguł biznesowych)

---

## 5.5 Zmiana statusu zamówienia

PATCH /orders/{id}

Request:

```
{
  "status": "Placed"
}
```

Nagłówki (opcjonalnie – kontrola współbieżności):

If-Match: "3"

Odpowiedzi:

- 204 No Content
- 404 Not Found
- 409 Conflict (niedozwolone przejście stanu)
- 412 Precondition Failed (niezgodność wersji / ETag)
- 422 Unprocessable Entity (naruszenie reguły biznesowej)

---


## 5.6 Usunięcie zamówienia

DELETE /orders/{id}

Request:

(brak body)

Odpowiedzi:
- 204 No Content
- 404 Not Found
- 409 Conflict (zamówienie nie jest w stanie Draft)
    
---

## 5.7 Usunięcie pozycji z zamówienia

DELETE /orders/{id}/items/{productId}

Request:

(brak body)

Odpowiedzi:
- 204 No Content
- 404 Not Found (zamówienie nie istnieje)
- 404 Not Found (pozycja nie istnieje w zamówieniu)
- 409 Conflict (zamówienie nie jest w stanie Draft)

---

# 6. Kontrola współbieżności (ETag)

Odpowiedź GET zawiera nagłówek:

ETag: "version"

Żądania modyfikujące mogą zawierać:

If-Match: "version"

Jeżeli wersja nie jest zgodna:

- 412 Precondition Failed

---

# 7. Obsługa błędów – Problem Details (RFC 7807)

Błędy zwracane są w ustandaryzowanym formacie:

```
{
  "type": "https://example.com/problems/invalid-state-transition",
  "title": "Invalid state transition",
  "status": 409,
  "detail": "Order cannot be cancelled after shipment.",
  "instance": "/orders/{id}"
}
```

Znaczenie kodów statusu:

- 400 – Nieprawidłowy format żądania
- 404 – Zasób nie istnieje
- 409 – Konflikt aktualnego stanu
- 412 – Konflikt wersji (współbieżność)
- 422 – Naruszenie reguły biznesowej
- 500 – Nieoczekiwany błąd serwera

---

# 8. Zasady projektowe

- REST modeluje zasoby, nie akcje.
- Kody statusu HTTP są częścią kontraktu biznesowego.
- Reguły biznesowe determinują odpowiedzi HTTP.
- Kontrakt jest niezależny od technologii implementacji.
- Logika domenowa nie powinna znajdować się w kontrolerach.

---

# 9. Zakres dokumentu

Dokument definiuje:

- Endpointy HTTP
- Format żądań i odpowiedzi
- Semantykę kodów statusu
- Mechanizm kontroli współbieżności
- Format odpowiedzi błędów

Szczegóły implementacyjne są celowo wyłączone z zakresu tego dokumentu.