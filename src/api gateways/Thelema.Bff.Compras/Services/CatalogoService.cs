using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Thelema.Bff.Compras.Extensions;
using Thelema.Bff.Compras.Models;

namespace Thelema.Bff.Compras.Services
{
    public interface ICatalogoService
    {
        Task<ItemProdutoDTO> ObterPorId(Guid id);
        Task<IEnumerable<ItemProdutoDTO>> ObterItens(IEnumerable<Guid> ids);
    }

    public class CatalogoService : Service, ICatalogoService
    {
        private readonly HttpClient _httpClient;

        public CatalogoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
        }

        public async Task<ItemProdutoDTO> ObterPorId(Guid id)
        {
            var Response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");

            TratarErrosResponse(Response);

            return await DeserializarObjetoResponse<ItemProdutoDTO>(Response);
        }

        public async Task<IEnumerable<ItemProdutoDTO>> ObterItens(IEnumerable<Guid> ids)
        {
            var idsRequest = string.Join(",", ids);

            var Response = await _httpClient.GetAsync($"/catalogo/produtos/lista/{idsRequest}/");

            TratarErrosResponse(Response);

            return await DeserializarObjetoResponse<IEnumerable<ItemProdutoDTO>>(Response);
        }
    }
}