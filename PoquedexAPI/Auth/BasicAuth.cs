using MySql.Data.MySqlClient;
using System.Net;
using System.Text;

namespace PoquedexAPI.Auth
{
    public class BasicAuth
    {
        private readonly RequestDelegate _next;

        public BasicAuth(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Verifica se o cabeçalho Authorization está presente
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Cabeçalho de autorização ausente.");
                return;
            }

            // Obtém o valor do cabeçalho Authorization
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Tipo de autenticação inválido.");
                return;
            }

            // Decodifica as credenciais
            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            var credentials = decodedCredentials.Split(':', 2);

            if (credentials.Length != 2 || !IsAuthorized(credentials[0], credentials[1]))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Credenciais inválidas.");
                return;
            }

            // Continua para o próximo middleware
            await _next(context);
        }
      
        public bool IsAuthorized(string username, string password)
        {
            
            // Substitua por sua lógica de validação de credenciais
            var storedUsername = "admin";
            var f = new BasicAuth(null).GeneratePasswordHash("password");
            var storedPasswordHash = f;// "$2a$11$WMUWmXMD0CdAWkHPi94gXO5O/4TOAHcLPMF9XLdVUa1i7svmpziGG";   // Hash de "password"
            conexao(storedPasswordHash);

            if (username != storedUsername)
                return false;

            // Verifica a senha usando Bcrypt
            return BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);
        
        }
        public string GeneratePasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        void conexao(string valor)
        {
            //CONEXAO
            try
            {
                var strConexao = "server=localhost;uid=root;database=teste";
                var connection = new MySqlConnection(strConexao);
                connection.Open();
                Console.WriteLine("conexao efetuada");

                //TESTE DE COMANDO
                var comando = new MySqlCommand($"insert into senhas value (default,'{valor}')", connection);
                var reader = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao conectar ao banco de dados: {ex.Message}");
            }
        }
        /*
        public string ComputeSha256Hash(string rawData)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        */
    }
}
