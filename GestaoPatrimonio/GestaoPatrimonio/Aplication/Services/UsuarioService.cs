using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Aplication.Regras;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.UsuarioDto;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Aplication.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarUsuarioDto> Listar()
        {
            return _repository.Listar().Select(usuario => UsuarioParaDto.ConverterParaDto(usuario)).ToList();
        }

        public ListarUsuarioDto ObterPorId(Guid id)
        {
            Usuario user = _repository.ObterPorId(id);

            if (user == null)
                throw new DomainException("ID não encontrado.");

            return UsuarioParaDto.ConverterParaDto(user);
        }

        public void ValidarUsuarioDuplicado(CriarUsuarioDto dto)
        {
            Usuario usuarioDuplicado = _repository.BuscarDuplicado(dto.NIF, dto.CPF, dto.Email);

            if (usuarioDuplicado != null)
            {
                if (usuarioDuplicado.NIF == dto.NIF)
                    throw new DomainException("Ja existe um usuario cadastrado com esse NIF.");

                if (usuarioDuplicado.CPF == dto.CPF)
                    throw new DomainException("Ja existe um usuario cadastrado com esse CPF.");

                if (usuarioDuplicado.Email.ToLower() == dto.Email.ToLower())
                    throw new DomainException("Ja existe um usuario cadastrado com esse Email.");
            }

            if (!_repository.EnderecoExiste(dto.EnderecoID))
                throw new DomainException("Endereco informado não existe.");

            if (!_repository.CargoExiste(dto.CargoID))
                throw new DomainException("Cargo informado não existe.");

            if (!_repository.TipoUsuarioExiste(dto.TipoUsuarioID))
                throw new DomainException("Tipo Usuario informado não existe.");
        }

        public void Adicionar(CriarUsuarioDto dto)
        {
            Validar.ValidarCPF(dto.CPF);
            Validar.ValidarEmail(dto.Email);
            Validar.ValidarNIF(dto.NIF);
            Validar.ValidarNome(dto.Nome);

            Usuario usuarioDuplicado = _repository.BuscarDuplicado(dto.NIF, dto.CPF, dto.Email);

            this.ValidarUsuarioDuplicado(dto);

            _repository.Adicionar(UsuarioParaDto.DtoParaDomain(dto, null));
        }

        public void Atualizar(Guid id, CriarUsuarioDto dto)
        {
            Validar.ValidarCPF(dto.CPF);
            Validar.ValidarEmail(dto.Email);
            Validar.ValidarNIF(dto.NIF);
            Validar.ValidarNome(dto.Nome);

            Usuario usuarioBanco = _repository.ObterPorId(id);
            if (usuarioBanco == null)
                throw new DomainException("Usuario não encontrado.");

            this.ValidarUsuarioDuplicado(dto);

            _repository.Atualizar(UsuarioParaDto.DtoParaDomain(dto, id));
        }

        public void AtualizarStatus(Guid id, AtualizarStatusUsuarioDto dto)
        {
            Usuario usuarioBanco = _repository.ObterPorId(id);

            if (usuarioBanco == null)
                throw new DomainException("Usuario não encontrado.");

            _repository.AtualizarStatus(id, dto.Ativo);
        }
    }
}
