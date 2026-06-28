using Core.Data;
using Domain.Contracts.Exceptions;
using Domain.Contracts.Requests;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Repositories;
using Service.Interfaces;

namespace Service.Services
{
    public class UnidadesService(IUnidadeRepository unidadeRepository, IUnitOfWork unitOfWork) : IUnidadesService
    {
        public async Task<ResultPaginado<UnidadeResponse>> ListaUnidades(int pagina = 1, int tamanhoPagina = 10)
        {
            var unidades = await unidadeRepository.ListPaginatedAsync(pagina, tamanhoPagina);

            if (!unidades.Items.Any()) throw new EmptyListException("Nenhuma unidade encontrada");

            return unidades;
        }

        public async Task<UnidadeResponse> GetUnidade(long unidadeId)
        {
            var unidade = await unidadeRepository.GetByIdAsync(unidadeId);

            if (unidade is null) throw new NotFoundException("Nenhuma unidade encontrada");

            return new UnidadeResponse()
            {
                Id = unidade.Id,
                Nome = unidade.Nome,
                Cidade = unidade.Cidade,
                Estado = unidade.Estado,
                Endereco = unidade.Endereco,
                Ativa = unidade.Ativa
            };
        }

        public async Task<UnidadeResponse> CadastraUnidade(UnidadeRequest request)
        {
            var unidade = new Unidade
            {
                Nome = request.Nome,
                Cidade = request.Cidade,
                Estado = request.Estado,
                Endereco = request.Endereco,
                Ativa = request.Ativa
            };

            await unidadeRepository.AddAsync(unidade);
            await unitOfWork.CommitAsync();

            return new UnidadeResponse
            {
                Id = unidade.Id,
                Nome = unidade.Nome,
                Cidade = unidade.Cidade,
                Estado = unidade.Estado,
                Endereco = unidade.Endereco,
                Ativa = unidade.Ativa
            };
        }

        public async Task<UnidadeResponse> AtualizaUnidade(long unidadeId, UnidadeRequest request)
        {
            var unidade = await unidadeRepository.GetByIdAsync(unidadeId);

            if (unidade is null) throw new NotFoundException("Unidade não encontrada");

            unidade.Nome = request.Nome;
            unidade.Cidade = request.Cidade;
            unidade.Estado = request.Estado;
            unidade.Endereco = request.Endereco;
            unidade.Ativa = request.Ativa;

            await unidadeRepository.UpdateAsync(unidade);
            await unitOfWork.CommitAsync();

            return new UnidadeResponse
            {
                Id = unidade.Id,
                Nome = unidade.Nome,
                Cidade = unidade.Cidade,
                Estado = unidade.Estado,
                Endereco = unidade.Endereco,
                Ativa = unidade.Ativa
            };
        }
    }
}
