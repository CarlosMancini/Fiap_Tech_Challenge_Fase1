namespace Core.Gateways
{
    public interface IBrasilGateway
    {
        Task<string> BuscarDDDAsync(int ddd);
    }
}
