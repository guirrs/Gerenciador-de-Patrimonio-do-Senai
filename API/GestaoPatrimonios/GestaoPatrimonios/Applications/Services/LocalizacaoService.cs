using GestaoPatrimonios.Applications.Regras;
using GestaoPatrimonios.Domains;
using GestaoPatrimonios.DTOs.LocalizacaoDto;
using GestaoPatrimonios.Exceptions;
using GestaoPatrimonios.Interfaces;

namespace GestaoPatrimonios.Applications.Services
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
            List<Localizacao> localizacoes = _repository.Listar();

            return localizacoes.Select(localizacao => new ListarLocalizacaoDto
            {
                LocalizacaoID = localizacao.LocalizacaoID,
                NomeLocal = localizacao.NomeLocal,
                LocalSAP = localizacao.LocalSAP,
                DescricaoSAP = localizacao.DescricaoSAP,
                AreaID = localizacao.AreaID
            }).ToList();
        }

        public ListarLocalizacaoDto BuscarPorId(Guid localizacaoId)
        {
            Localizacao localizacao = _repository.BuscarPorId(localizacaoId);

            if (localizacao == null)
            {
                throw new DomainException("Localização não encontrada.");
            }

            return new ListarLocalizacaoDto
            {
                LocalizacaoID = localizacao.LocalizacaoID,
                NomeLocal = localizacao.NomeLocal,
                LocalSAP = localizacao.LocalSAP,
                DescricaoSAP = localizacao.DescricaoSAP,
                AreaID = localizacao.AreaID
            };
        }

        public void Adicionar(CriarLocalizacaoDto dto)
        {
            Validar.ValidarNome(dto.NomeLocal);

            if (!_repository.AreaExiste(dto.AreaID))
            {
                throw new DomainException("Área informada não existe.");
            }

            if (!_repository.UsuarioExiste(dto.UsuarioID))
            {
                throw new DomainException("Usuário responsável informado não existe.");
            }

            Localizacao localExistente = _repository.BuscarPorNome(dto.NomeLocal, dto.AreaID);

            if (localExistente != null)
            {
                throw new DomainException("Já existe um local cadastrado com esse nome nessa área.");
            }

            Localizacao localizacao = new Localizacao
            {
                NomeLocal = dto.NomeLocal,
                LocalSAP = dto.LocalSAP,
                DescricaoSAP = dto.DescricaoSAP,
                AreaID = dto.AreaID,
                Ativo = true
            };

            _repository.Adicionar(localizacao);

            _repository.VincularUsuario(localizacao.LocalizacaoID, dto.UsuarioID);
        }

        public void Atualizar(Guid localizacaoId, CriarLocalizacaoDto dto)
        {
            Validar.ValidarNome(dto.NomeLocal);

            Localizacao localizacaoBanco = _repository.BuscarPorId(localizacaoId);

            if (localizacaoBanco == null)
            {
                throw new DomainException("Localização não encontrada.");
            }

            if (!_repository.AreaExiste(dto.AreaID))
            {
                throw new DomainException("Área informada não existe.");
            }

            if (!_repository.UsuarioExiste(dto.UsuarioID))
            {
                throw new DomainException("Usuário responsável informado não existe.");
            }

            Localizacao localExistente = _repository.BuscarPorNome(dto.NomeLocal, dto.AreaID);

            if (localExistente != null && localExistente.LocalizacaoID != localizacaoId)
            {
                throw new DomainException("Já existe um local cadastrado com esse nome nessa área.");
            }

            localizacaoBanco.NomeLocal = dto.NomeLocal;
            localizacaoBanco.LocalSAP = dto.LocalSAP;
            localizacaoBanco.DescricaoSAP = dto.DescricaoSAP;
            localizacaoBanco.AreaID = dto.AreaID;

            _repository.Atualizar(localizacaoBanco);
        }
    }
}