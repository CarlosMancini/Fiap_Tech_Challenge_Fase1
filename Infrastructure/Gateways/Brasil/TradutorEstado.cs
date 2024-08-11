using System.Collections.Generic;

public static class TradutorEstado
{
    private static readonly Dictionary<string, string> EstadoMap = new Dictionary<string, string>
    {
        { "AC", "Acre" },
        { "AL", "Alagoas" },
        { "AP", "Amapá" },
        { "AM", "Amazonas" },
        { "BA", "Bahia" },
        { "CE", "Ceará" },
        { "DF", "Distrito Federal" },
        { "ES", "Espírito Santo" },
        { "GO", "Goiás" },
        { "MA", "Maranhão" },
        { "MT", "Mato Grosso" },
        { "MS", "Mato Grosso do Sul" },
        { "MG", "Minas Gerais" },
        { "PA", "Pará" },
        { "PB", "Paraíba" },
        { "PR", "Paraná" },
        { "PE", "Pernambuco" },
        { "PI", "Piauí" },
        { "RJ", "Rio de Janeiro" },
        { "RN", "Rio Grande do Norte" },
        { "RS", "Rio Grande do Sul" },
        { "RO", "Rondônia" },
        { "RR", "Roraima" },
        { "SC", "Santa Catarina" },
        { "SP", "São Paulo" },
        { "SE", "Sergipe" },
        { "TO", "Tocantins" }
    };

    public static string TraduzirSigla(string sigla)
    {
        return EstadoMap.TryGetValue(sigla, out var nome) ? nome : "Estado desconhecido";
    }
}
