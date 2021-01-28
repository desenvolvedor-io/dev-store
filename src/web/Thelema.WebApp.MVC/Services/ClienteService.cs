using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Thelema.Core.Communication;
using Thelema.WebApp.MVC.Extensions;
using Thelema.WebApp.MVC.Models;

namespace Thelema.WebApp.MVC.Services
{
    public interface IClienteService
    {
        Task<EnderecoViewModel> ObterEndereco();
        Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco);
    }

    public class ClienteService : Service, IClienteService
    {
        private readonly HttpClient _httpClient;

        public ClienteService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ClienteUrl);
        }

        public async Task<EnderecoViewModel> ObterEndereco()
        {
            var Response = await _httpClient.GetAsync("/cliente/endereco/");

            if (Response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(Response);

            return await DeserializarObjetoResponse<EnderecoViewModel>(Response);
        }

        public async Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco)
        {
            var enderecoContent = ObterConteudo(endereco);

            var Response = await _httpClient.PostAsync("/cliente/endereco/", enderecoContent);

            if (!TratarErrosResponse(Response)) return await DeserializarObjetoResponse<ResponseResult>(Response);

            return RetornoOk();
        }
    }
}