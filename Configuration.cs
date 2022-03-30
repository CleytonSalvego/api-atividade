namespace api
{
    public static class Configuration
    {
        public static string JwtKey { get; set; } = "JwtKey"; //Colocar um hash seguro

        public static string PasswordKey { get; set; } = "123"; //Colocar uma senha padrão

        public static string ConnectionString { get; set; } = "Server=localhost;DataBase=api-atividade;Uid=root;Pwd="; //Colocar a conection string do banco 
    }
}
