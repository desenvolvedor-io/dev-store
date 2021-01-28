using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NSE.WebApp.MVC.Models
{
    public class EnderecoViewModel
    {
        [Required]
        public string Logradouro { get; set; }
        [Required]
        [DisplayName("Número")]
        public string Numero { get; set; }
        public string Complemento { get; set; }
        [Required]
        public string Bairro { get; set; }
        [Required]
        [DisplayName("CEP")]
        public string Cep { get; set; }
        [Required]
        public string Cidade { get; set; }
        [Required]
        public string Estado { get; set; }

        public override string ToString()
        {
            return $"{Logradouro}, {Numero} {Complemento} - {Bairro} - {Cidade} - {Estado}";
        }
    }
}