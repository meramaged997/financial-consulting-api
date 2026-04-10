# Startawy API – Frontend Integration Package

This document is written for frontend developers integrating with the Startawy backend running locally.

## 1) Local Base URL + Swagger

- **HTTP**: `http://localhost:5150`
- **HTTPS**: `https://localhost:7289`
- **Swagger UI** (dev): `http://localhost:5150/` (root)
- **OpenAPI JSON**: `http://localhost:5150/swagger/v1/swagger.json`

All endpoints are prefixed with **`/api`**.

## 2) Standard Response Envelope (ALL endpoints)

Every endpoint returns JSON in this shape:

```json
{
  "success": true,
  "message": "Operation successful.",
  "data": {},
  "errors": null
}
```

- **success**: `true`/`false`
- **message**: human-readable message (safe to show in UI)
- **data**: response payload (object/array/null depending on endpoint)
- **errors**: `null` or `string[]` (validation/business errors)

### Common error responses

- **401 Unauthorized** (missing/invalid token):

```json
{
  "success": false,
  "message": "Unauthorized. Missing or invalid JWT token.",
  "data": null,
  "errors": null
}
```

- **403 Forbidden** (token is valid but role/policy denies access):

```json
{
  "success": false,
  "message": "Forbidden. You don't have permission to access this resource.",
  "data": null,
  "errors": null
}
```

- **400 Validation failed**:

```json
{
  "success": false,
  "message": "Validation failed.",
  "data": null,
  "errors": [
    "email: Email is required.",
    "password: Password is required."
  ]
}
```

## 3) Authentication (JWT)

### Register
- **URL**: `POST /api/auth/register`
- **Auth**: Public
- **Headers**: `Content-Type: application/json`

**Request body (example)**:

```json
{
  "fullName": "Test User",
  "email": "test@example.com",
  "password": "Test@1234",
  "confirmPassword": "Test@1234",
  "phoneNumber": "01000000000",
  "role": 1
}
```

`role` values:
- `1` = `StartupFounder`
- `2` = `FinancialConsultant`
- `3` = `Administrator`

**Response (201)**:

```json
{
  "success": true,
  "message": "Registration successful. Welcome to Startawy!",
  "data": {
    "userId": "GUID",
    "fullName": "Test User",
    "email": "test@example.com",
    "phoneNumber": "01000000000",
    "role": "StartupFounder",
    "redirectTo": "/founder/dashboard",
    "token": "JWT_TOKEN",
    "tokenExpiry": "2026-03-17T14:11:48Z"
  },
  "errors": null
}
```

### Login
- **URL**: `POST /api/auth/login`
- **Auth**: Public
- **Headers**: `Content-Type: application/json`

**Request body (example)**:

```json
{
  "email": "test@example.com",
  "password": "Test@1234"
}
```

**Response (200)**: same shape as register (token in `data.token`).

### Using the token (protected endpoints)

Add this header to every protected request:

```
Authorization: Bearer {token}
```

Example:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

## 4) CORS (Frontend on localhost)

CORS is enabled for **any localhost/127.0.0.1 origin on any port** (dev).
Your frontend can call the API from:
- `http://localhost:3000` (React)
- `http://localhost:5173` (Vite)
- `http://localhost:4200` (Angular)
- etc.

## 5) Endpoints (by feature)

Notes:
- Unless explicitly marked “Public”, endpoints are **JWT-protected**.
- Some endpoints are restricted by **role** or by **package policy**:
  - **BasicOrAbove**: requires JWT claim `package` = `Basic` or `Premium`
  - **PremiumOnly**: requires JWT claim `package` = `Premium`

Below, each endpoint includes: **URL**, **Method**, **Auth**, **Headers**, **Request**, **Response**.

### Auth (Public)

- **Register**
  - **Method**: `POST`
  - **URL**: `/api/auth/register`
  - **Auth**: Public
  - **Headers**: `Content-Type: application/json`
  - **Request**:

```json
{
  "fullName": "Test User",
  "email": "test@example.com",
  "password": "Test@1234",
  "confirmPassword": "Test@1234",
  "phoneNumber": "01000000000",
  "role": 1
}
```

  - **Response (201)**:

```json
{
  "success": true,
  "message": "Registration successful. Welcome to Startawy!",
  "data": {
    "userId": "GUID",
    "fullName": "Test User",
    "email": "test@example.com",
    "phoneNumber": "01000000000",
    "role": "StartupFounder",
    "redirectTo": "/founder/dashboard",
    "token": "JWT_TOKEN",
    "tokenExpiry": "2026-03-17T14:11:48Z"
  },
  "errors": null
}
```

- **Login**
  - **Method**: `POST`
  - **URL**: `/api/auth/login`
  - **Auth**: Public
  - **Headers**: `Content-Type: application/json`
  - **Request**:

```json
{
  "email": "test@example.com",
  "password": "Test@1234"
}
```

  - **Response (200)**:

```json
{
  "success": true,
  "message": "Welcome back, Test User!",
  "data": {
    "token": "JWT_TOKEN",
    "tokenExpiry": "2026-03-17T14:11:48Z",
    "role": "StartupFounder"
  },
  "errors": null
}
```

### Packages

- **List packages**
  - **Method**: `GET`
  - **URL**: `/api/packages`
  - **Auth**: Public
  - **Headers**: (none)
  - **Request**: (none)
  - **Response (200)**:

```json
{
  "success": true,
  "message": "Operation successful.",
  "data": [
    {
      "packageId": "GUID_OR_STRING",
      "type": "Free",
      "description": "Free trial plan...",
      "price": 0,
      "durationDays": null
    }
  ],
  "errors": null
}
```

- **Get package**
  - **Method**: `GET`
  - **URL**: `/api/packages/{id}`
  - **Auth**: Public
  - **Headers**: (none)
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<PackageResponse>`

- **Create package**
  - **Method**: `POST`
  - **URL**: `/api/packages`
  - **Auth**: JWT (Role: `Administrator`)
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{
  "type": "Basic",
  "description": "Basic plan",
  "price": 299,
  "durationDays": 30,
  "unlimitedAi": true,
  "unlimitedAnalysis": true
}
```

  - **Response (201)**: `ApiResponse<PackageResponse>`

- **Update package**
  - **Method**: `PUT`
  - **URL**: `/api/packages/{id}`
  - **Auth**: JWT (Role: `Administrator`)
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{
  "type": "Premium",
  "description": "Premium plan",
  "price": 599,
  "durationDays": 30,
  "followUpDurationDays": 30,
  "consultantReview": true,
  "consultantSupport": true
}
```

  - **Response (200)**: `ApiResponse<PackageResponse>`

- **Delete package**
  - **Method**: `DELETE`
  - **URL**: `/api/packages/{id}`
  - **Auth**: JWT (Role: `Administrator`)
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**:

```json
{ "success": true, "message": "Package deleted.", "data": null, "errors": null }
```

### Dashboard (JWT)

- **Get dashboard**
  - **Method**: `GET`
  - **URL**: `/api/Dashboard`
  - **Auth**: JWT
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<DashboardResponse>`

### AI Chat (JWT)

- **Send chat message**
  - **Method**: `POST`
  - **URL**: `/api/ai/chat`
  - **Auth**: JWT
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{
  "message": "Hello",
  "sessionId": null
}
```

  - **Response (200)**: `ApiResponse<ChatResponse>`

- **List chat sessions**
  - **Method**: `GET`
  - **URL**: `/api/ai/sessions`
  - **Auth**: JWT
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<ChatSessionResponse[]>`

- **Get chat history**
  - **Method**: `GET`
  - **URL**: `/api/ai/sessions/{sessionId}`
  - **Auth**: JWT
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<ChatMessageResponse[]>`

### Budget (JWT)

- **Create budget analysis**
  - **Method**: `POST`
  - **URL**: `/api/Budget`
  - **Auth**: JWT
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{
  "businessName": "My Startup",
  "industry": "SaaS",
  "period": "2026-Q1",
  "lineItems": [
    {
      "category": "Marketing",
      "description": "Ads",
      "plannedAmount": 2000,
      "actualAmount": 1500,
      "type": "Expense"
    }
  ]
}
```

  - **Response (201)**: `ApiResponse<BudgetAnalysisResponse>`

- **List my budget analyses**
  - **Method**: `GET`
  - **URL**: `/api/Budget`
  - **Auth**: JWT
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**:

```json
{ "success": true, "message": "Operation successful.", "data": [], "errors": null }
```

- **Get budget analysis by id**
  - **Method**: `GET`
  - **URL**: `/api/Budget/{id}`
  - **Auth**: JWT
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<BudgetAnalysisResponse>`

- **Delete budget analysis**
  - **Method**: `DELETE`
  - **URL**: `/api/Budget/{id}`
  - **Auth**: JWT
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**:

```json
{ "success": true, "message": "Operation successful.", "data": null, "errors": null }
```

### Cash Flow (Policy: BasicOrAbove)

- **Create cash flow forecast**
  - **Method**: `POST`
  - **URL**: `/api/CashFlow`
  - **Auth**: JWT (Policy: `BasicOrAbove`)
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{
  "businessName": "My Startup",
  "initialCashBalance": 10000,
  "monthlyRevenueTrend": 5000,
  "monthlyExpenseTrend": 3000,
  "growthRate": 0.1,
  "forecastMonths": 12
}
```

  - **Response (201)**: `ApiResponse<CashFlowForecastResponse>`

- **List my cash flow forecasts**
  - **Method**: `GET`
  - **URL**: `/api/CashFlow`
  - **Auth**: JWT (Policy: `BasicOrAbove`)
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**:

```json
{ "success": true, "message": "Operation successful.", "data": [], "errors": null }
```

### Financial (Policy: PremiumOnly)

- **Create financial statement**
  - **Method**: `POST`
  - **URL**: `/api/Financial/statements`
  - **Auth**: JWT (Policy: `PremiumOnly`)
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{
  "type": "IncomeStatement",
  "period": "2026-Q1",
  "statementDate": "2026-03-17T00:00:00Z",
  "grossRevenue": 50000,
  "costOfGoodsSold": 15000,
  "operatingExpenses": 20000,
  "netIncome": 15000,
  "totalAssets": 80000,
  "totalLiabilities": 30000,
  "operatingCashFlow": 12000,
  "investingCashFlow": -2000,
  "financingCashFlow": 1000
}
```

  - **Response (201)**: `ApiResponse<FinancialStatementResponse>`

- **List my financial statements**
  - **Method**: `GET`
  - **URL**: `/api/Financial/statements?type=IncomeStatement` (type optional)
  - **Auth**: JWT (Policy: `PremiumOnly`)
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<FinancialStatementResponse[]>`

- **Get risk analysis**
  - **Method**: `GET`
  - **URL**: `/api/Financial/statements/{id}/risk`
  - **Auth**: JWT (Policy: `PremiumOnly`)
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<object>`

### Market Research (Policy: BasicOrAbove)

- **Create market research**
  - **Method**: `POST`
  - **URL**: `/api/MarketResearch`
  - **Auth**: JWT (Policy: `BasicOrAbove`)
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{
  "industry": "FinTech",
  "targetMarket": "SMEs",
  "geographicScope": "Egypt",
  "keywords": ["payments", "lending"],
  "knownCompetitors": ["Competitor A", "Competitor B"]
}
```

  - **Response (201)**: `ApiResponse<MarketResearchResponse>`

- **List my market researches**
  - **Method**: `GET`
  - **URL**: `/api/MarketResearch`
  - **Auth**: JWT (Policy: `BasicOrAbove`)
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<MarketResearchResponse[]>`

- **Get market research by id**
  - **Method**: `GET`
  - **URL**: `/api/MarketResearch/{id}`
  - **Auth**: JWT (Policy: `BasicOrAbove`)
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<MarketResearchResponse>`

### Marketing (Policy: PremiumOnly)

- **Create marketing campaign**
  - **Method**: `POST`
  - **URL**: `/api/Marketing`
  - **Auth**: JWT (Policy: `PremiumOnly`)
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{
  "campaignName": "Launch Campaign",
  "businessType": "SaaS",
  "targetAudience": "Founders",
  "budget": 3000,
  "productDescription": "Product details",
  "uniqueValueProposition": "We do X better",
  "startDate": "2026-03-20T00:00:00Z",
  "endDate": "2026-04-20T00:00:00Z"
}
```

  - **Response (201)**: `ApiResponse<MarketingCampaignResponse>`

- **List my marketing campaigns**
  - **Method**: `GET`
  - **URL**: `/api/Marketing`
  - **Auth**: JWT (Policy: `PremiumOnly`)
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<MarketingCampaignResponse[]>`

- **Update campaign status**
  - **Method**: `PATCH`
  - **URL**: `/api/Marketing/{id}/status`
  - **Auth**: JWT (Policy: `PremiumOnly`)
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request** (enum as JSON string):

```json
"Active"
```

  - **Response (200)**: `ApiResponse<MarketingCampaignResponse>`

### Consultations (JWT)

- **Create consultation request**
  - **Method**: `POST`
  - **URL**: `/api/Consultations`
  - **Auth**: JWT
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{
  "subject": "Need pricing help",
  "description": "I want feedback on my pricing model",
  "type": "FinancialPlanning",
  "preferredDate": "2026-03-25T10:00:00Z"
}
```

  - **Response (201)**: `ApiResponse<ConsultationResponse>`

- **List my consultations**
  - **Method**: `GET`
  - **URL**: `/api/Consultations/mine`
  - **Auth**: JWT
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<ConsultationResponse[]>`

- **List all consultations**
  - **Method**: `GET`
  - **URL**: `/api/Consultations/all`
  - **Auth**: JWT (Roles: `FinancialConsultant`, `Administrator`)
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<ConsultationResponse[]>`

- **Update consultation status**
  - **Method**: `PATCH`
  - **URL**: `/api/Consultations/{id}/status`
  - **Auth**: JWT (Roles: `FinancialConsultant`, `Administrator`)
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{ "status": "Approved", "notes": "Scheduled for next week" }
```

  - **Response (200)**: `ApiResponse<ConsultationResponse>`

### Sessions (JWT, mixed)

- **Create availability slot**
  - **Method**: `POST`
  - **URL**: `/api/sessions/availability`
  - **Auth**: JWT (Role: `FinancialConsultant`)
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{ "startAtUtc": "2026-03-20T10:00:00Z", "endAtUtc": "2026-03-20T11:00:00Z" }
```

  - **Response (200)**: `ApiResponse<AvailabilitySlotResponse>`

- **Get consultant availability (Public)**
  - **Method**: `GET`
  - **URL**: `/api/sessions/availability/{consultantUserId}?fromUtc=2026-03-20T00:00:00Z&toUtc=2026-03-27T00:00:00Z`
  - **Auth**: Public
  - **Headers**: (none)
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<AvailabilitySlotResponse[]>`

- **Book a session**
  - **Method**: `POST`
  - **URL**: `/api/sessions/book`
  - **Auth**: JWT (Role: `StartupFounder`)
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{ "availabilitySlotId": 123, "paymentMethod": "Card" }
```

  - **Response (200)**: `ApiResponse<ConsultationSessionResponse>`

- **Get my sessions (Founder)**
  - **Method**: `GET`
  - **URL**: `/api/sessions/mine`
  - **Auth**: JWT (Role: `StartupFounder`)
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<ConsultationSessionResponse[]>`

- **Get my sessions (Consultant)**
  - **Method**: `GET`
  - **URL**: `/api/sessions/consultant`
  - **Auth**: JWT (Role: `FinancialConsultant`)
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<ConsultationSessionResponse[]>`

- **Complete a session**
  - **Method**: `PATCH`
  - **URL**: `/api/sessions/{sessionId}/complete`
  - **Auth**: JWT (Role: `FinancialConsultant`)
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{ "notes": "Session notes", "recommendations": "Next steps..." }
```

  - **Response (200)**: `ApiResponse<ConsultationSessionResponse>`

### Feedback (JWT)

- **Submit feedback**
  - **Method**: `POST`
  - **URL**: `/api/feedback`
  - **Auth**: JWT (Role: `StartupFounder`)
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{ "message": "The dashboard is great, but I need export." }
```

  - **Response (200)**: `ApiResponse<FeedbackResponse>`

- **List all feedback**
  - **Method**: `GET`
  - **URL**: `/api/feedback`
  - **Auth**: JWT (Role: `Administrator`)
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<FeedbackResponse[]>`

- **Review feedback**
  - **Method**: `PATCH`
  - **URL**: `/api/feedback/{feedbackId}/review`
  - **Auth**: JWT (Role: `Administrator`)
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{ "category": "Suggestion", "markReviewed": true }
```

  - **Response (200)**: `ApiResponse<FeedbackResponse>`

### Follow-up Plans (JWT)

- **Create follow-up plan**
  - **Method**: `POST`
  - **URL**: `/api/follow-up-plans`
  - **Auth**: JWT (Role: `FinancialConsultant`)
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{
  "founderUserId": "FOUNDER_USER_ID",
  "goal": "Improve cash runway",
  "timelineStartUtc": "2026-03-17T00:00:00Z",
  "timelineEndUtc": "2026-04-17T00:00:00Z",
  "steps": [
    {
      "title": "Reduce burn",
      "description": "Cut non-essential expenses",
      "dueAtUtc": "2026-03-24T00:00:00Z"
    }
  ]
}
```

  - **Response (200)**: `ApiResponse<FollowUpPlanResponse>`

- **Get my follow-up plans**
  - **Method**: `GET`
  - **URL**: `/api/follow-up-plans/mine`
  - **Auth**: JWT (Role: `StartupFounder`)
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<FollowUpPlanResponse[]>`

### Payments (JWT)

- **Create payment intent**
  - **Method**: `POST`
  - **URL**: `/api/payments/intent`
  - **Auth**: JWT
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
    - Optional: `Idempotency-Key: <any unique string>`
  - **Request**:

```json
{
  "purpose": "SubscriptionUpgrade",
  "packageId": "PACKAGE_ID",
  "availabilitySlotId": null,
  "paymentMethod": "Card"
}
```

  - **Response (200)**: `ApiResponse<PaymentIntentResponse>`

- **Confirm payment**
  - **Method**: `POST`
  - **URL**: `/api/payments/{transactionId}/confirm`
  - **Auth**: JWT
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{ "succeeded": true, "externalReference": "stripe_pi_123" }
```

  - **Response (200)**: `ApiResponse<PaymentResponse>`

### Subscriptions (JWT)

- **Get my subscription**
  - **Method**: `GET`
  - **URL**: `/api/subscriptions/me`
  - **Auth**: JWT
  - **Headers**: `Authorization: Bearer {token}`
  - **Request**: (none)
  - **Response (200)**: `ApiResponse<SubscriptionResponse>`

- **Upgrade package**
  - **Method**: `POST`
  - **URL**: `/api/subscriptions/upgrade`
  - **Auth**: JWT
  - **Headers**:
    - `Authorization: Bearer {token}`
    - `Content-Type: application/json`
  - **Request**:

```json
{ "packageId": "PACKAGE_ID", "paymentMethod": "Card" }
```

  - **Response (200)**: `ApiResponse<SubscriptionResponse>`

## 6) Ready-to-use Frontend Code (Axios + Fetch)

### Axios setup

```js
import axios from "axios";

export const api = axios.create({
  baseURL: "http://localhost:5150",
  headers: { "Content-Type": "application/json" },
});

export function setAuthToken(token) {
  if (token) api.defaults.headers.common.Authorization = `Bearer ${token}`;
  else delete api.defaults.headers.common.Authorization;
}
```

### Login (Axios)

```js
import { api, setAuthToken } from "./api";

export async function login(email, password) {
  const { data } = await api.post("/api/auth/login", { email, password });
  if (!data.success) throw new Error(data.message);
  setAuthToken(data.data.token);
  return data.data; // contains token, role, redirectTo, ...
}
```

### Register (Axios)

```js
import { api, setAuthToken } from "./api";

export async function register(payload) {
  const { data } = await api.post("/api/auth/register", payload);
  if (!data.success) throw new Error(data.message);
  setAuthToken(data.data.token);
  return data.data;
}
```

### Call a protected endpoint (Axios)

```js
import { api } from "./api";

export async function getBudgetHistory() {
  const { data } = await api.get("/api/Budget");
  if (!data.success) throw new Error(data.message);
  return data.data; // array
}
```

### Login (Fetch)

```js
const BASE_URL = "http://localhost:5150";

export async function loginFetch(email, password) {
  const res = await fetch(`${BASE_URL}/api/auth/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password }),
  });
  const json = await res.json();
  if (!res.ok || !json.success) throw new Error(json.message);
  localStorage.setItem("token", json.data.token);
  return json.data;
}
```

### Protected call (Fetch)

```js
const BASE_URL = "http://localhost:5150";

export async function getDashboard() {
  const token = localStorage.getItem("token");
  const res = await fetch(`${BASE_URL}/api/Dashboard`, {
    headers: { Authorization: `Bearer ${token}` },
  });
  const json = await res.json();
  if (!res.ok || !json.success) throw new Error(json.message);
  return json.data;
}
```

---

If you need exact request/response field names for any DTO, open Swagger UI at `http://localhost:5150/` and expand the endpoint — the schema is documented there.

