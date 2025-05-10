# FCG

# EF Core
## Migração Inicial
- Para executar a primeira migração, criando a tabela de usuário, é necessário executar o comando

Update-Database FirstMigration -StartupProject FCG.Infrastructure -Connection "Server={ServerName};Database={DBName};Trusted_Connection=True;TrustServerCertificate=True"

