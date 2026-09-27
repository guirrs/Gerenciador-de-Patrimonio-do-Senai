# Gerenciador de Patrimônio do Senai

Sistema para gestão de patrimônios, localizações, transferências e usuários. A parte de backend presente neste repositório é uma API REST em ASP.NET Core; o diretório do frontend está registrado como um submódulo Git, mas não contém o código-fonte neste clone.

> **Estado atual do repositório:** a API .NET 8 pode ser configurada para execução local. O frontend Node não pode ser iniciado a partir deste clone: `UI/patrimonio` está registrado como gitlink, mas o repositório não contém o arquivo `.gitmodules` necessário para identificar o submódulo, e não há `package.json` ou lockfile. Também não foram encontrados scripts SQL nem migrações do Entity Framework para criar o banco. Os itens ausentes estão detalhados abaixo para evitar comandos que não funcionariam.

## Tecnologias identificadas

- **Backend:** ASP.NET Core / .NET 8

- **Persistência:** Entity Framework Core 8 e Microsoft SQL Server

- **Autenticação:** JWT

- **Documentação da API:** Swagger, habilitado no ambiente `Development`

- **Frontend esperado:** diretório `UI/patrimonio` referenciado como submódulo Git; o código e as tecnologias/versões do frontend não estão disponíveis neste clone para confirmar os comandos npm

## Pré-requisitos para executar a API

- Git

- **.NET SDK 8**

- Microsoft SQL Server acessível pela máquina

- SQL Server LocalDB no Windows, se for usar a connection string padrão do arquivo `.env.example`

## 1. Clonar o repositório

```bash
git clone https://github.com/guirrs/Gerenciador-de-Patrimonio-do-Senai.git
cd Gerenciador-de-Patrimonio-do-Senai
```

## 2. Preparar o banco de dados

A API usa Entity Framework Core e obtém a conexão da variável `CONNECTION_STRING`. O arquivo `API/GestaoPatrimonios/GestaoPatrimonios/.env.example` traz uma connection string de exemplo para um banco chamado `GestaoPatrimonios` em SQL Server LocalDB.

No estado atual, o repositório **não inclui** arquivo `.sql`, migrações do EF Core ou uma chamada automática a `Database.Migrate( )`/`EnsureCreated()`. Portanto, antes de testar operações da API, o SQL Server precisa estar ativo e o banco precisa ter um schema compatível com as entidades da aplicação. A criação desse schema não está automatizada nem documentada neste repositório.

Se utilizar outro servidor ou sistema operacional, edite `CONNECTION_STRING` com os dados da sua instância SQL Server. Não publique credenciais no Git.

## 3. Configurar o ambiente local da API

Vá para a pasta da API e copie o modelo de variáveis de ambiente:

```bash
cd API/GestaoPatrimonios/GestaoPatrimonios
cp .env.example .env
```

Edite o arquivo `.env` e configure:

```
CONNECTION_STRING=Server=(localdb)\MSSQLLocalDB;Database=GestaoPatrimonios;Trusted_Connection=True;TrustServerCertificate=True
JWT_KEY=COLOQUE_AQUI_UMA_CHAVE_LOCAL_FORTE_COM_PELO_MENOS_32_CARACTERES
```

- Ajuste `CONNECTION_STRING` para seu SQL Server e banco.

- `JWT_KEY` precisa existir e ter pelo menos 32 caracteres. Use uma chave aleatória local, não o texto de exemplo.

- O `.env` é ignorado pelo Git neste projeto. Mantenha nele apenas configurações locais e não versione segredos.

- Os valores de `Jwt:Issuer` e `Jwt:Audience` são lidos de `appsettings.json`; não precisam ser duplicados no `.env`.

## 4. Restaurar dependências e executar a API

Ainda na pasta `API/GestaoPatrimonios/GestaoPatrimonios`:

```bash
dotnet restore
dotnet run --launch-profile https
```

O perfil `https` configurado no projeto disponibiliza:

- API HTTPS: [https://localhost:7063](https://localhost:7063)

- API HTTP: [http://localhost:5164](http://localhost:5164)

- Swagger: [https://localhost:7063/swagger](https://localhost:7063/swagger)

Se o certificado local do ASP.NET Core não estiver instalado, execute:

```bash
dotnet dev-certs https --trust
```

Confirme a instalação do certificado quando o sistema operacional solicitar e reinicie a API.

## 5. Executar a partir da raiz do repositório (alternativa )

Também é possível executar usando o caminho completo do projeto:

```bash
dotnet restore API/GestaoPatrimonios/GestaoPatrimonios/GestaoPatrimonios.csproj
dotnet run --project API/GestaoPatrimonios/GestaoPatrimonios/GestaoPatrimonios.csproj --launch-profile https
```

A API carrega `.env` usando o diretório de trabalho atual. Se iniciar o comando pela raiz e ocorrer erro informando que `CONNECTION_STRING` ou `JWT_KEY` não foi encontrada, execute a partir da pasta do projeto conforme a seção 4 para garantir que o arquivo `.env` seja localizado.

## 6. Frontend Node.js

O frontend não pode ser instalado/iniciado com o conteúdo atualmente disponível. O caminho `UI/patrimonio` está marcado no Git como submódulo, mas falta `.gitmodules` com o endereço de origem; além disso, não há `package.json` nem arquivo de lock no clone. Assim, não é possível confirmar se o frontend usa Next.js, Vite ou outro framework, nem quais comandos e versões npm deve usar.

Para disponibilizar os passos de execução do frontend, é necessário corrigir o repositório para incluir a configuração do submódulo (ou versionar o código diretamente ) e garantir que o frontend contenha seu `package.json` e lockfile. Depois disso, os comandos corretos devem ser obtidos dos scripts declarados nesse `package.json`.

## Testes

Não foi localizado projeto de testes (`*Tests.csproj`) no backend. Não há, portanto, um comando de testes automatizados documentado para este clone.

## Estrutura disponível

```
.
├── API/
│   └── GestaoPatrimonios/
│       ├── GestaoPatrimonios.sln
│       └── GestaoPatrimonios/
│           ├── Applications/
│           ├── Contexts/
│           ├── Controllers/
│           ├── Domains/
│           ├── Repositories/
│           ├── .env.example
│           └── GestaoPatrimonios.csproj
└── UI/
    └── patrimonio/  # gitlink para submódulo; origem/código não disponível neste clone
```

## Itens recomendados para tornar o projeto executável do início ao fim

1. Corrigir o registro Git do frontend: adicionar `.gitmodules` com a URL correta do submódulo e validar que o repositório referenciado está acessível, ou substituir o gitlink pelos arquivos do frontend.

1. Incluir e versionar o `package.json` e o lockfile do frontend, sem incluir `node_modules` ou arquivos de segredos.

1. Adicionar migrações do Entity Framework ou um script SQL de criação do schema do banco, e documentar a ordem de preparação.

1. Manter `JWT_KEY` e credenciais do SQL Server apenas em `.env`/secret store local ou de deployment; nunca adicionar segredos reais ao Git.
