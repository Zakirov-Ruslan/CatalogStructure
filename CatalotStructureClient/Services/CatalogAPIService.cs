using CatalogStructureDto;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace CatalogStructureClient.Services
{
    public class CatalogAPIService
    {
        private readonly HttpClient _httpClient;

        public CatalogAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatalogItemDto>> GetCatalogTreeAsync()
        {
            var getResponse = await _httpClient.GetAsync("api/CatalogItems");
            var rootCatalogItems = await getResponse.Content.ReadFromJsonAsync<List<CatalogItemDto>>();

            return rootCatalogItems;
        }

        public async Task<CatalogItemDto> GetCatalogByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CatalogItemDto>($"api/CatalogItems/{id}");
        }

        public async Task<CatalogItemDto?> CreateCatalogAsync(CatalogItemCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/CatalogItems", dto);

            if (response.StatusCode != System.Net.HttpStatusCode.Created)
                throw new Exception(await response.Content.ReadAsStringAsync());

            return await response.Content.ReadFromJsonAsync<CatalogItemDto>();
        }

        public async Task UpdateCatalogAsync(int id, CatalogItemUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/CatalogItems/{id}", dto);

            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task DeleteCatalogAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/CatalogItems/{id}");

            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }
}
