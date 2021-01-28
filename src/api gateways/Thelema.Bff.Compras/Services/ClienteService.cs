using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Thelema.Bff.Compras.Extensions;
using Thelema.Bff.Compras.Models;

namespace Thelema.Bff.Compras.Services
{
    public interface IClienteService
    {
        Task<EnderecoDTO> ObterEndereco();
    }

    public class ClienteService : Service, IClienteService
    {
        private readonly HttpClient _httpClient;

        public ClienteService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ClienteUrl);
        }

        public async Task<EnderecoDTO> ObterEndereco()
        {
            var Response = await _httpClient.GetAsync("/cliente/endereco/");

            if (Response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(Response);

            return await DeserializarObjetoResponse<EnderecoDTO>(Response);
        }
    }
}