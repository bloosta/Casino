using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasinoFrameworkApp.Services
{
    public record ClientDto(int Id, string Name);
    public record GameDto(int Id, DateTime PlayedAt, string Type, List<int> PlayerIds)
    {
        public string PlayerIdsDisplay => string.Join(", ", PlayerIds);
    }

    public interface IApiClientService
    {
        // Пользователи
        Task RegisterUserAsync(string username, string password);
        Task<bool> LoginAsync(string username, string password);

        // Клиенты
        Task CreateClientAsync(string name, bool useRawSql = false);
        Task UpdateClientAsync(int id, string name, bool useRawSql = false);
        Task DeleteClientAsync(int id, bool useRawSql = false);
        Task<List<ClientDto>> SearchClientsAsync(string? nameFilter, bool useRawSql = false);

        // Игры
        Task AddGameAsync(DateTime playedAt, string type, List<int> clientIds, bool useRawSql = false);
        Task UpdateGameAsync(int id, DateTime playedAt, string type, List<int> clientIds, bool useRawSql = false);
        Task DeleteGameAsync(int id, bool useRawSql = false);
        Task<List<GameDto>> SearchGamesAsync(DateTime? from, DateTime? to, string? typeFilter, int? clientId, bool useRawSql = false);
    }
}
