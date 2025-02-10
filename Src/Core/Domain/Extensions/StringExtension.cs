using System.Text.RegularExpressions;

namespace FIAP.Pos.Hackathon.Micro.Servico.Processamento.Imagens.Producao.Domain.Extensions
{
    public static class StringExtension
    {
        public static bool IsBlank(this string? text)
        {
            return text != null && string.IsNullOrWhiteSpace(text) && text.Length > 0;
        }

        public static string? ToSnakeCase(this string? str)
        {
            var pattern =
                new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+");

            return str == null
                ? null
                : string
                    .Join("_", pattern.Matches(str).Cast<Match>().Select(m => m.Value))
                    .ToLower();
        }
    }
}
