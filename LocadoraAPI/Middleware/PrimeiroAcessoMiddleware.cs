using System.Security.Claims;

namespace LocadoraAPI.Middleware;

public class PrimeiroAcessoMiddleware
{
    private readonly RequestDelegate _next;

    public PrimeiroAcessoMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower() ?? "";

        // Rotas liberadas mesmo com primeiro acesso pendente
        var rotasLiberadas = new[]
        {
            "/auth/login",
            "/auth/alterar-senha",
            "/swagger"
        };

        bool eRotaLiberada = rotasLiberadas.Any(r => path.StartsWith(r));

        if (!eRotaLiberada && context.User.Identity?.IsAuthenticated == true)
        {
            var primeiroAcesso = context.User.FindFirst("primeiroAcesso")?.Value;

            if (primeiroAcesso == "True")
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new
                {
                    mensagem = "Você precisa alterar sua senha antes de continuar.",
                    redirecionarPara = "/auth/alterar-senha"
                });
                return;
            }
        }

        await _next(context);
    }
}