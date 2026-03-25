CREATE DATABASE GestaoPatrimonios;
GO

USE GestaoPatrimonios;
GO

-- AREA
CREATE TABLE Area (
	AreaID		UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	NomeArea	VARCHAR(50) UNIQUE NOT NULL
);
GO

-- TIPO USUÁRIO
-- Responsavel e Coordenador
CREATE TABLE TipoUsuario(
	TipoUsuarioID	UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	NomeTipo		VARCHAR(50) UNIQUE NOT NULL
);
GO

-- CARGO
CREATE TABLE Cargo(
	CargoID		UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	NomeCargo	VARCHAR(50) UNIQUE NOT NULL
);
GO

-- TIPO PATRIMONIO
CREATE TABLE TipoPatrimonio(
	TipoPatrimonioID	UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	NomeTipo			VARCHAR(100) UNIQUE NOT NULL
);
GO

-- STATUS PATRIMONIO
-- Inativo, Ativo, Transferido, Assis. Tecnica
CREATE TABLE StatusPatrimonio(
	StatusPatrimonioID	UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	NomeStatus			VARCHAR(50) UNIQUE NOT NULL
);
GO

-- STATUS TRANSFERENCIA
-- Pendente de aprovação, Aprovado e Recusado
CREATE TABLE StatusTransferencia(
	StatusTransferenciaID	UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	NomeStatus				VARCHAR(50) UNIQUE NOT NULL
);
GO

-- TIPO ALTERAÇÃO
-- Modificação e transferência
CREATE TABLE TipoAlteracao(
	TipoAlteracaoID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	NomeTipo		VARCHAR(50) UNIQUE NOT NULL
);
GO

-- CIDADE 
CREATE TABLE Cidade(
	CidadeID	UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	NomeCidade	VARCHAR(50) NOT NULL,
	Estado		VARCHAR(50) NOT NULL 
);
GO

-- LOCAL / AMBIENTE
CREATE TABLE Localizacao(
	LocalizacaoID	UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	NomeLocal		VARCHAR(100) NOT NULL,
	LocalSAP		INT,
	DescricaoSAP	VARCHAR(100),
	Ativo			BIT DEFAULT 1,
	AreaID			UNIQUEIDENTIFIER NOT NULL,

	CONSTRAINT FK_Localizacao_Area 
		FOREIGN KEY (AreaID) REFERENCES Area(AreaID)
);
GO

-- BAIRRO
CREATE TABLE Bairro(
	BairroID	UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	NomeBairro	VARCHAR(50) NOT NULL,
	CidadeID	UNIQUEIDENTIFIER NOT NULL,

	CONSTRAINT FK_Bairro_Cidade
		FOREIGN KEY (CidadeID) REFERENCES Cidade(CidadeID)
);
GO

-- ENDEREÇO
CREATE TABLE Endereco(
	EnderecoID		UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	Logradouro		VARCHAR(100) NOT NULL,
	Numero			INT,
	Complemento		VARCHAR(20),
	CEP				VARCHAR(10),
	BairroID		UNIQUEIDENTIFIER NOT NULL,

	CONSTRAINT FK_Endereco_Bairro
		FOREIGN KEY (BairroID) REFERENCES Bairro(BairroID)
);
GO

-- Usuário
CREATE TABLE Usuario(
	UsuarioID			UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	NIF					VARCHAR(7) UNIQUE NOT NULL,
	Nome				VARCHAR(150) NOT NULL,
	RG					VARCHAR(15) UNIQUE,
	CPF					VARCHAR(11) UNIQUE NOT NULL,
	CarteiraTrabalho	VARCHAR(14) UNIQUE NOT NULL,
	Senha				VARBINARY(32) NOT NULL,
	Email				VARCHAR(150) UNIQUE NOT NULL,
	Ativo				BIT DEFAULT 1,
	EnderecoID			UNIQUEIDENTIFIER NOT NULL,
	CargoID				UNIQUEIDENTIFIER NOT NULL,
	TipoUsuarioID		UNIQUEIDENTIFIER NOT NULL,

	CONSTRAINT FK_Usuario_Endereco
		FOREIGN KEY (EnderecoID) REFERENCES Endereco(EnderecoID),

	CONSTRAINT FK_Usuario_Cargo
		FOREIGN KEY (CargoID) REFERENCES Cargo(CargoID),

	CONSTRAINT FK_Usuario_TipoUsuario
		FOREIGN KEY (TipoUsuarioID) REFERENCES TipoUsuario(TipoUsuarioID)
);
GO

-- PATRIMÔNIO
CREATE TABLE Patrimonio (
	PatrimonioID		UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	Denominacao			VARCHAR(MAX) NOT NULL,
	NumeroPatrimonio	VARCHAR(30) UNIQUE NOT NULL,
	Valor				DECIMAL(10,2),
	Imagem				VARCHAR(MAX),
	LocalizacaoID		UNIQUEIDENTIFIER NOT NULL,
	TipoPatrimonioID	UNIQUEIDENTIFIER NOT NULL,
	StatusPatrimonioID	UNIQUEIDENTIFIER NOT NULL,

	CONSTRAINT FK_Patrimonio_Localizacao
		FOREIGN KEY (LocalizacaoID) REFERENCES Localizacao(LocalizacaoID),

	CONSTRAINT FK_Patrimonio_TipoPatrimonio
		FOREIGN KEY (TipoPatrimonioID) REFERENCES TipoPatrimonio(TipoPatrimonioID),

	CONSTRAINT FK_Patrimonio_StatusPatrimonio
		FOREIGN KEY (StatusPatrimonioID) REFERENCES StatusPatrimonio(StatusPatrimonioID)
);
GO

-- LOG PATRIMÔNIO
CREATE TABLE LogPatrimonio(
	LogPatrimonioID		UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	DataTransferencia	DATETIME2(0) NOT NULL,
	TipoAlteracaoID		UNIQUEIDENTIFIER NOT NULL,
	StatusPatrimonioID	UNIQUEIDENTIFIER NOT NULL,
	PatrimonioID		UNIQUEIDENTIFIER NOT NULL,
	UsuarioID			UNIQUEIDENTIFIER NOT NULL,
	LocalizacaoID		UNIQUEIDENTIFIER NOT NULL,

	CONSTRAINT FK_LogPatrimonio_TipoAlteracao
		FOREIGN KEY (TipoAlteracaoID) REFERENCES TipoAlteracao(TipoAlteracaoID),

	CONSTRAINT FK_LogPatrimonio_StatusPatrimonio
		FOREIGN KEY (StatusPatrimonioID) REFERENCES StatusPatrimonio(StatusPatrimonioID),

	CONSTRAINT FK_LogPatrimonio_Patrimonio
		FOREIGN KEY (PatrimonioID) REFERENCES Patrimonio(PatrimonioID),

	CONSTRAINT FK_LogPatrimonio_Usuario
		FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID),

	CONSTRAINT FK_LogPatrimonio_Localizacao
		FOREIGN KEY (LocalizacaoID) REFERENCES Localizacao(LocalizacaoID)
);
GO

-- SOLICITAÇÃO DE TRANSFERÊNCIA
CREATE TABLE SolicitacaoTransferencia(
	TransferenciaID			UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	DataCriacaoSolicitante	DATETIME2(0) NOT NULL,
	DataResposta			DATETIME2(0),
	Justificativa			VARCHAR(MAX) NOT NULL,
	StatusTransferenciaID	UNIQUEIDENTIFIER NOT NULL,
	UsuarioIDSolicitacao	UNIQUEIDENTIFIER NOT NULL,
	UsuarioIDAprovacao		UNIQUEIDENTIFIER,
	PatrimonioID			UNIQUEIDENTIFIER NOT NULL,
	LocalizacaoID			UNIQUEIDENTIFIER NOT NULL,

	CONSTRAINT FK_SolicitacaoTransferencia_StatusTransferencia
		FOREIGN KEY (StatusTransferenciaID) 
			REFERENCES StatusTransferencia(StatusTransferenciaID),

	CONSTRAINT FK_SolicitacaoTransferencia_UsuarioSolicitacao
		FOREIGN KEY (UsuarioIDSolicitacao) REFERENCES Usuario(UsuarioID),

	CONSTRAINT FK_SolicitacaoTransferencia_UsuarioAprovacao
		FOREIGN KEY (UsuarioIDAprovacao) REFERENCES Usuario(UsuarioID),

	CONSTRAINT FK_SolicitacaoTransferencia_Patrimonio
		FOREIGN KEY (PatrimonioID) REFERENCES Patrimonio(PatrimonioID),

	CONSTRAINT FK_SolicitacaoTransferencia_Localizacao
		FOREIGN KEY (LocalizacaoID) REFERENCES Localizacao(LocalizacaoID)
);
GO

-- Local Usuario
CREATE TABLE LocalUsuario(
	LocalizacaoID	UNIQUEIDENTIFIER,
	UsuarioID		UNIQUEIDENTIFIER,

	CONSTRAINT PK_LocalUsuario PRIMARY KEY (LocalizacaoID, UsuarioID),

	CONSTRAINT FK_LocalUsuario_Localizacao
		FOREIGN KEY (LocalizacaoID) REFERENCES Localizacao(LocalizacaoID),

	CONSTRAINT FK_LocalUsuario_Usuario
		FOREIGN KEY (UsuarioID) REFERENCES Usuario(UsuarioID)
);

GO

-- TRIGGER PARA SOFT DELETE DE USUÁRIO
CREATE TRIGGER trg_Usuario_SoftDelete
ON Usuario
INSTEAD OF DELETE 
AS
BEGIN
	UPDATE Usuario
		SET Ativo = 0
		WHERE UsuarioID IN (SELECT UsuarioID FROM deleted);
END

GO

-- TRIGGER PARA SOFT DELETE DE LOCALIZAÇÃO
CREATE TRIGGER trg_Local_SoftDelete
ON Localizacao
INSTEAD OF DELETE
AS
BEGIN
	UPDATE Localizacao
		SET Ativo = 0
		WHERE LocalizacaoID IN (SELECT LocalizacaoID FROM deleted);
END

GO

-- TRIGGER PARA SOFT DELETE DE PATRIMÔNIO
CREATE TRIGGER trg_Patrimonio_SoftDelete
ON Patrimonio
INSTEAD OF DELETE
AS
BEGIN
	UPDATE Patrimonio
		SET StatusPatrimonioID = 
			(SELECT StatusPatrimonioID 
				FROM StatusPatrimonio
				WHERE NomeStatus = 'Inativo')
		WHERE PatrimonioID IN (SELECT PatrimonioID FROM deleted);
END

GO

-- INSERTS

-- Area
INSERT INTO Area (NomeArea) VALUES
('Bloco A - Térreo'),
('Bloco A - 1° Andar')
GO

-- TipoUsuario
INSERT INTO TipoUsuario (NomeTipo) VALUES
('Responsável'),
('Coordenador')
GO

-- Cargo
INSERT INTO Cargo (NomeCargo) VALUES
('Diretor'),
('Instrutor de Formação Profissional II')
GO

-- TipoPatrimonio
INSERT INTO TipoPatrimonio (NomeTipo) VALUES
('Mesa'),
('Notebook')
GO

-- StatusPatrimonio
-- Inativo, Ativo, Transferido, Assis. Tecnica
INSERT INTO StatusPatrimonio (NomeStatus) VALUES
('Inativo'),
('Ativo'),
('Transferido'),
('Em manutenção')
GO

-- StatusTransferencia
-- Pendente de aprovação, Aprovado e Recusado
INSERT INTO StatusTransferencia (NomeStatus) VALUES
('Pendente de aprovação'),
('Aprovado'),
('Recusado')
GO

-- TipoAlteracao
-- Modificação e transferência
INSERT INTO TipoAlteracao (NomeTipo) VALUES
('Atualização de dados'),
('Transferência')
GO

-- Cidade
INSERT INTO Cidade (NomeCidade, Estado) VALUES
('São Caetano do Sul', 'São Paulo'),
('Diadema', 'São Paulo')
GO

-- Local
INSERT INTO Localizacao (LocalSAP, DescricaoSAP, NomeLocal, AreaID) VALUES
(NULL, NULL, 'Manutenção', (SELECT AreaID FROM Area WHERE NomeArea = 'Bloco A - Térreo'))
GO

-- Bairro
INSERT INTO Bairro (NomeBairro, CidadeID) VALUES
('Centro', (SELECT CidadeID FROM Cidade WHERE NomeCidade = 'São Caetano do Sul'))
GO