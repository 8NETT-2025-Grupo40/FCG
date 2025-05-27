# FCG – FIAP Cloud Games

## Visão Geral  

O FCG (FIAP Cloud Games) é uma plataforma RESTful desenvolvida em .NET 8 para venda e gestão de jogos digitais, além da gestão de servidores de jogos. 

Entre as funcionalidades funcionalidades da API estão:

- **Cadastro e autenticação de usuários** via JWT, com controle de papéis (usuário e administrador);
- **Arquitetura modular e foco em DDD** para facilitar evolução;
- **(em desenvolvimento) Gerenciamento de biblioteca**: registro e consulta dos jogos adquiridos;
- **(em desenvolvimento) Administração**: CRUD de jogos e promoções para administradores;
- **(em desenvolvimento) Gestão de servidores de partidas** como base para futuros módulos. 

Projeto acadêmico solicitado pela instituição de ensino FIAP.

**Curso:** Arquitetura de Sistemas .NET
**Turma:** 8NETT  
**Grupo:** 40

### Integrantes:
- [Luciano Castilho](https://github.com/lcastilho)
- [Anderson Mori](https://github.com/AndersonMori)
- [Ricardo Cavati](https://github.com/RicardoKromerCavati)
- [Gustavo Coelho](https://github.com/GustavoCoelho1705)

## Tecnologias  
- .NET 8
- C#
- Entity Framework Core
- System.IdentityModel.Tokens.Jwt (JWT)
- Serilog
- Swagger
- xUnit + NSubstitute

## Pré-requisitos  
- .NET 8 SDK
- SQL Server

## Instalação e configuração  
No arquivo appsettings.Development.json em FCG.API, substitua:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server={ServerName};Database={DBName};Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### Migration
Para executar a primeira migration, existem 2 opções:
1. (Recomendado para ambiente de desenvolvimento) Executar o sistema com o profile **https with ApplyMigration**
    - dotnet run --project FCG.API --launch-profile "https with ApplyMigration"
2. Executar no Package Manager Console: Update-Database {MigrationName} -StartupProject FCG.API -Connection "Server={ServerName};Database={DBName};Trusted_Connection=True;TrustServerCertificate=True"

### Swagger
Ao subir a aplicação, a aplicação se encontra nas urls:
- `https://localhost:7137/index.html`
- `http://localhost:5264/index.html`

### Uso inicial
Por se tratar de um projeto acadêmico, disponibilizamos as credenciais do usuário administrador inserido via migrations, para facilitar o uso inicial do sistema nas operações que exigem o nível de acesso de Administrador. Ele tem as seguintes credenciais:

```
{
  "email": "adm@fcg.com",
  "password": "Adm1234@"
}
```

Basta utilizá-la no `POST` /authentication/login
