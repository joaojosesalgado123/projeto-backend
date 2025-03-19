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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=aws-0-eu-west-2.pooler.supabase.com;Port=6543;Database=ES2;Username=postgres.xmluhjrjpzopiijmwjkz;Password=mNyQwUMNNAFhX5y");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Membro>(entity =>
        {
            entity.HasKey(e => e.IdMembro).HasName("Membro_pkey");

            entity.ToTable("Membro");

            entity.Property(e => e.IdMembro).HasColumnName("idMembro");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.IdProjeto)
                .ValueGeneratedOnAdd()
                .HasColumnName("idProjeto");
            entity.Property(e => e.IdUtilizador)
                .ValueGeneratedOnAdd()
                .HasColumnName("idUtilizador");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.IdProjetoNavigation).WithMany(p => p.Membros)
                .HasForeignKey(d => d.IdProjeto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_idProjeto");

            entity.HasOne(d => d.IdUtilizadorNavigation).WithMany(p => p.Membros)
                .HasForeignKey(d => d.IdUtilizador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_idUtilizador");
        });

        modelBuilder.Entity<Projeto>(entity =>
        {
            entity.HasKey(e => e.IdProjeto).HasName("Projeto_pkey");

            entity.ToTable("Projeto");

            entity.Property(e => e.IdProjeto).HasColumnName("idProjeto");
            entity.Property(e => e.Descricao)
                .HasMaxLength(300)
                .HasColumnName("descricao");
            entity.Property(e => e.IdUtilizador)
                .ValueGeneratedOnAdd()
                .HasColumnName("idUtilizador");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.NomeCliente)
                .HasMaxLength(100)
                .HasColumnName("nomeCliente");
            entity.Property(e => e.PrecoHora)
                .HasPrecision(10, 2)
                .HasColumnName("precoHora");

            entity.HasOne(d => d.IdUtilizadorNavigation).WithMany(p => p.Projetos)
                .HasForeignKey(d => d.IdUtilizador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_idUtilizador");
        });

        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.HasKey(e => e.IdTarefa).HasName("Tarefa_pkey");

            entity.ToTable("Tarefa");

            entity.Property(e => e.IdTarefa).HasColumnName("idTarefa");
            entity.Property(e => e.DataInicio).HasColumnName("dataInicio");
            entity.Property(e => e.Descricao)
                .HasMaxLength(300)
                .HasColumnName("descricao");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.PrecoHora)
                .HasPrecision(10, 2)
                .HasColumnName("precoHora");

            entity.HasMany(d => d.IdProjetos).WithMany(p => p.IdTarefas)
                .UsingEntity<Dictionary<string, object>>(
                    "TarefaProjeto",
                    r => r.HasOne<Projeto>().WithMany()
                        .HasForeignKey("IdProjeto")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_idProjeto"),
                    l => l.HasOne<Tarefa>().WithMany()
                        .HasForeignKey("IdTarefa")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_idTarefa"),
                    j =>
                    {
                        j.HasKey("IdTarefa", "IdProjeto").HasName("TarefaProjeto_pkey");
                        j.ToTable("TarefaProjeto");
                        j.IndexerProperty<int>("IdTarefa")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("idTarefa");
                        j.IndexerProperty<int>("IdProjeto")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("idProjeto");
                    });

            entity.HasMany(d => d.IdUtilizadors).WithMany(p => p.IdTarefas)
                .UsingEntity<Dictionary<string, object>>(
                    "TarefaUtilizador",
                    r => r.HasOne<Utilizador>().WithMany()
                        .HasForeignKey("IdUtilizador")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_idUtilizador"),
                    l => l.HasOne<Tarefa>().WithMany()
                        .HasForeignKey("IdTarefa")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_idTarefa"),
                    j =>
                    {
                        j.HasKey("IdTarefa", "IdUtilizador").HasName("TarefaUtilizador_pkey");
                        j.ToTable("TarefaUtilizador");
                        j.IndexerProperty<int>("IdTarefa")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("idTarefa");
                        j.IndexerProperty<int>("IdUtilizador")
                            .ValueGeneratedOnAdd()
                            .HasColumnName("idUtilizador");
                    });
        });

        modelBuilder.Entity<Utilizador>(entity =>
        {
            entity.HasKey(e => e.IdUtilizador).HasName("Utilizador_pkey");

            entity.ToTable("Utilizador");

            entity.Property(e => e.IdUtilizador).HasColumnName("idUtilizador");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.NumHoras)
                .HasPrecision(10, 2)
                .HasColumnName("numHoras");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
