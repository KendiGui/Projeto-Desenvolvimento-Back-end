using Core.Data;
using Domain.Contracts.Exceptions;
using Domain.Contracts.Requests;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Repositories;
using Service.Interfaces;

namespace Service.Services
{
    public class CardapioService(IUnidadeRepository unidadeRepository, IProdutoRepository produtoRepository, IUnidadeProdutoRepository unidadeProdutoRepository, IUnitOfWork unitOfWork) : ICardapioService
    {
        public async Task<ResultPaginado<CardapioItemResponse>> GetCardapio(long unidadeId, int pagina = 1, int tamanhoPagina = 10)
        {
            var unidade = await unidadeRepository.GetByIdAsync(unidadeId);
            if (unidade is null) throw new NotFoundException("Unidade não encontrada");

            return await unidadeProdutoRepository.GetCardapioAsync(unidadeId, pagina, tamanhoPagina);
        }

        public async Task<CardapioItemResponse> AdicionaProduto(long unidadeId, CardapioProdutoRequest request)
        {
            var unidade = await unidadeRepository.GetByIdAsync(unidadeId);
            if (unidade is null) throw new NotFoundException("Unidade não encontrada");

            var produto = await produtoRepository.GetByIdAsync(request.ProdutoId);
            if (produto is null) throw new NotFoundException("Produto não encontrado");

            var existente = await unidadeProdutoRepository.GetByUnidadeProdutoAsync(unidadeId, request.ProdutoId);
            if (existente is not null)
                throw new ValidationException("Produto já consta no cardápio desta unidade.");

            var unidadeProduto = new UnidadeProduto
            {
                UnidadeId = unidadeId,
                ProdutoId = request.ProdutoId,
                Disponivel = request.Disponivel,
                PrecoCustomizado = request.PrecoCustomizado
            };

            await unidadeProdutoRepository.AddAsync(unidadeProduto);
            await unitOfWork.CommitAsync();

            return new CardapioItemResponse
            {
                ProdutoId = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Categoria = produto.Categoria,
                Preco = unidadeProduto.PrecoCustomizado ?? produto.Preco,
                Disponivel = unidadeProduto.Disponivel,
                Sazonal = produto.Sazonal
            };
        }

        public async Task<CardapioItemResponse> AtualizaProduto(long unidadeId, long produtoId, CardapioProdutoRequest request)
        {
            var unidade = await unidadeRepository.GetByIdAsync(unidadeId);
            if (unidade is null) throw new NotFoundException("Unidade não encontrada");

            var produto = await produtoRepository.GetByIdAsync(produtoId);
            if (produto is null) throw new NotFoundException("Produto não encontrado");

            var unidadeProduto = await unidadeProdutoRepository.GetByUnidadeProdutoAsync(unidadeId, produtoId);
            if (unidadeProduto is null)
                throw new NotFoundException("Produto não consta no cardápio desta unidade.");

            unidadeProduto.Disponivel = request.Disponivel;
            unidadeProduto.PrecoCustomizado = request.PrecoCustomizado;

            await unidadeProdutoRepository.UpdateAsync(unidadeProduto);
            await unitOfWork.CommitAsync();

            return new CardapioItemResponse
            {
                ProdutoId = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Categoria = produto.Categoria,
                Preco = unidadeProduto.PrecoCustomizado ?? produto.Preco,
                Disponivel = unidadeProduto.Disponivel,
                Sazonal = produto.Sazonal
            };
        }
    }
}
