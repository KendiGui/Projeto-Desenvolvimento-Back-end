namespace Domain.Helpers
{
    /// <summary>
    /// Perfis de acesso da aplicação. Usados nas policies/atributos [Authorize].
    /// </summary>
    public static class Roles
    {
        public const string Admin = "ADMIN";
        public const string Gerente = "GERENTE";
        public const string Atendente = "ATENDENTE";
        public const string Cozinha = "COZINHA";
        public const string Cliente = "CLIENTE";
    }
}
