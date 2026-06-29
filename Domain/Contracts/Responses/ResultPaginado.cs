namespace Domain.Contracts.Responses
{
    public class ResultPaginado<TEntity>
    {
        public IEnumerable<TEntity> Items { get; set; } = [];
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalItems { get; set; }
        public int TotalPaginas { get; set; }

        public bool HasPaginaAnterior => Pagina > 1;
        public bool HasProximaPagina => Pagina < TotalPaginas;

        public ResultPaginado<TOut> Map<TOut>(Func<TEntity, TOut> selector) => new()
        {
            Items = Items.Select(selector).ToList(),
            Pagina = Pagina,
            TamanhoPagina = TamanhoPagina,
            TotalItems = TotalItems,
            TotalPaginas = TotalPaginas
        };
    }
}
