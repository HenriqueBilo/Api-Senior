using Api_Senior.Interfaces;
using Api_Senior.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_Senior.Data
{
    public class CidadeDataContext: DbContext, ICidadeDataContext
    {
        public DbSet<Cidade> Cidade { get; set; }

        public CidadeDataContext(DbContextOptions<CidadeDataContext> options) : base(options)
        {
        }

        public async Task AddCidadesAsync(List<Cidade> cidades)
        {
            await Cidade.AddRangeAsync(cidades);
            await SaveChangesAsync();
        }
    }
}
