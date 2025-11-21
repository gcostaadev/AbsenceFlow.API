using AbsenceFlow.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AbsenceFlow.API.Persistence
{
    public class AbsenceFlowDbContext : DbContext
    {
        public AbsenceFlowDbContext(DbContextOptions<AbsenceFlowDbContext> options)
            : base(options)
        {

        }

        
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Solicitacao> Solicitacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            builder
                .Entity<Colaborador>(e =>
                {
                    e.HasKey(c => c.Id); 

                    
                    e.HasIndex(c => c.EmailCorporativo).IsUnique();

                    
                    e.HasQueryFilter(c => !c.IsDeleted);
                });

            
            builder
                .Entity<Solicitacao>(e =>
                {
                    e.HasKey(s => s.Id); 

                   
                    e.HasOne(s => s.Colaborador)
                     .WithMany(c => c.Solicitacoes)
                     .HasForeignKey(s => s.IdColaborador)
                     .OnDelete(DeleteBehavior.Restrict); 
                                       
                    e.Property(s => s.Tipo)
                     .HasConversion<string>();

                    e.Property(s => s.Status)
                     .HasConversion<string>();

                    
                    e.HasQueryFilter(s => !s.IsDeleted);
                });

            base.OnModelCreating(builder);
        }
    }
}
