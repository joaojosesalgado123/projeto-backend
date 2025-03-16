using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ES2Backend.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Membro> Membros { get; set; }

    public virtual DbSet<Projeto> Projetos { get; set; }

    public virtual DbSet<Tarefa> Tarefas { get; set; }

    public virtual DbSet<Utilizador> Utilizadores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=aws-0-eu-west-2.pooler.supabase.com;Port=6543;Database=sgsc;Username=postgres.xmluhjrjpzopiijmwjkz;Password=mNyQwUMNNAFhX5y;Timeout=60;CommandTimeout=60");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Membro>(entity =>
        {
            entity.HasKey(e => e.IdMembro).HasName("Membro_pkey");
            entity.ToTable("Membro");

            entity.Property(e => e.IdMembro).UseIdentityAlwaysColumn().HasColumnName("IdMembro");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.IdProjeto).HasColumnName("IdProjeto");
            entity.Property(e => e.IdUtilizador).HasColumnName("IdUtilizador");
            entity.Property(e => e.Nome).HasMaxLength(100).HasColumnName("nome");
            entity.Property(e => e.Password).HasMaxLength(100).HasColumnName("password");
            entity.Property(e => e.Username).HasMaxLength(100).HasColumnName("username");

            entity.HasOne(d => d.IdProjetoNavigation).WithMany(p => p.Membros).HasForeignKey(d => d.IdProjeto).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_idProjeto");
            entity.HasOne(d => d.IdUtilizadorNavigation).WithMany(p => p.Membros).HasForeignKey(d => d.IdUtilizador).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_idUtilizador");
        });

        modelBuilder.Entity<Projeto>(entity =>
        {
            entity.HasKey(e => e.IdProjeto).HasName("Projeto_pkey");
            entity.ToTable("Projeto");

            entity.Property(e => e.IdProjeto).UseIdentityAlwaysColumn().HasColumnName("IdProjeto");
            entity.Property(e => e.Descricao).HasMaxLength(300).HasColumnName("descricao");
            entity.Property(e => e.IdUtilizador).HasColumnName("IdUtilizador");
            entity.Property(e => e.Nome).HasMaxLength(100).HasColumnName("nome");
            entity.Property(e => e.NomeCliente).HasMaxLength(100).HasColumnName("nomeCliente");
            entity.Property(e => e.PrecoHora).HasPrecision(10, 2).HasColumnName("precoHora");

            entity.HasOne(d => d.IdUtilizadorNavigation).WithMany(p => p.Projetos).HasForeignKey(d => d.IdUtilizador).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_idUtilizador");

            entity.HasMany(d => d.IdTarefas).WithMany(p => p.IdProjetos)
                .UsingEntity<Dictionary<string, object>>(
                    "TarefaProjeto",
                    r => r.HasOne<Tarefa>().WithMany().HasForeignKey("IdTarefa").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_idTarefa"),
                    l => l.HasOne<Projeto>().WithMany().HasForeignKey("IdProjeto").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_idProjeto"),
                    j =>
                    {
                        j.HasKey("IdProjeto", "IdTarefa").HasName("TarefaProjeto_pkey");
                        j.ToTable("TarefaProjeto");
                        j.IndexerProperty<int>("IdProjeto").HasColumnName("IdProjeto");
                        j.IndexerProperty<int>("IdTarefa").HasColumnName("IdTarefa");
                    });
        });

        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.HasKey(e => e.IdTarefa).HasName("Tarefa_pkey");
            entity.ToTable("Tarefa");

            entity.Property(e => e.IdTarefa).UseIdentityAlwaysColumn().HasColumnName("IdTarefa");
            entity.Property(e => e.DataInicio).HasColumnName("dataInicio");
            entity.Property(e => e.Descricao).HasMaxLength(300).HasColumnName("descricao");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.PrecoHora).HasPrecision(10, 2).HasColumnName("precoHora");
        });

        modelBuilder.Entity<Utilizador>(entity =>
        {
            entity.HasKey(e => e.IdUtilizador).HasName("Utilizador_pkey");
            entity.ToTable("Utilizador");

            entity.Property(e => e.IdUtilizador).UseIdentityAlwaysColumn().HasColumnName("IdUtilizador");
            entity.Property(e => e.Nome).HasMaxLength(100).HasColumnName("nome");
            entity.Property(e => e.NumHoras).HasPrecision(10, 2).HasColumnName("numHoras");
            entity.Property(e => e.Password).HasMaxLength(100).HasColumnName("password");
            entity.Property(e => e.Username).HasMaxLength(100).HasColumnName("username");

            entity.HasMany(d => d.IdTarefas).WithMany(p => p.IdUtilizadors)
                .UsingEntity<Dictionary<string, object>>(
                    "TarefaUtilizador",
                    r => r.HasOne<Tarefa>().WithMany().HasForeignKey("IdTarefa").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_idTarefa"),
                    l => l.HasOne<Utilizador>().WithMany().HasForeignKey("IdUtilizador").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_idUtilizador"),
                    j =>
                    {
                        j.HasKey("IdUtilizador", "IdTarefa").HasName("TarefaUtilizador_pkey");
                        j.ToTable("TarefaUtilizador");
                        j.IndexerProperty<int>("IdUtilizador").HasColumnName("IdUtilizador");
                        j.IndexerProperty<int>("IdTarefa").HasColumnName("IdTarefa");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}