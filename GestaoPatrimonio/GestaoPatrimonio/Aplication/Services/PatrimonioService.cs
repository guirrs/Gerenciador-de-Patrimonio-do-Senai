using CsvHelper;
using CsvHelper.Configuration;
using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Aplication.Mapeamentos;
using GestaoPatrimonio.Aplication.Regras;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.PatrimonioDto;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;
using System.Globalization;

namespace GestaoPatrimonio.Aplication.Services
{
    public class PatrimonioService
    {
        private readonly IPatrimonioRepository _repository;

        public PatrimonioService(IPatrimonioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarPatrimonioDto> Listar()
        {
            return _repository.Listar().Select(l => PatrimonioParaDto.ListarPatrimonioParaDto(l)).ToList();
        }

        public ListarPatrimonioDto ObterPorId(Guid id)
        {
            Patrimonio patrimonio = _repository.ObterPorId(id);

            if (patrimonio == null)
                throw new DomainException("Patromonio não encontrado.");

            return PatrimonioParaDto.ListarPatrimonioParaDto(patrimonio);
        }

        public void Adicionar(IFormFile arquivoCsv, Guid usuarioId)
        {
            if (arquivoCsv == null || arquivoCsv.Length == 0)
            {
                throw new DomainException("Arquivo CSV é obrigatório.");
            }

            Localizacao localizacaoSemLocal = _repository.BuscarLocalizacaoPorNome("Sem local");

            if (localizacaoSemLocal == null)
            {
                throw new DomainException("Localização 'Sem local' não cadastrada.");
            }

            StatusPatrimonio statusAtivo = _repository.BuscarStatusPatrimonioPorNome("Ativo");

            if (statusAtivo == null)
            {
                throw new DomainException("Status 'Ativo' não cadastrado.");
            }

            TipoAlteracao tipoAlteracao = _repository.BuscarTipoAlteracaoPorNome("Atualização de dados");

            if (tipoAlteracao == null)
            {
                throw new DomainException("Tipo de alteração 'Atualização de dados' não cadastrado.");
            }

            List<ImportarPatrimonioCsvDto> registros;

            // Abre o arquivo enviado(IFormFile)
            using (var stream = arquivoCsv.OpenReadStream())
            // Le os arquivos como texto
            using (var reader = new StreamReader(stream))

            // Criar Leitor de CSV com configurações personalizadas
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Define que separador é o ponto e virgula
                Delimiter = ";",

                // Ingnora erros caso o cabeçalho não bata 100%
                // Não trara a apliacacao por conta da formatação, vamos tratar os erros depois.
                HeaderValidated = null,

                // Ingnora o erro se faltar algum campo
                MissingFieldFound = null,

                // Ingnora dados "quebrados" no CSV
                BadDataFound = null, // Ex. abriu aspas e não fechou

                // Renive espaços extras automaticamente
                TrimOptions = TrimOptions.Trim,
            }))
            {
                // Registra o mapa que criamos (Traducao CSV -> DTO)
                csv.Context.RegisterClassMap<ImportarPatrimonioCsvMap>();

                // Pega todas as linhas do CSV e converte para uma lista de DTO
                registros = csv.GetRecords<ImportarPatrimonioCsvDto>().ToList();
            }

            var erros = new List<String>();

            foreach (var item in registros)
            {
                // se nçao tem numeri de patrimônio, ingnora o registro
                if (string.IsNullOrWhiteSpace(item.NumeroPatrimonio))
                {
                    // ingnora e vai para o proximo
                    continue;
                }

                // Remove espaços extras do numero
                string numeroPatrimonio = item.NumeroPatrimonio.Trim();

                if (string.IsNullOrWhiteSpace(item.Denominacao))
                {
                    erros.Add($"Patrimônio {numeroPatrimonio} sem denominação.");
                    continue; // não cadastra e segue o loop
                }

                string denominacao = item.Denominacao.Trim();

                DateTime? dataIncorporacao = null;

                // usa o formato brasileiro só para ler
                // Depois pega o DateTime e formata
                if (!string.IsNullOrWhiteSpace(item.Denominacao))
                {
                    if (DateTime.TryParse(item.DataIncorporacao, new CultureInfo("pt-BR"), DateTimeStyles.None
                        , out DateTime dataConvertida))
                    {
                        dataIncorporacao = dataConvertida;
                    }
                }

                decimal? valorAquisicao = null;

                if (!string.IsNullOrWhiteSpace(item.ValorAquisicao))
                {
                    // Remove separador de milhar e ajusta decimal
                    string valorTexto = item.ValorAquisicao.Replace(".", "").Replace(",", ".");

                    // TrtParse - converte strint -> decimal
                    // NumberStyles.Any -> define quais dormatos de números são permitidos - any aceita qualquer numero, mesmo com sinal
                    // espaco, etc.
                    // out decimal valorCinvertido -> se der certo: cria a variavel com o valor convertido;
                    if (decimal.TryParse(valorTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valorConvertido))
                    {
                        valorAquisicao = valorConvertido;
                    }

                    Validar.ValidarNumeroPatrimonio(numeroPatrimonio);
                    Validar.ValidarNome(denominacao);

                    bool patrimonioExistente = _repository.BuscarPorNumeroPatrimonio(numeroPatrimonio);

                    if (patrimonioExistente == true)
                    {
                        continue;
                    }

                    Patrimonio patrimonio = new Patrimonio
                    {
                        Denominacao = denominacao,
                        NumeroPatrimonio = numeroPatrimonio,
                        Valor = valorAquisicao,
                        Imagem = null,
                        LocalizacaoID = localizacaoSemLocal.LocalizacaoID,
                        StatusPatrimonioID = statusAtivo.StatusPatrimonioID
                    };

                    _repository.Adicionar(patrimonio);

                    LogPatrimonio log = new LogPatrimonio
                    {
                        DataTransferencia = dataIncorporacao ?? DateTime.Now,
                        TipoAlteracaoID = tipoAlteracao.TipoAlteracaoID,
                        StatusPatrimonioID = patrimonio.StatusPatrimonioID,
                        PatrimonioID = patrimonio.PatrimonioID,
                        UsuarioID = usuarioId,
                        LocalizacaoID = patrimonio.LocalizacaoID
                    };

                    _repository.AdicionarLog(log);
                }
            }
        }
            public void AtualizarStatus(Guid patrimonioId, AtualizarStatusPatrimonio dto)
        {
            Patrimonio patrimonioBanco = _repository.ObterPorId(patrimonioId);

            if (patrimonioBanco == null)
            {
                throw new DomainException("Patrimônio não encontrado.");
            }

            if (!_repository.StatusPatrimonioExiste(dto.StatusPatrimonioID))
            {
                throw new DomainException("Status de patrimônio informado não existe.");
            }

            patrimonioBanco.StatusPatrimonioID = dto.StatusPatrimonioID;

            _repository.AtualizarStatus(patrimonioBanco);
        }
    }
}


