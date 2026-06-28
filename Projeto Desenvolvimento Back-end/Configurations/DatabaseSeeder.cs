using Domain.Entities;
using Domain.Helpers;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Projeto_Desenvolvimento_Back_end.Configurations
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(DatabaseContext context)
        {
            await context.Database.MigrateAsync();

            if (await context.Usuarios.AnyAsync())
                return;

            SeedUsuarios(context);

            var unidades = SeedUnidades(context);
            var produtos = SeedProdutos(context);
            await context.SaveChangesAsync();

            SeedCardapioEEstoque(context, unidades, produtos);
            await context.SaveChangesAsync();
        }

        private static void SeedUsuarios(DatabaseContext context)
        {
            context.Usuarios.AddRange(
                NovoUsuario("Administrador", "admin@raizes.com", "Admin@123", Roles.Admin),
                NovoUsuario("Gerente", "gerente@raizes.com", "Gerente@123", Roles.Gerente),
                NovoUsuario("Atendente", "atendente@raizes.com", "Atendente@123", Roles.Atendente),
                NovoUsuario("Cozinha", "cozinha@raizes.com", "Cozinha@123", Roles.Cozinha),
                NovoUsuario("Cliente", "cliente@raizes.com", "Cliente@123", Roles.Cliente)
            );
        }

        private static Usuario NovoUsuario(string nome, string email, string senha, string role) => new()
        {
            Nome = nome,
            Email = email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha),
            Role = role,
            Ativo = true
        };

        private static List<Unidade> SeedUnidades(DatabaseContext context)
        {
            var unidades = new List<Unidade>
            {
                new() { Nome = "Recife Centro", Cidade = "Recife", Estado = "PE", Endereco = "Rua da Aurora, 100", Ativa = true },
                new() { Nome = "Salvador Shopping", Cidade = "Salvador", Estado = "BA", Endereco = "Av. Tancredo Neves, 2915", Ativa = true }
            };
            context.Unidades.AddRange(unidades);
            return unidades;
        }

        private static List<Produto> SeedProdutos(DatabaseContext context)
        {
            var produtos = new List<Produto>
            {
                new() { Nome = "Tapioca de queijo coalho", Descricao = "Tapioca recheada com queijo coalho", Preco = 12.90m, Categoria = "Salgados", Ativo = true, Sazonal = false },
                new() { Nome = "Cuscuz recheado", Descricao = "Cuscuz nordestino recheado", Preco = 15.50m, Categoria = "Salgados", Ativo = true, Sazonal = false },
                new() { Nome = "Suco de cajá", Descricao = "Suco natural de cajá", Preco = 8.00m, Categoria = "Bebidas", Ativo = true, Sazonal = true },
                new() { Nome = "Bolo de macaxeira", Descricao = "Bolo cremoso de macaxeira", Preco = 10.00m, Categoria = "Sobremesas", Ativo = true, Sazonal = false }
            };
            context.Produtos.AddRange(produtos);
            return produtos;
        }

        private static void SeedCardapioEEstoque(DatabaseContext context, List<Unidade> unidades, List<Produto> produtos)
        {
            var recife = unidades[0];
            var salvador = unidades[1];

            var tapioca = produtos[0];
            var cuscuz = produtos[1];
            var suco = produtos[2];
            var bolo = produtos[3];

            // Cardápio: ambas as unidades vendem os 4 produtos (Salvador com preço
            // customizado na tapioca para demonstrar variação regional).
            context.UnidadeProdutos.AddRange(
                Cardapio(recife, tapioca), Cardapio(recife, cuscuz), Cardapio(recife, suco), Cardapio(recife, bolo),
                Cardapio(salvador, tapioca, 13.90m), Cardapio(salvador, cuscuz), Cardapio(salvador, suco), Cardapio(salvador, bolo)
            );

            // Estoque: cada unidade com 3+ produtos disponíveis e, em Recife, o
            // "Bolo de macaxeira" com estoque baixo para testar o erro 409.
            context.Estoques.AddRange(
                Estoque(recife, tapioca, 50, 10),
                Estoque(recife, cuscuz, 40, 10),
                Estoque(recife, suco, 100, 20),
                Estoque(recife, bolo, 1, 5),
                Estoque(salvador, tapioca, 30, 10),
                Estoque(salvador, cuscuz, 20, 5),
                Estoque(salvador, suco, 60, 15),
                Estoque(salvador, bolo, 25, 5)
            );
        }

        private static UnidadeProduto Cardapio(Unidade unidade, Produto produto, decimal? precoCustomizado = null) => new()
        {
            UnidadeId = unidade.Id,
            ProdutoId = produto.Id,
            Disponivel = true,
            PrecoCustomizado = precoCustomizado
        };

        private static Estoque Estoque(Unidade unidade, Produto produto, int atual, int minima) => new()
        {
            UnidadeId = unidade.Id,
            ProdutoId = produto.Id,
            QuantidadeAtual = atual,
            QuantidadeMinima = minima
        };
    }
}
