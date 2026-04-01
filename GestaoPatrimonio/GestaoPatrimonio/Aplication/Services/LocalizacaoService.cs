using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Aplication.Regras;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.Localizacao;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Aplication.Services
{
    public class LocalizacaoService
    {
        private readonly ILocalizacaoRepository _repository;

        public LocalizacaoService(ILocalizacaoRepository repository)
        {
            _repository = repository;
        }

        public List<ListarLocalizacaoDto> Listar()
        {
            return _repository.Listar().Select(l => LocalizacaoParaDto.ConverterParaDto(l)).ToList();
        }

        public ListarLocalizacaoDto ObterPorID(Guid id)
        {
            Localizacao local = _repository.ObterPorID(id);

            if (local == null)
            {
                throw new DomainException("Localização não encontrada");
            }

            return LocalizacaoParaDto.ConverterParaDto(local);
        }

        public void Adicionar(CriarLocalizacaoDto dto)
        {
            Validar.ValidarNome(dto.NomeLocal);

            bool Cadastrado = _repository.NomeExiste(dto.NomeLocal);

            if (Cadastrado)
            {
                throw new DomainException("Local ja cadastrado");
            }

            if (!_repository.AreaExiste(dto.AreaID))
            {
                throw new DomainException("Area informada não existe.");
            }

            Localizacao localizacao = new Localizacao
            {
                NomeLocal = dto.NomeLocal,
                DescricaoSAP = dto.Descricaco,
                LocalSAP = dto.LocalSAP,
                AreaID = dto.AreaID,
            };

            _repository.Adicionar(localizacao);
        }

        public void Atualizar(Guid id, CriarLocalizacaoDto dto)
        {
            Validar.ValidarNome(dto.NomeLocal);

            Localizacao localizacaoBanco = _repository.ObterPorID(id);

            bool Cadastrado = _repository.NomeExiste(dto.NomeLocal);

            if (Cadastrado)
            {
                throw new DomainException("Local ja cadastrado");
            }

            if (localizacaoBanco == null)
            {
                throw new DomainException("Localização não encontrada.");
            }

            if (!_repository.AreaExiste(dto.AreaID))
            {
                throw new DomainException("Area informada não existe.");
            }

            localizacaoBanco.LocalizacaoID = id;
            localizacaoBanco.NomeLocal = dto.NomeLocal;
            localizacaoBanco.AreaID = dto.AreaID;
            localizacaoBanco.DescricaoSAP = dto.Descricaco;
            localizacaoBanco.LocalSAP = dto.LocalSAP;

            _repository.Atualizar(localizacaoBanco);
        }
    }
}
