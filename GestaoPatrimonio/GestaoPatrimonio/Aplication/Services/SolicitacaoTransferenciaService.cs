using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.SolicitacaoTransferenciaDto;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Aplication.Services
{
    public class SolicitacaoTransferenciaService
    {
        private readonly ISolicitacaoTransferenciaRepository _repository;
        private readonly IUsuarioRepository _usuarioRepository;

        public SolicitacaoTransferenciaService(ISolicitacaoTransferenciaRepository repository, IUsuarioRepository usuarioRepository)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
        }

        public List<ListarSolicitacaoTransferenciaDto> Listar()
        {
            return _repository.Listar().Select(s => SolicitacaoTransferenciaParaDto.ConverterParaDto(s)).ToList();
        }

        public ListarSolicitacaoTransferenciaDto ObterPorId(Guid id)
        {
           SolicitacaoTransferencia solicitacao = _repository.ObterPorId(id);

            if (solicitacao == null)
                throw new DomainException("Solicitacao de tranferencia não encontrada.");

            return SolicitacaoTransferenciaParaDto.ConverterParaDto(solicitacao);
        }
    }
}
