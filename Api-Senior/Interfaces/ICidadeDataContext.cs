using Api_Senior.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_Senior.Interfaces
{
    public interface ICidadeDataContext
    {
        DbSet<Cidade> Cidade { get; set; }
        Task AddCidadesAsync(List<Cidade> cidades);
    }
}
