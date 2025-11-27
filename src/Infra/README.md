# Pulumi Infra – Template C#

Ce dépôt propose une structure complète pour démarrer un projet **Pulumi en C#**, incluant :

- Trois environnements : **dev**, **staging**, **prod**
- Une architecture modulaire (Storage, FunctionApp, AppService, Monitoring…)
- Des stacks propres par environnement
- Des utils de nommage et tags
- Une pipeline GitHub Actions prête à l’emploi
- Un projet de tests xUnit pour valider l’infrastructure

---

## 🚀 Structure du projet

```
pulumi-infra/
│
├── Pulumi.yaml
├── Pulumi.dev.yaml
├── Pulumi.staging.yaml
├── Pulumi.prod.yaml
│
├── src/
│   ├── Infra/
│   │   ├── Infra.csproj
│   │   ├── Program.cs
│   │   ├── Stacks/
│   │   │   ├── DevStack.cs
│   │   │   ├── ProdStack.cs
│   │   │   └── StagingStack.cs
│   │   ├── Modules/
│   │   │   ├── Storage/
│   │   │   │   ├── StorageModule.cs
│   │   │   ├── FunctionApp/
│   │   │   │   ├── FunctionModule.cs
│   │   │   ├── AppService/
│   │   │   │   ├── AppServiceModule.cs
│   │   │   └── Monitoring/
│   │   │       ├── MonitoringModule.cs
│   │   └── Utils/
│   │       ├── TagBuilder.cs
│   │       └── NameFactory.cs
│   │
│   └── Tests/
│       ├── Infra.Tests.csproj
│       └── FunctionStackTests.cs
│
└── .github/
    └── workflows/
        └── deploy.yaml
```

---

## 🛠 Installation

### Prérequis
- .NET 8
- Pulumi CLI (`curl -fsSL https://get.pulumi.com | sh`)
- Azure CLI (`az login`)
- Un service principal pour la CI/CD GitHub (`--sdk-auth`)

---

## ⚙️ Lancer un déploiement local

### Initialiser Pulumi
```bash
pulumi login
pulumi stack init dev
```

### Configurer l’environnement
```bash
pulumi config set pulumi-infra:environment dev
pulumi config set pulumi-infra:location westeurope
pulumi config set pulumi-infra:baseName xra-dev
```

### Déployer
```bash
pulumi up
```

---

## 🔁 CI/CD GitHub Actions

Une pipeline est incluse dans :

```
.github/workflows/deploy.yaml
```

Elle :
- installe .NET
- installe Pulumi
- se connecte à Azure
- sélectionne la stack
- exécute le `preview` puis le `up`

### Secrets nécessaires

Dans GitHub → Settings → Secrets :

| Secret | Description |
|--------|-------------|
| `AZURE_CREDENTIALS` | JSON du service principal Azure |
| `PULUMI_STATE_CONTAINER` | Nom du container Blob pour stocker l’état |
| `PULUMI_STACK` | ex : `dev` |

---

## 🧪 Tests Infrastructure

Les tests se trouvent dans :

```
src/Tests/
```

Exemple de test :

```csharp
[Fact]
public async Task DevStack_Should_Expose_FunctionEndpoint()
{
    var resources = await Testing.RunAsync<DevStack>();
    Assert.NotNull(resources);
}
```

---

## 🧩 Modules inclus

| Module | Description |
|--------|-------------|
| StorageModule | Provisionne un Storage Account |
| FunctionModule | Function App + Plan Y1 + AppSettings |
| AppServiceModule | App Service basique (B1) |
| MonitoringModule | Application Insights |
| TagBuilder | Construit un set de tags standard |
| NameFactory | Génère les noms de ressources |

---

## 🎯 Objectif du template

Ce template sert de **starter solide** pour construire une plateforme Azure modulaire, testable, versionnée et industrialisée.  
Il est pensé pour être propre, pédagogique et prêt à enrichir avec :

- APIM  
- Service Bus  
- Cosmos DB  
- Key Vault  
- Event Grid