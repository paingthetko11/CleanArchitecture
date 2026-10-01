# .NET 10 Clean Architecture Template

ASP.NET Core Web API starter project ကို RxiFinTrack ရဲ့ `aspnet-clean-architecture-skill` နှင့် Hybrid Clean Architecture စည်းမျဉ်းများအတိုင်း ဖွဲ့စည်းထားပါတယ်။ `dotnet new` template အဖြစ်လည်း အသုံးပြုနိုင်ပါတယ်။

## လိုအပ်ချက်များ

- .NET 10 SDK

## Project ဖွဲ့စည်းပုံ

```text
src/
  CleanArchitecture.Domain/          # Domain entities, business rules, audit နှင့် soft-delete
  CleanArchitecture.Application/     # IApplicationDbContext, DTOs, services, CQRS, pagination
  CleanArchitecture.Infrastructure/  # EF Core DbContext, configurations, migrations, interceptors
  CleanArchitecture.Api/             # Controllers, middleware, API response, OpenAPI နှင့် Scalar
```

Dependency ဦးတည်ချက်က `Application -> Domain` နှင့် `Infrastructure -> Application + Domain` ဖြစ်ပါတယ်။ `Api` က application ကို infrastructure နဲ့ ချိတ်ဆက်ပေးတဲ့ composition root ဖြစ်ပါတယ်။ Repository Pattern နဲ့ Generic Repository မသုံးဘဲ EF Core ကို `IApplicationDbContext` ကတစ်ဆင့် အသုံးပြုထားပါတယ်။

## Template အသုံးပြုနည်း

ဒီ repository ရဲ့ root folder မှာ အောက်ပါ command တွေကို run လုပ်ပါ။

```powershell
dotnet new install .
dotnet new ca-hybrid --name MyFinanceApp --output ..\MyFinanceApp
```

Project အသစ်ကိုဖန်တီးပြီးနောက် `MyFinanceApp` folder ထဲဝင်ပြီး run လုပ်ပါ။

## လက်ရှိပါဝင်တဲ့ ဥပမာများ

- **Banks** — ရိုးရှင်းတဲ့ master data CRUD ကို Application Service နဲ့ ကိုင်တွယ်ထားပါတယ်။
- **Transactions** — business operation တွေကို MediatR CQRS command/query နဲ့ ခွဲထားပါတယ်။
- **Pagination** — list endpoint တွေမှာ `page` (မူလ `1`), `take` (မူလ `20`, အများဆုံး `100`) နဲ့ optional `search` ကို သုံးနိုင်ပါတယ်။ `Skip`/`Take` ကို shared pagination extension တစ်နေရာတည်းမှာ စီမံထားပါတယ်။
- **API response** — endpoint response တွေရဲ့ ပုံစံက `{ "success", "code", "message", "data" }` ဖြစ်ပါတယ်။
- **EF Core** — read-only query တွေမှာ `AsNoTracking()` နှင့် DTO projection ကို သုံးထားပါတယ်။ Audit timestamps ကို `SaveChangesInterceptor` က သတ်မှတ်ပြီး soft delete ကို global query filter နဲ့ စစ်ထုတ်ပါတယ်။

## Run လုပ်ရန်

Solution root မှာ run ပါ။

```powershell
dotnet restore
dotnet run --project .\src\CleanArchitecture.Api
```

Development မှာ SQLite database ကို `cleanarchitecture.db` အမည်နဲ့ solution root မှာ ဖန်တီးပြီး schema ကို ပထမဆုံး run ချိန်မှာ တည်ဆောက်ပေးပါတယ်။ API documentation ကို `/scalar/v1` မှာကြည့်နိုင်ပြီး OpenAPI document က `/openapi/v1.json` မှာ ရှိပါတယ်။

## Frontend ချိတ်ဆက်နိုင်သည့် နည်းလမ်းများ

ဒီ project မှာ REST API ပါဝင်ပြီး frontend ကို သီးခြား project အဖြစ် ချိတ်ဆက်နိုင်ပါတယ်။ JSON နဲ့ HTTP request ပို့နိုင်တဲ့ frontend မည်သည့်နည်းပညာမဆို အသုံးပြုနိုင်ပါတယ်။ ဥပမာများကတော့ **Angular**, **React** (သို့) **Next.js**, **Vue**, **Svelte**, **Blazor WebAssembly** တို့ဖြစ်ပါတယ်။ Mobile app လိုအပ်ရင် **Flutter** သို့မဟုတ် **React Native** ကနေလည်း API ကို ခေါ်နိုင်ပါတယ်။ ဒီ repository မှာ အဲဒီ frontend တွေကို ထည့်မပေးထားပါဘူး။

API ကို `http://localhost:5115` မှာ run ထားချိန် frontend က `fetch`, `Axios`, Angular `HttpClient` စတဲ့ HTTP client နဲ့ endpoint တွေကို ခေါ်နိုင်ပါတယ်။ ဥပမာ —

```javascript
const response = await fetch("http://localhost:5115/api/banks?page=1&take=20");
const result = await response.json();

if (result.success) {
  console.log(result.data.items);
}
```

Frontend နဲ့ API ကို မတူတဲ့ origin/port မှာ run မယ်ဆိုရင် browser ရဲ့ CORS ကန့်သတ်ချက်ကြောင့် API ဘက်မှာ frontend origin ကို ခွင့်ပြုထားဖို့ လိုပါတယ်။ လက်ရှိ template မှာ CORS policy မသတ်မှတ်ရသေးတဲ့အတွက် frontend ရဲ့ development/production URL တွေနဲ့ကိုက်ညီအောင် `Program.cs` မှာ CORS ကို configure လုပ်ပါ။ API endpoint နဲ့ response schema တွေကို Scalar (`/scalar/v1`) သို့မဟုတ် OpenAPI (`/openapi/v1.json`) ကနေ ကြည့်ပြီး frontend ချိတ်ဆက်နိုင်ပါတယ်။

## Build လုပ်ရန်

```powershell
dotnet build .\CleanArchitecture.sln
```

## API ဥပမာ

Bank အသစ်ဖန်တီးရန် `POST /api/banks` ကို အောက်ပါ JSON နဲ့ ခေါ်ပါ။

```json
{
  "code": "BANK01",
  "name": "Example Bank"
}
```

Bank စာရင်းယူရန် `GET /api/banks?page=1&take=20&search=example` ကို ခေါ်ပါ။ Transaction စာရင်းအတွက် `GET /api/transactions?page=1&take=20` ကို သုံးနိုင်ပါတယ်။

## Feature အသစ်ထည့်ရန်

- ရိုးရှင်းတဲ့ master data CRUD အတွက် `Application/Services/<Feature>` အောက်မှာ DTO, service interface နဲ့ implementation ထည့်ပါ။ EF Core query ကို `IApplicationDbContext` ကတစ်ဆင့် ခေါ်ပြီး controller ကို ပါးလွှာအောင်ထားပါ။
- Workflow, state transition, သို့မဟုတ် report လို business logic ရှုပ်ထွေးတဲ့အခါ `Application/Features/<Feature>` အောက်မှာ command/query, handler နဲ့ validator ထည့်ပါ။ CQRS controller က `ISender.Send()` ကိုသာ ခေါ်ပါစေ။
- Domain စည်းမျဉ်းတွေကို Domain layer ထဲမှာထားပြီး EF Core configuration နဲ့ persistence logic ကို Infrastructure layer ထဲမှာထားပါ။ EF entity တွေကို API response အဖြစ် တိုက်ရိုက်မပြန်ပါနဲ့။

## Database migration

Starter project က development အတွက် အလွယ်တကူ စတင်နိုင်ရန် `EnsureCreated` ကို သုံးထားပါတယ်။ Production အတွက် migration သုံးမယ်ဆိုရင် startup ရှိ `EnsureCreated` ကို migration initialization နဲ့ အစားထိုးပြီး အောက်ပါ command တွေ run ပါ။ `EnsureCreated` database ကို migrations နဲ့ တိုက်ရိုက်ပေါင်းသုံးလို့မရပါဘူး။

```powershell
dotnet tool install --global dotnet-ef --version 10.0.12
dotnet ef migrations add InitialCreate --project .\src\CleanArchitecture.Infrastructure --startup-project .\src\CleanArchitecture.Api --output-dir Persistence/Migrations
dotnet ef database update --project .\src\CleanArchitecture.Infrastructure --startup-project .\src\CleanArchitecture.Api
```
