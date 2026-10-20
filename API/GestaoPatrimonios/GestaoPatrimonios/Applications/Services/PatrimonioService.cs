using CsvHelper;
using CsvHelper.Configuration;
using GestaoPatrimonios.Applications.Mapeamentos;
using GestaoPatrimonios.Applications.Regras;
using GestaoPatrimonios.Domains;
using GestaoPatrimonios.DTOs.PatrimonioDto;
using GestaoPatrimonios.Exceptions;
using GestaoPatrimonios.Interfaces;
using System.Globalization;

namespace GestaoPatrimonios.Applications.Services
{
    // Sugestão: mover para a pasta DTOs/PatrimonioDto
    public class ResultadoImportacaoPatrimonioDto
    {
        public int Importados { get; set; }
        public int Ignorados { get; set; }
        public List<string> Erros { get; set; } = new();
    }

    public class PatrimonioService
    {
        private const string LocalizacaoSemLocal = "Sem local";
        private const string StatusPatrimonioAtivo = "Ativo";
        private const string TipoAlteracaoAtualizacaoDados = "Atualização de dados";

        private static readonly CultureInfo CulturaBrasileira = new("pt-BR");

        private readonly IPatrimonioRepository _repository;

        public PatrimonioService(IPatrimonioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarPatrimonioDto> Listar()
        {
            return _repository.Listar()
                .Select(MapearParaDto)
                .ToList();
        }

        public ListarPatrimonioDto BuscarPorId(Guid patrimonioId)
        {
            return MapearParaDto(ObterPatrimonio(patrimonioId));
        }

        public ResultadoImportacaoPatrimonioDto Adicionar(IFormFile arquivoCsv, Guid usuarioId)
        {
            if (arquivoCsv == null || arquivoCsv.Length == 0)
                throw new DomainException("Arquivo CSV é obrigatório.");

            Localizacao localizacaoSemLocal = _repository.BuscarLocalizacaoPorNome(LocalizacaoSemLocal)
                ?? throw new DomainException($"Localização '{LocalizacaoSemLocal}' não cadastrada.");

            StatusPatrimonio statusAtivo = _repository.BuscarStatusPatrimonioPorNome(StatusPatrimonioAtivo)
                ?? throw new DomainException($"Status '{StatusPatrimonioAtivo}' não cadastrado.");

            TipoAlteracao tipoAlteracao = _repository.BuscarTipoAlteracaoPorNome(TipoAlteracaoAtualizacaoDados)
                ?? throw new DomainException($"Tipo de alteração '{TipoAlteracaoAtualizacaoDados}' não cadastrado.");

            List<ImportarPatrimonioCsvDto> registros = LerCsv(arquivoCsv);

            var resultado = new ResultadoImportacaoPatrimonioDto();

            // Evita cadastrar o mesmo número duas vezes dentro do próprio arquivo
            var numerosProcessados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < registros.Count; i++)
            {
                ImportarPatrimonioCsvDto item = registros[i];
                int linha = i + 2; // +1 do índice começar em 0, +1 do cabeçalho

                // Linha sem número de patrimônio é considerada vazia e ignorada
                if (string.IsNullOrWhiteSpace(item.NumeroPatrimonio))
                    continue;

                string numeroPatrimonio = item.NumeroPatrimonio.Trim();

                try
                {
                    if (!numerosProcessados.Add(numeroPatrimonio))
                    {
                        resultado.Ignorados++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.Denominacao))
                        throw new DomainException("sem denominação.");

                    string denominacao = item.Denominacao.Trim();

                    Validar.ValidarNumeroPatrimonio(numeroPatrimonio);
                    Validar.ValidarNome(denominacao);

                    decimal? valor = ConverterValor(item.ValorAquisicao);
                    DateTime? dataIncorporacao = ConverterData(item.DataIncorporacao);

                    if (_repository.BuscarPorNumeroPatrimonio(numeroPatrimonio))
                    {
                        resultado.Ignorados++;
                        continue;
                    }

                    CadastrarPatrimonio(
                        numeroPatrimonio, denominacao, valor, dataIncorporacao,
                        localizacaoSemLocal, statusAtivo, tipoAlteracao, usuarioId);

                    resultado.Importados++;
                }
                catch (DomainException ex)
                {
                    // Um registro inválido não derruba a importação inteira
                    resultado.Erros.Add($"Linha {linha} (patrimônio {numeroPatrimonio}): {ex.Message}");
                }
            }

            return resultado;
        }

        public void AtualizarStatus(Guid patrimonioId, AtualizarStatusPatrimonioDto dto)
        {
            Patrimonio patrimonioBanco = ObterPatrimonio(patrimonioId);

            if (!_repository.StatusPatrimonioExiste(dto.StatusPatrimonioID))
                throw new DomainException("Status de patrimônio informado não existe.");

            patrimonioBanco.StatusPatrimonioID = dto.StatusPatrimonioID;

            _repository.AtualizarStatus(patrimonioBanco);
        }

        // ======================= Importação =======================

        private static List<ImportarPatrimonioCsvDto> LerCsv(IFormFile arquivoCsv)
        {
            var configuracao = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",

                // Tolerâncias: erros de formatação são tratados depois, por linha,
                // em vez de derrubar a leitura do arquivo todo.
                HeaderValidated = null,   // cabeçalho que não bate 100%
                MissingFieldFound = null, // campo faltando
                BadDataFound = null,      // dado quebrado (ex.: aspas abertas e não fechadas)

                TrimOptions = TrimOptions.Trim
            };

            using var stream = arquivoCsv.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, configuracao);

            // Mapa de tradução CSV -> DTO
            csv.Context.RegisterClassMap<ImportarPatrimonioCsvMap>();

            return csv.GetRecords<ImportarPatrimonioCsvDto>().ToList();
        }

        private void CadastrarPatrimonio(
            string numeroPatrimonio,
            string denominacao,
            decimal? valor,
            DateTime? dataIncorporacao,
            Localizacao localizacao,
            StatusPatrimonio status,
            TipoAlteracao tipoAlteracao,
            Guid usuarioId)
        {
            Patrimonio patrimonio = new Patrimonio
            {
                Denominacao = denominacao,
                NumeroPatrimonio = numeroPatrimonio,
                Valor = valor,
                Imagem = null,
                LocalizacaoID = localizacao.LocalizacaoID,
                StatusPatrimonioID = status.StatusPatrimonioID
            };

            _repository.Adicionar(patrimonio);

            _repository.AdicionarLog(new LogPatrimonio
            {
                DataTransferencia = dataIncorporacao ?? DateTime.Now,
                TipoAlteracaoID = tipoAlteracao.TipoAlteracaoID,
                StatusPatrimonioID = patrimonio.StatusPatrimonioID,
                PatrimonioID = patrimonio.PatrimonioID,
                UsuarioID = usuarioId,
                LocalizacaoID = patrimonio.LocalizacaoID
            });
        }

        private static decimal? ConverterValor(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return null;

            // Formato brasileiro: "1.234,56" (aceita também "R$ 1.234,56")
            if (decimal.TryParse(texto, NumberStyles.Currency, CulturaBrasileira, out decimal valor))
                return valor;

            throw new DomainException($"valor de aquisição inválido ('{texto}').");
        }

        private static DateTime? ConverterData(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return null;

            if (DateTime.TryParse(texto, CulturaBrasileira, DateTimeStyles.None, out DateTime data))
                return data;

            throw new DomainException($"data de incorporação inválida ('{texto}').");
        }

        // ======================= Busca / mapeamento =======================

        private Patrimonio ObterPatrimonio(Guid patrimonioId)
        {
            return _repository.BuscarPorId(patrimonioId)
                ?? throw new DomainException("Patrimônio não encontrado.");
        }

        private static ListarPatrimonioDto MapearParaDto(Patrimonio patrimonio)
        {
            return new ListarPatrimonioDto
            {
                PatrimonioID = patrimonio.PatrimonioID,
                Denominacao = patrimonio.Denominacao,
                NumeroPatrimonio = patrimonio.NumeroPatrimonio,
                Valor = patrimonio.Valor,
                Imagem = patrimonio.Imagem,
                LocalizacaoID = patrimonio.LocalizacaoID,
                StatusPatrimonioID = patrimonio.StatusPatrimonioID
            };
        }
    }
}
