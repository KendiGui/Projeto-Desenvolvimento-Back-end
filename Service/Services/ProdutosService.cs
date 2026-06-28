using Core.Data;
using Domain.Contracts.Exceptions;
using Domain.Contracts.Requests;
using Domain.Contracts.Responses;
using Domain.Entities;
using Domain.Repositories;
using Service.Interfaces;

namespace Service.Services
{
    public class ProdutosService(IProdutoRepository produtoRepository, IUnitOfWork unitOfWork) : IProdutosService
    {
        public async Task<ResultPaginado<ProdutoResponse>> ListaProdutos(int pagina = 1, int tamanhoPagina = 10)
        {
            var produtos = await produtoRepository.ListPaginatedAsync(pagina, tamanhoPagina);

            if (!produtos.Items.Any()) throw new EmptyListException("Nenhum produto encontrado");

            return produtos;
        }

        public async Task<ProdutoResponse> GetProduto(long produtoId)
        {
            var produto = await produtoRepository.GetByIdAsync(produtoId);

            if (produto is null) throw new NotFoundException("Produto não encontrado");

            return new ProdutoResponse
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Categoria = produto.Categoria,
                Ativo = produto.Ativo,
                Sazonal = produto.Sazonal
            };
        }

        public async Task<ProdutoResponse> CadastraProduto(ProdutoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new ValidationException("Nome do produto é obrigatório.",
                    [new("nome", "Campo obrigatório")]);

            if (request.Preco < 0)
                throw new ValidationException("Preço não pode ser negativo.",
                    [new("preco", "Deve ser maior ou igual a zero")]);

            var produto = new Produto
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                Preco = request.Preco,
                Categoria = request.Categoria,
                Ativo = request.Ativo,
                Sazonal = request.Sazonal
            };

            await produtoRepository.AddAsync(produto);
            await unitOfWork.CommitAsync();

            return new ProdutoResponse
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Categoria = produto.Categoria,
                Ativo = produto.Ativo,
                Sazonal = produto.Sazonal
            };
        }

        public async Task<ProdutoResponse> AtualizaProduto(long produtoId, ProdutoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new ValidationException("Nome do produto é obrigatório.",
                    [new("nome", "Campo obrigatório")]);

            if (request.Preco < 0)
                throw new ValidationException("Preço não pode ser negativo.",
                    [new("preco", "Deve ser maior ou igual a zero")]);

            var produto = await produtoRepository.GetByIdAsync(produtoId);

            if (produto is null) throw new NotFoundException("Produto não encontrado");

            produto.Nome = request.Nome;
            produto.Descricao = request.Descricao;
            produto.Preco = request.Preco;
            produto.Categoria = request.Categoria;
            produto.Ativo = request.Ativo;
            produto.Sazonal = request.Sazonal;

            await produtoRepository.UpdateAsync(produto);
            await unitOfWork.CommitAsync();

            return new ProdutoResponse
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Categoria = produto.Categoria,
                Ativo = produto.Ativo,
                Sazonal = produto.Sazonal
            };
        }

        public async Task RemoveProduto(long produtoId)
        {
            var produto = await produtoRepository.GetByIdAsync(produtoId);

            if (produto is null) throw new NotFoundException("Produto não encontrado");

            produto.Ativo = false;
            await produtoRepository.UpdateAsync(produto);
            await unitOfWork.CommitAsync();
        }
    }
}
