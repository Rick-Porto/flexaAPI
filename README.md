# flexaAPI

[Português (Brasil)](#português-br) | [English (US)](#english-us)

---

## Português (Brasil)

### Visão geral

A **flexaAPI** é uma API REST ASP.NET Core para gerenciar aplicativos (`AppEntity`), telas (`Screen`), componentes de tela (`Component`) e dados associados (`DataEntry`). O projeto utiliza .NET 9, autenticação JWT do Supabase e PostgreSQL por meio do Entity Framework Core.

A solução contém a API e suas camadas de aplicação, domínio, infraestrutura e utilitários. Não há um projeto de frontend incluído nesta solução.

### Arquitetura

| Projeto | Responsabilidade |
| --- | --- |
| `flexa.Api` | Endpoints HTTP, autenticação, CORS, Swagger e middleware de erros. |
| `flexa.App` | Serviços da aplicação e DTOs de entrada/saída. |
| `flexa.Domain` | Entidades e interfaces dos repositórios. |
| `flexa.Infra` | `AppDbContext`, configurações do EF Core, repositórios e migrations. |
| `flexa.Utils` | Conversor JSON de `DateTime` para o fuso `America/Sao_Paulo`. |

### Modelo de domínio

- **AppEntity**: aplicativo pertencente a um usuário, com várias telas e um link público opcional.
- **Screen**: pertence a um aplicativo e pode conter componentes e entradas de dados.
- **Component**: pertence a uma tela e guarda tipo, rótulo, modelo, configuração, ordem e indicador de obrigatório.
- **DataEntry**: guarda `Data` e `Config` como strings JSON e referencia uma tela e um componente.

Aplicativos usam IDs `Guid`; telas, componentes e entradas de dados usam IDs inteiros. Relacionamentos e exclusão em cascata são configurados em `flexa.Infra/Configurations`. O PostgreSQL armazena `Config` e `Data` como `jsonb`.

### API

As rotas foram verificadas diretamente nos controllers. Os endpoints protegidos exigem `Authorization: Bearer <token>` com um JWT válido do Supabase. O identificador do usuário vem da claim `sub`; não envie `userId` no corpo ou na query string.

| Recurso | Método e rota | Descrição |
| --- | --- | --- |
| Telas | `GET /api/screens` | Lista telas do usuário autenticado. |
| | `GET /api/screens/{id}` | Obtém uma tela com componentes e dados associados. |
| | `POST /api/screens` | Cria uma tela. Campos: `name`, `description`, `appEntityId`. |
| | `PUT /api/screens/{id}` | Atualiza uma tela. |
| | `DELETE /api/screens/{id}` | Exclui uma tela. |
| Componentes | `GET /api/Component` | Lista os componentes do usuário. |
| | `GET /api/Component/{id}` | Obtém um componente. |
| | `GET /api/Component/by-screen/{screenId}` | Lista componentes de uma tela. |
| | `POST /api/Component` | Cria um componente. Campos: `elementType`, `label`, `model`, `config`, `order`, `required`, `screenId`. |
| | `PUT /api/Component/{id}` | Atualiza um componente. |
| | `DELETE /api/Component/{id}` | Exclui um componente. |
| Entradas de dados | `GET /api/DataEntry` | Lista as entradas do usuário. |
| | `GET /api/DataEntry/{id}` | Obtém uma entrada. |
| | `GET /api/DataEntry/by-screen/{screenId}` | Lista entradas de uma tela. |
| | `GET /api/DataEntry/by-component/{componentId}` | Lista entradas de um componente. |
| | `POST /api/DataEntry` | Cria uma entrada. Campos: `config`, `data`, `screenId`, `componentId`. |
| | `PUT /api/DataEntry/{id}` | Atualiza uma entrada. |
| | `DELETE /api/DataEntry/{id}` | Exclui uma entrada. |
| Aplicativos | `GET /api/AppEntity` | Lista os aplicativos do usuário. |
| | `GET /api/AppEntity/{id}` | Obtém um aplicativo (`id` é um `Guid`). |
| | `POST /api/AppEntity` | Cria um aplicativo. Campos: `name` e `description`. |
| | `PUT /api/AppEntity/{id}` | Atualiza um aplicativo. |
| | `DELETE /api/AppEntity/{id}` | Exclui um aplicativo. |
| | `POST /api/AppEntity/{id}/publish` | Publica o aplicativo e retorna o link público. |
| | `POST /api/AppEntity/{id}/unpublish` | Remove o link público. |
| | `GET /api/AppEntity/public/{publicLink}` | Consulta um aplicativo publicado; não exige autenticação. |

Os três controllers de recursos usam a rota `[controller]`; a rota de telas é definida explicitamente. O ASP.NET Core normalmente trata rotas sem distinção entre maiúsculas e minúsculas. O endpoint público retorna os dados de `AppEntity`; não presuma que ele também retorna telas e componentes.

#### DTOs principais

- `ScreenRequest`: `name` e `description` opcionais; `appEntityId` (`Guid`). A resposta inclui IDs, datas e coleções de componentes e dados.
- `ComponentRequest`: `elementType` e `label` obrigatórios; `model` e `config` opcionais; também `order`, `required` e `screenId`.
- `DataEntryRequest`: `config` opcional, `data` (string JSON, padrão `{}`), `screenId` e `componentId`.
- `AppEntityRequest`: `name`, `description` e `publicLink` opcionais. O link público é gerenciado pelos endpoints `publish` e `unpublish`.

Consulte `flexa.App/DTOs` para os contratos completos, campos de resposta e nulabilidade.

### Pré-requisitos

- .NET SDK 9.0.
- Instância PostgreSQL acessível pela connection string configurada.
- Projeto Supabase configurado para emitir JWTs assinados com um segredo compatível com o validador da API.
- Ferramenta `dotnet-ef` 9.x para aplicar migrations pela CLI.

### Configuração

A API carrega `appsettings.json`, `appsettings.{Environment}.json` e variáveis de ambiente. Mantenha segredos fora do controle de versão, usando variáveis de ambiente ou um gerenciador seguro:

| Variável de ambiente | Chave de configuração | Finalidade |
| --- | --- | --- |
| `Supabase__Url` | `Supabase:Url` | URL do projeto Supabase. |
| `Supabase__JwtSecret` | `Supabase:JwtSecret` | Segredo usado para validar JWTs. |
| `ConnectionStrings__Supabase` | `ConnectionStrings:Supabase` | Connection string do PostgreSQL. |
| `Cors__AllowedOrigins__0` | `Cors:AllowedOrigins:0` | Origem permitida para o frontend; use índices adicionais para outras origens. |

O CORS usa a política `Frontend`. Em produção, permita somente as origens necessárias. A API valida tokens; este repositório não contém endpoints de cadastro ou login do Supabase.

> **Segurança:** `appsettings.json` e `appsettings.Development.json` atualmente contêm valores de credenciais. Não reutilize nem publique esses valores. Remova segredos dos arquivos versionados, invalide e rotacione credenciais já expostas e configure novos valores localmente ou em um cofre de segredos.

### Executar localmente

Na raiz do repositório:

```powershell
dotnet restore flexa.sln
dotnet build flexa.sln
dotnet run --project flexa.Api/flexa.Api.csproj --launch-profile http
```

O perfil `http` usa `http://localhost:5000`. Em ambiente `Development`, a interface Swagger fica em:

```text
http://localhost:5000/FlexaSwagger
```

Configure os valores de banco e autenticação antes de iniciar. O banco precisa estar acessível e atualizado com as migrations.

### Migrations do banco

Com `dotnet-ef` 9.x instalado e a configuração do banco disponível, aplique as migrations:

```powershell
dotnet ef database update --project flexa.Infra/flexa.Infra.csproj --startup-project flexa.Api/flexa.Api.csproj
```

Para criar uma migration:

```powershell
dotnet ef migrations add NomeDaMigration --project flexa.Infra/flexa.Infra.csproj --startup-project flexa.Api/flexa.Api.csproj --output-dir Migrations
```

As migrations existentes incluem a criação inicial do esquema e a adição de links públicos para aplicativos.

### Autenticação, erros e datas

- Envie `Authorization: Bearer <token>` para endpoints protegidos.
- O acesso aos dados é isolado pela claim `sub` validada no JWT; a API verifica a propriedade das entidades.
- `GET /api/AppEntity/public/{publicLink}` é explicitamente anônimo; os demais endpoints dos controllers exigem autorização.
- O middleware global retorna JSON com a propriedade `error`; em desenvolvimento pode incluir também `stackTrace`.
- `DateTime` é tratado pelo `BrazilDateTimeConverter`, que interpreta/converte valores usando `America/Sao_Paulo` e escreve datas ISO 8601.

### Estrutura do repositório

```text
flexa.sln
flexa.Api/                 # API, controllers e configuração
flexa.App/                 # serviços, interfaces e DTOs
flexa.Domain/              # entidades e interfaces de repositório
flexa.Infra/                # EF Core, repositórios, configurações e migrations
flexa.Utils/                # utilitários e conversores
```

---

## English (US)

### Overview

**flexaAPI** is an ASP.NET Core REST API for managing apps (`AppEntity`), screens (`Screen`), screen components (`Component`), and related data (`DataEntry`). It targets .NET 9, uses Supabase JWT authentication, and accesses PostgreSQL through Entity Framework Core.

The solution contains the API and its application, domain, infrastructure, and utility layers. It does not include a frontend project.

### Architecture

| Project | Responsibility |
| --- | --- |
| `flexa.Api` | HTTP endpoints, authentication, CORS, Swagger, and error middleware. |
| `flexa.App` | Application services and request/response DTOs. |
| `flexa.Domain` | Domain entities and repository interfaces. |
| `flexa.Infra` | `AppDbContext`, EF Core configuration, repositories, and migrations. |
| `flexa.Utils` | JSON `DateTime` converter for the `America/Sao_Paulo` time zone. |

### Domain model

- **AppEntity**: belongs to a user, can contain multiple screens, and can have an optional public link.
- **Screen**: belongs to an app and can contain components and data entries.
- **Component**: belongs to a screen and stores its type, label, model, configuration, order, and required flag.
- **DataEntry**: stores `Data` and `Config` as JSON strings and references a screen and a component.

Apps use `Guid` IDs; screens, components, and data entries use integer IDs. Relationships and cascade delete behavior are configured under `flexa.Infra/Configurations`. PostgreSQL stores `Config` and `Data` as `jsonb`.

### API

Routes below are verified against the controllers. Protected endpoints require `Authorization: Bearer <token>` with a valid Supabase JWT. The user ID is read from the `sub` claim; do not send `userId` in the request body or query string.

| Resource | Method and route | Description |
| --- | --- | --- |
| Screens | `GET /api/screens` | Lists screens for the authenticated user. |
| | `GET /api/screens/{id}` | Gets a screen, including its components and related data entries. |
| | `POST /api/screens` | Creates a screen. Fields: `name`, `description`, `appEntityId`. |
| | `PUT /api/screens/{id}` | Updates a screen. |
| | `DELETE /api/screens/{id}` | Deletes a screen. |
| Components | `GET /api/Component` | Lists the user's components. |
| | `GET /api/Component/{id}` | Gets a component. |
| | `GET /api/Component/by-screen/{screenId}` | Lists components for a screen. |
| | `POST /api/Component` | Creates a component. Fields: `elementType`, `label`, `model`, `config`, `order`, `required`, `screenId`. |
| | `PUT /api/Component/{id}` | Updates a component. |
| | `DELETE /api/Component/{id}` | Deletes a component. |
| Data entries | `GET /api/DataEntry` | Lists the user's data entries. |
| | `GET /api/DataEntry/{id}` | Gets a data entry. |
| | `GET /api/DataEntry/by-screen/{screenId}` | Lists entries for a screen. |
| | `GET /api/DataEntry/by-component/{componentId}` | Lists entries for a component. |
| | `POST /api/DataEntry` | Creates a data entry. Fields: `config`, `data`, `screenId`, `componentId`. |
| | `PUT /api/DataEntry/{id}` | Updates a data entry. |
| | `DELETE /api/DataEntry/{id}` | Deletes a data entry. |
| Apps | `GET /api/AppEntity` | Lists the user's apps. |
| | `GET /api/AppEntity/{id}` | Gets an app (`id` is a `Guid`). |
| | `POST /api/AppEntity` | Creates an app. Fields: `name` and `description`. |
| | `PUT /api/AppEntity/{id}` | Updates an app. |
| | `DELETE /api/AppEntity/{id}` | Deletes an app. |
| | `POST /api/AppEntity/{id}/publish` | Publishes the app and returns its public link. |
| | `POST /api/AppEntity/{id}/unpublish` | Removes the public link. |
| | `GET /api/AppEntity/public/{publicLink}` | Retrieves a published app; authentication is not required. |

The three resource controllers use the `[controller]` route token; the screen route is explicitly declared. ASP.NET Core routes are normally case-insensitive. The public endpoint returns `AppEntity` response data; do not assume it also returns screens and components.

#### Main DTOs

- `ScreenRequest`: optional `name` and `description`, plus `appEntityId` (`Guid`). Its response includes IDs, timestamps, and component/data collections.
- `ComponentRequest`: required `elementType` and `label`; optional `model` and `config`; also `order`, `required`, and `screenId`.
- `DataEntryRequest`: optional `config`, `data` (a JSON string, default `{}`), `screenId`, and `componentId`.
- `AppEntityRequest`: optional `name`, `description`, and `publicLink`. Public-link management is performed through the `publish` and `unpublish` endpoints.

See `flexa.App/DTOs` for complete contracts, response fields, and nullability.

### Prerequisites

- .NET 9.0 SDK.
- A PostgreSQL instance reachable through the configured connection string.
- A Supabase project configured to issue JWTs signed with a secret compatible with the API validator.
- `dotnet-ef` 9.x to apply migrations from the CLI.

### Configuration

The API loads `appsettings.json`, `appsettings.{Environment}.json`, and environment variables. Keep secrets out of source control and provide them through environment variables or a secure secret manager:

| Environment variable | Configuration key | Purpose |
| --- | --- | --- |
| `Supabase__Url` | `Supabase:Url` | Supabase project URL. |
| `Supabase__JwtSecret` | `Supabase:JwtSecret` | Secret used to validate JWTs. |
| `ConnectionStrings__Supabase` | `ConnectionStrings:Supabase` | PostgreSQL connection string. |
| `Cors__AllowedOrigins__0` | `Cors:AllowedOrigins:0` | Allowed frontend origin; configure additional indexes for more origins. |

CORS uses the `Frontend` policy. In production, allow only required origins. The API validates tokens; this repository does not contain Supabase sign-up or login endpoints.

> **Security:** `appsettings.json` and `appsettings.Development.json` currently contain credential values. Do not reuse or publish them. Remove secrets from tracked files, revoke and rotate any credentials that have already been exposed, and configure replacement values locally or in a secrets vault.

### Run locally

From the repository root:

```powershell
dotnet restore flexa.sln
dotnet build flexa.sln
dotnet run --project flexa.Api/flexa.Api.csproj --launch-profile http
```

The `http` launch profile uses `http://localhost:5000`. In the `Development` environment, Swagger UI is available at:

```text
http://localhost:5000/FlexaSwagger
```

Configure database and authentication values before starting the API. The database must be reachable and up to date with the migrations.

### Database migrations

With `dotnet-ef` 9.x installed and database configuration available, apply migrations with:

```powershell
dotnet ef database update --project flexa.Infra/flexa.Infra.csproj --startup-project flexa.Api/flexa.Api.csproj
```

Create a migration with:

```powershell
dotnet ef migrations add MigrationName --project flexa.Infra/flexa.Infra.csproj --startup-project flexa.Api/flexa.Api.csproj --output-dir Migrations
```

Existing migrations include the initial schema and the addition of public links for apps.

### Authentication, errors, and dates

- Send `Authorization: Bearer <token>` for protected endpoints.
- Data access is scoped to the validated JWT `sub` claim; the API checks entity ownership.
- `GET /api/AppEntity/public/{publicLink}` is explicitly anonymous; the other controller endpoints require authorization.
- Global exception middleware returns JSON with an `error` property; in development it may also include `stackTrace`.
- `DateTime` values are handled by `BrazilDateTimeConverter`, which interprets/converts values using `America/Sao_Paulo` and writes ISO 8601 timestamps.

### Repository structure

```text
flexa.sln
flexa.Api/                 # API, controllers, and configuration
flexa.App/                 # services, interfaces, and DTOs
flexa.Domain/              # entities and repository interfaces
flexa.Infra/                # EF Core, repositories, configurations, and migrations
flexa.Utils/                # utilities and converters
```

