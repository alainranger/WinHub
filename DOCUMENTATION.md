# Documentation du Projet WinHub

## Table des matières

1. [Vue d'ensemble](#vue-densemble)
2. [Architecture de l'application](#architecture-de-lapplication)
3. [Modèle de données](#modèle-de-données)
4. [Architecture technique](#architecture-technique)
5. [Technologies utilisées](#technologies-utilisées)
6. [Configuration et déploiement](#configuration-et-déploiement)

## Vue d'ensemble

**WinHub** est une application de gestion de tirages au sort développée avec .NET Aspire. Elle permet de créer et gérer des concours avec des participants qui peuvent avoir plusieurs participations.

### Objectifs du projet

- Expérimenter la plateforme **.NET Aspire**
- Démontrer l'interopérabilité entre différentes technologies frontend
- Implémenter une architecture moderne et observalble
- Supporter plusieurs bases de données sans modification de code

### Fonctionnalités principales

- ✅ Création et gestion de concours (tirages au sort)
- ✅ Gestion des participants
- ✅ Gestion des participations
- ✅ Interface Blazor Server
- ✅ API REST avec Swagger
- ✅ Support multi-frontend (React, Angular, Vue.js, Svelte)

## Architecture de l'application

### Diagramme d'architecture globale

```mermaid
graph TB
    subgraph "Clients"
        B[Blazor Server]
        R[React SPA]
        A[Angular SPA]
        V[Vue.js SPA]
        S[Svelte SPA]
    end
    
    subgraph "Aspire AppHost"
        AH[WinHub.AppHost<br/>Orchestrateur]
    end
    
    subgraph "Backend Services"
        API[WinHub.ApiService<br/>API REST]
        SD[WinHub.ServiceDefaults<br/>Services partagés]
    end
    
    subgraph "Infrastructure"
        DB[(PostgreSQL<br/>Base de données)]
        PGA[pgAdmin<br/>Administration DB]
    end
    
    B --> API
    R --> API
    A --> API
    V --> API
    S --> API
    
    AH --> B
    AH --> R
    AH --> A
    AH --> V
    AH --> S
    AH --> API
    AH --> DB
    AH --> PGA
    
    API --> DB
    API --> SD
    
    style AH fill:#e1f5fe
    style API fill:#f3e5f5
    style DB fill:#e8f5e8
```

### Architecture en couches du backend

```mermaid
graph TB
    subgraph "Présentation Layer"
        EP[Endpoints<br/>Carter]
        SW[Swagger/OpenAPI]
    end
    
    subgraph "Application Layer"
        MR[MediatR<br/>Commands/Queries]
        VL[FluentValidation<br/>Validators]
        HD[Handlers<br/>Business Logic]
    end
    
    subgraph "Domain Layer"
        EN[Entities<br/>Contest, Participant, Participation]
        BE[BaseEntity<br/>Auditing]
    end
    
    subgraph "Infrastructure Layer"
        EF[Entity Framework Core]
        DB[(PostgreSQL)]
    end
    
    EP --> MR
    SW --> EP
    MR --> HD
    HD --> VL
    HD --> EN
    EN --> BE
    HD --> EF
    EF --> DB
    
    style EP fill:#ffecb3
    style MR fill:#e1f5fe
    style EN fill:#f3e5f5
    style EF fill:#e8f5e8
```

## Modèle de données

### Diagramme de classes des entités

```mermaid
classDiagram
    class BaseEntity~TKey~ {
        +TKey Id
        +DateTime CreatedAt
        +string CreatedBy
        +DateTime UpdatedAt
        +string UpdatedBy
    }
    
    class Contest {
        +Guid Id
        +string Name
        +string Description
        +DateTime StartDateTime
        +DateTime EndDateTime
        +DateTime ContestDateTime
        +ICollection~Participation~ Participations
    }
    
    class Participant {
        +Guid Id
        +string Firstname
        +string Lastname
        +string Email
        +ICollection~Participation~ Participations
    }
    
    class Participation {
        +Guid Id
        +Guid ContestId
        +Guid ParticipantId
        +Contest Contest
        +Participant Participant
    }
    
    BaseEntity~Guid~ <|-- Contest
    BaseEntity~Guid~ <|-- Participant
    BaseEntity~Guid~ <|-- Participation
    
    Contest ||--o{ Participation : "1 to many"
    Participant ||--o{ Participation : "1 to many"
    Participation }o--|| Contest : "many to 1"
    Participation }o--|| Participant : "many to 1"
```

### Diagramme entité-relation

```mermaid
erDiagram
    CONTEST {
        uuid id PK
        varchar name
        text description
        timestamp start_date_time
        timestamp end_date_time
        timestamp contest_date_time
        timestamp created_at
        varchar created_by
        timestamp updated_at
        varchar updated_by
    }
    
    PARTICIPANT {
        uuid id PK
        varchar firstname
        varchar lastname
        varchar email
        timestamp created_at
        varchar created_by
        timestamp updated_at
        varchar updated_by
    }
    
    PARTICIPATION {
        uuid id PK
        uuid contest_id FK
        uuid participant_id FK
        timestamp created_at
        varchar created_by
        timestamp updated_at
        varchar updated_by
    }
    
    CONTEST ||--o{ PARTICIPATION : "has many"
    PARTICIPANT ||--o{ PARTICIPATION : "has many"
    PARTICIPATION }o--|| CONTEST : "belongs to"
    PARTICIPATION }o--|| PARTICIPANT : "belongs to"
```

## Architecture technique

### Diagramme de déploiement Aspire

```mermaid
graph TB
    subgraph "Development Environment"
        subgraph "Aspire Dashboard"
            AD[Aspire Dashboard<br/>https://localhost:15888]
        end
        
        subgraph "Applications"
            API[backend-apiservice<br/>https://localhost:7148]
            BLZ[frontend-blazor<br/>https://localhost:7149]
            RCT[frontend-react<br/>http://localhost:3000]
            ANG[frontend-angular<br/>http://localhost:4200]
            VUE[frontend-vue<br/>http://localhost:8080]
            SVT[frontend-svelte<br/>http://localhost:5173]
        end
        
        subgraph "Infrastructure"
            PG[(PostgreSQL<br/>localhost:5432)]
            PGA[pgAdmin<br/>http://localhost:8080]
        end
    end
    
    AD -.-> API
    AD -.-> BLZ
    AD -.-> RCT
    AD -.-> ANG
    AD -.-> VUE
    AD -.-> SVT
    AD -.-> PG
    AD -.-> PGA
    
    BLZ --> API
    RCT --> API
    ANG --> API
    VUE --> API
    SVT --> API
    
    API --> PG
    PGA --> PG
    
    style AD fill:#ffecb3
    style API fill:#f3e5f5
    style PG fill:#e8f5e8
```

### Architecture des fonctionnalités (Vertical Slice)

```mermaid
graph TB
    subgraph "Contest Features"
        subgraph "GetAllContest"
            CAE1[EndPoint]
            CAH1[Handler]
            CAQ1[Query]
        end
        
        subgraph "CreateContest"
            CCE[EndPoint]
            CCH[Handler]
            CCC[Command]
            CCV[Validator]
        end
        
        subgraph "UpdateContest"
            CUE[EndPoint]
            CUH[Handler]
            CUC[Command]
            CUV[Validator]
        end
    end
    
    subgraph "Participant Features"
        subgraph "GetAllParticipant"
            PAE1[EndPoint]
            PAH1[Handler]
            PAQ1[Query]
        end
        
        subgraph "CreateParticipant"
            PCE[EndPoint]
            PCH[Handler]
            PCC[Command]
            PCV[Validator]
        end
    end
    
    subgraph "Participation Features"
        subgraph "GetAllParticipation"
            PTAE1[EndPoint]
            PTAH1[Handler]
            PTAQ1[Query]
        end
        
        subgraph "CreateParticipation"
            PTCE[EndPoint]
            PTCH[Handler]
            PTCC[Command]
            PTCV[Validator]
        end
    end
    
    CAE1 --> CAH1
    CAH1 --> CAQ1
    CCE --> CCH
    CCH --> CCC
    CCH --> CCV
    CUE --> CUH
    CUH --> CUC
    CUH --> CUV
    
    PAE1 --> PAH1
    PAH1 --> PAQ1
    PCE --> PCH
    PCH --> PCC
    PCH --> PCV
    
    PTAE1 --> PTAH1
    PTAH1 --> PTAQ1
    PTCE --> PTCH
    PTCH --> PTCC
    PTCH --> PTCV
    
    style CAE1 fill:#ffecb3
    style CCE fill:#ffecb3
    style CUE fill:#ffecb3
    style PAE1 fill:#ffecb3
    style PCE fill:#ffecb3
    style PTAE1 fill:#ffecb3
    style PTCE fill:#ffecb3
```

### Diagramme de flux des requêtes

```mermaid
sequenceDiagram
    participant C as Client (Frontend)
    participant E as Endpoint (Carter)
    participant M as MediatR
    participant H as Handler
    participant V as Validator
    participant DB as Database
    
    C->>+E: HTTP Request
    E->>+M: Send Command/Query
    M->>+H: Route to Handler
    
    alt Command (Create/Update/Delete)
        H->>+V: Validate Input
        V-->>-H: Validation Result
        
        alt Validation Success
            H->>+DB: Execute Operation
            DB-->>-H: Result
            H-->>-M: Success Response
        else Validation Failed
            H-->>-M: Validation Error
        end
    else Query (Read)
        H->>+DB: Execute Query
        DB-->>-H: Data
        H-->>-M: Query Result
    end
    
    M-->>-E: Response
    E-->>-C: HTTP Response (JSON)
```

## Technologies utilisées

### Backend Technologies

```mermaid
mindmap
  root((Backend))
    Framework
      .NET 9
      ASP.NET Core
      .NET Aspire
    Architecture
      Vertical Slice Architecture
      CQRS with MediatR
      Carter for Minimal APIs
    Data Access
      Entity Framework Core
      PostgreSQL
      Database Migrations
    Validation & Documentation
      FluentValidation
      Swagger/OpenAPI
    Monitoring
      Aspire Dashboard
      Health Checks
      Logging & Telemetry
```

### Frontend Technologies

```mermaid
mindmap
  root((Frontend))
    .NET
      Blazor Server
      Razor Components
      Interactive Server Mode
    JavaScript/TypeScript
      React with Vite
      Angular CLI
      Vue.js 3 Composition API
      Svelte with SvelteKit
    Build Tools
      npm/yarn
      Vite
      Webpack
      esbuild
```

## Configuration et déploiement

### Variables d'environnement et configuration

```mermaid
graph LR
    subgraph "Configuration Sources"
        AS[appsettings.json]
        AD[appsettings.Development.json]
        ENV[Environment Variables]
        AU[User Secrets]
    end
    
    subgraph "Aspire Configuration"
        AC[Aspire Configuration]
        SR[Service References]
        EP[Endpoint Configuration]
    end
    
    subgraph "Application"
        APP[WinHub Application]
    end
    
    AS --> APP
    AD --> APP
    ENV --> APP
    AU --> APP
    AC --> APP
    SR --> APP
    EP --> APP
```

### Étapes de déploiement

```mermaid
flowchart TD
    A[Clone Repository] --> B[Install .NET 9 SDK]
    B --> C[Install Docker Desktop]
    C --> D[Restore NuGet Packages]
    D --> E[Restore npm Packages]
    E --> F[Run Aspire AppHost]
    F --> G[Access Aspire Dashboard]
    G --> H[Verify Services Health]
    
    H --> I{All Services Running?}
    I -->|Yes| J[Access Applications]
    I -->|No| K[Check Logs in Dashboard]
    K --> L[Fix Configuration]
    L --> F
    
    J --> M[Test Functionality]
    M --> N[Development Ready]
    
    style A fill:#e1f5fe
    style F fill:#f3e5f5
    style J fill:#e8f5e8
    style N fill:#c8e6c9
```

### Commandes de développement

```bash
# Démarrer l'application complète
dotnet run --project src/WinHub.AppHost

# Démarrer uniquement l'API
dotnet run --project src/backend/WinHub.ApiService

# Démarrer uniquement Blazor
dotnet run --project src/frontend/WinHub.Blazor

# Démarrer React
cd src/frontend/WinHub.React
npm start

# Démarrer Angular
cd src/frontend/WinHub.Angular
ng serve

# Démarrer Vue.js
cd src/frontend/WinHub.VueJS
npm run dev

# Démarrer Svelte
cd src/frontend/WinHub.Svelte
npm run dev
```

## Points clés de l'architecture

### Avantages de l'approche choisie

1. **Vertical Slice Architecture** : Chaque fonctionnalité est autonome et facile à maintenir
2. **CQRS avec MediatR** : Séparation claire entre les commandes et les requêtes
3. **Multi-frontend** : Démonstration de l'interopérabilité avec différentes technologies
4. **Aspire** : Orchestration simplifiée et observabilité intégrée
5. **Entity Framework Core** : Abstraction de la base de données permettant le changement facile de SGBD

### Extensibilité

Le projet est conçu pour être facilement extensible :

- Ajout de nouveaux frontends
- Support de nouvelles bases de données
- Ajout de fonctionnalités métier
- Intégration de services externes
- Déploiement sur différents environnements (Azure, AWS, etc.)

---

*Cette documentation est maintenue automatiquement et reflète l'état actuel du projet WinHub.*
 
