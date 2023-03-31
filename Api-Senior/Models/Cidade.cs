using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api_Senior.Models
{
    public class Cidade
    {
        [Key]
        [JsonIgnore]
        public string IdCidade { get; set; }
        [JsonPropertyName("Temperatura (ºC)")]
        public double Temperatura { get; set; }
        [JsonPropertyName("Nome Cidade")]
        public string Nome { get; set; }
        [JsonIgnore]
        public DateTime Data { get; set; }
        [JsonPropertyName("Data da coleta")]
        public string DataFormatada { get; set; }

    }
}
