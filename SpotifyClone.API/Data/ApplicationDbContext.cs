﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpotifyClone.API.Models.Entities;

namespace SpotifyClone.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<ListeningHistory> ListeningHistories { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PlaylistSong>()
                .HasKey(ps => new { ps.PlaylistId, ps.SongId });

            builder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.PlaylistId);

            builder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Song)
                .WithMany()
                .HasForeignKey(ps => ps.SongId);

            builder.Entity<ListeningHistory>()
                .HasOne(l => l.User)
                .WithMany(u => u.ListeningHistories)
                .HasForeignKey(l => l.UserId);

            builder.Entity<ListeningHistory>()
                .HasOne(l => l.Song)
                .WithMany()
                .HasForeignKey(l => l.SongId);

            builder.Entity<Like>()
                .HasIndex(l => new { l.UserId, l.SongId })
                .IsUnique();

            builder.Entity<Album>()
                .HasMany(a => a.Songs)
                .WithOne(s => s.Album)
                .HasForeignKey(s => s.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<Genre>()
                .HasMany(g => g.Songs)
                .WithOne(s => s.Genre)
                .HasForeignKey(s => s.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Genre>()
                .HasMany(g => g.Albums)
                .WithOne(a => a.Genre)
                .HasForeignKey(a => a.GenreId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
