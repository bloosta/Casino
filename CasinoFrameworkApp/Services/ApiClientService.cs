using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CasinoFrameworkApp.Services
{
    public class ApiClientService : IApiClientService
    {
        private readonly HttpClient _client;

        public ApiClientService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("CasinoApi");
        }

        // Пользователи
        public async Task RegisterUserAsync(string username, string password) =>
            await _client.PostAsJsonAsync("api/users/register", new { username, password });

        public async Task<bool> LoginAsync(string username, string password)
        {
            var response = await _client.PostAsJsonAsync("api/users/login", new { username, password });
            return response.IsSuccessStatusCode;
        }

        // Клиенты
        public async Task CreateClientAsync(string name, bool useRawSql = false) =>
            await _client.PostAsJsonAsync("api/clients", new { name, useRawSql });

        public async Task UpdateClientAsync(int id, string name, bool useRawSql = false) =>
            await _client.PutAsJsonAsync($"api/clients/{id}", new { name, useRawSql });

        public async Task DeleteClientAsync(int id, bool useRawSql = false) =>
            await _client.DeleteAsync($"api/clients/{id}?useRawSql={useRawSql}");

        public async Task<List<ClientDto>> SearchClientsAsync(
        string? nameFilter,
        bool useRawSql = false)
        {
            var query = new List<string>();
            if (!string.IsNullOrWhiteSpace(nameFilter))
                query.Add($"nameFilter={Uri.EscapeDataString(nameFilter)}");
            query.Add($"useRawSql={useRawSql}");

            var url = "api/clients";
            if (query.Count > 0) url += "?" + string.Join('&', query);

            var result = await _client.GetFromJsonAsync<List<ClientDto>>(url);
            return result ?? new List<ClientDto>();
        }


        // Игры
        public async Task AddGameAsync(DateTime playedAt, string type, List<int> clientIds, bool useRawSql = false)
        {
            var response = await _client.PostAsJsonAsync("api/games", new { playedAt, type, clientIds, useRawSql });
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateGameAsync(int id, DateTime playedAt, string type, List<int> clientIds, bool useRawSql = false) =>
            await _client.PutAsJsonAsync($"api/games/{id}", new { playedAt, type, clientIds, useRawSql });

        public async Task DeleteGameAsync(int id, bool useRawSql = false) =>
            await _client.DeleteAsync($"api/games/{id}?useRawSql={useRawSql}");

        public async Task<List<GameDto>> SearchGamesAsync(
            DateTime? from,
            DateTime? to,
            string? typeFilter,
            int? clientId,
            bool useRawSql = false)
        {
            var queryParams = new List<string>();

            if (from.HasValue)
                queryParams.Add($"from={Uri.EscapeDataString(from.Value.ToString("O"))}");
            if (to.HasValue)
                queryParams.Add($"to={Uri.EscapeDataString(to.Value.ToString("O"))}");
            if (!string.IsNullOrWhiteSpace(typeFilter))
                queryParams.Add($"typeFilter={Uri.EscapeDataString(typeFilter)}");
            if (clientId.HasValue)
                queryParams.Add($"clientId={clientId.Value}");

            // Всегда передаём флаг useRawSql
            queryParams.Add($"useRawSql={useRawSql}");

            var url = "api/games";
            if (queryParams.Count > 0)
                url += "?" + string.Join('&', queryParams);

            var result = await _client.GetFromJsonAsync<List<GameDto>>(url);
            return result ?? new List<GameDto>();
        }
    }
}
