using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace TourAssist.Model.Scaffold;

public partial class TourismDbContext : DbContext
{
    public TourismDbContext()
    {
    }

    public TourismDbContext(DbContextOptions<TourismDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attraction> Attractions { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Entry> Entries { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<PecularitiesCountry> PecularitiesCountries { get; set; }

    public virtual DbSet<PecularitiesRegion> PecularitiesRegions { get; set; }

    public virtual DbSet<Peculiarity> Peculiarities { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Transport> Transports { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    public virtual DbSet<Usertour> Usertours { get; set; }

    public virtual DbSet<UsertourAttraction> UsertourAttractions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=" + Configuration.MySQLServerIP +
            ";user=" + Configuration.MySQLUser +
            ";password=" + Configuration.MySQLPassword + ";database=tourism_db",
            ServerVersion.Parse("8.0.36-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Attraction>(entity =>
        {
            entity.HasKey(e => e.IdAttraction).HasName("PRIMARY");

            entity.ToTable("attraction");

            entity.HasIndex(e => e.CityIdCity, "fk_Attraction_City1_idx");

            entity.Property(e => e.IdAttraction).HasColumnName("idAttraction");
            entity.Property(e => e.CityIdCity).HasColumnName("City_idCity");
            entity.Property(e => e.FullName).HasMaxLength(45);

            entity.HasOne(d => d.CityIdCityNavigation).WithMany(p => p.Attractions)
                .HasForeignKey(d => d.CityIdCity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Attraction_City1");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.IdCity).HasName("PRIMARY");

            entity.ToTable("city");

            entity.HasIndex(e => e.RegionIdRegion, "fk_City_Region1_idx");

            entity.Property(e => e.IdCity)
                .ValueGeneratedNever()
                .HasColumnName("idCity");
            entity.Property(e => e.FullName).HasMaxLength(45);
            entity.Property(e => e.RegionIdRegion).HasColumnName("Region_idRegion");

            entity.HasOne(d => d.RegionIdRegionNavigation).WithMany(p => p.Cities)
                .HasForeignKey(d => d.RegionIdRegion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_City_Region1");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Iso31661).HasName("PRIMARY");

            entity.ToTable("country");

            entity.Property(e => e.Iso31661)
                .HasMaxLength(2)
                .HasColumnName("ISO3166-1");
            entity.Property(e => e.FullName).HasMaxLength(45);
        });

        modelBuilder.Entity<Entry>(entity =>
        {
            entity.HasKey(e => e.IdEntry).HasName("PRIMARY");

            entity.ToTable("entry");

            entity.HasIndex(e => e.RouteIdRoute, "fk_Entry_Route1_idx");

            entity.HasIndex(e => e.UserIdUser, "fk_Entry_User1_idx");

            entity.Property(e => e.IdEntry)
                .ValueGeneratedNever()
                .HasColumnName("idEntry");
            entity.Property(e => e.RouteIdRoute).HasColumnName("Route_idRoute");
            entity.Property(e => e.UserIdUser).HasColumnName("User_idUser");

            entity.HasOne(d => d.RouteIdRouteNavigation).WithMany(p => p.Entries)
                .HasForeignKey(d => d.RouteIdRoute)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Entry_Route1");

            entity.HasOne(d => d.UserIdUserNavigation).WithMany(p => p.Entries)
                .HasForeignKey(d => d.UserIdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Entry_User1");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.IdHotel).HasName("PRIMARY");

            entity.ToTable("hotel");

            entity.HasIndex(e => e.CityIdCity, "fk_Hotel_City1_idx");

            entity.Property(e => e.IdHotel)
                .ValueGeneratedNever()
                .HasColumnName("idHotel");
            entity.Property(e => e.CityIdCity).HasColumnName("City_idCity");
            entity.Property(e => e.FullName).HasMaxLength(45);

            entity.HasOne(d => d.CityIdCityNavigation).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.CityIdCity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Hotel_City1");
        });

        modelBuilder.Entity<PecularitiesCountry>(entity =>
        {
            entity.HasKey(e => e.IdCountryPecularity).HasName("PRIMARY");

            entity.ToTable("pecularities_countries");

            entity.HasIndex(e => e.CountryIso31661, "fk_Pecularities_Countries_Country1_idx");

            entity.HasIndex(e => e.PeculiarityIdPeculiarity, "fk_Pecularities_Countries_Peculiarity1_idx");

            entity.Property(e => e.IdCountryPecularity).HasColumnName("idCountryPecularity");
            entity.Property(e => e.CountryIso31661)
                .HasMaxLength(2)
                .HasColumnName("Country_ISO3166-1");
            entity.Property(e => e.PeculiarityIdPeculiarity).HasColumnName("Peculiarity_idPeculiarity");

            entity.HasOne(d => d.CountryIso31661Navigation).WithMany(p => p.PecularitiesCountries)
                .HasForeignKey(d => d.CountryIso31661)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pecularities_Countries_Country1");

            entity.HasOne(d => d.PeculiarityIdPeculiarityNavigation).WithMany(p => p.PecularitiesCountries)
                .HasForeignKey(d => d.PeculiarityIdPeculiarity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pecularities_Countries_Peculiarity1");
        });

        modelBuilder.Entity<PecularitiesRegion>(entity =>
        {
            entity.HasKey(e => e.IdRegionPecularity).HasName("PRIMARY");

            entity.ToTable("pecularities_regions");

            entity.HasIndex(e => e.PeculiarityIdPeculiarity, "fk_Pecularities_Regions_Peculiarity1_idx");

            entity.HasIndex(e => e.RegionIdRegion, "fk_Pecularities_Regions_Region1_idx");

            entity.Property(e => e.IdRegionPecularity).HasColumnName("idRegionPecularity");
            entity.Property(e => e.PeculiarityIdPeculiarity).HasColumnName("Peculiarity_idPeculiarity");
            entity.Property(e => e.RegionIdRegion).HasColumnName("Region_idRegion");

            entity.HasOne(d => d.PeculiarityIdPeculiarityNavigation).WithMany(p => p.PecularitiesRegions)
                .HasForeignKey(d => d.PeculiarityIdPeculiarity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pecularities_Regions_Peculiarity1");

            entity.HasOne(d => d.RegionIdRegionNavigation).WithMany(p => p.PecularitiesRegions)
                .HasForeignKey(d => d.RegionIdRegion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pecularities_Regions_Region1");
        });

        modelBuilder.Entity<Peculiarity>(entity =>
        {
            entity.HasKey(e => e.IdPeculiarity).HasName("PRIMARY");

            entity.ToTable("peculiarity");

            entity.Property(e => e.IdPeculiarity)
                .ValueGeneratedNever()
                .HasColumnName("idPeculiarity");
            entity.Property(e => e.Decription).HasColumnType("text");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.IdRegion).HasName("PRIMARY");

            entity.ToTable("region");

            entity.HasIndex(e => e.CountryIso31661, "fk_Region_Country_idx");

            entity.Property(e => e.IdRegion).HasColumnName("idRegion");
            entity.Property(e => e.CountryIso31661)
                .HasMaxLength(2)
                .HasColumnName("Country_ISO3166-1");
            entity.Property(e => e.FullName).HasMaxLength(45);

            entity.HasOne(d => d.CountryIso31661Navigation).WithMany(p => p.Regions)
                .HasForeignKey(d => d.CountryIso31661)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Region_Country");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.IdRoute).HasName("PRIMARY");

            entity.ToTable("route");

            entity.HasIndex(e => e.FromIdCity, "fk_Route_City1_idx");

            entity.HasIndex(e => e.ToIdCity, "fk_Route_City2_idx");

            entity.HasIndex(e => e.TransportIdTransport, "fk_Route_Transport1_idx");

            entity.Property(e => e.IdRoute)
                .ValueGeneratedNever()
                .HasColumnName("idRoute");
            entity.Property(e => e.Arrival).HasColumnType("datetime");
            entity.Property(e => e.Departure).HasColumnType("datetime");
            entity.Property(e => e.FromIdCity).HasColumnName("From_idCity");
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.Property(e => e.ToIdCity).HasColumnName("To_idCity");
            entity.Property(e => e.TransportIdTransport).HasColumnName("Transport_idTransport");

            entity.HasOne(d => d.FromIdCityNavigation).WithMany(p => p.RouteFromIdCityNavigations)
                .HasForeignKey(d => d.FromIdCity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Route_City1");

            entity.HasOne(d => d.ToIdCityNavigation).WithMany(p => p.RouteToIdCityNavigations)
                .HasForeignKey(d => d.ToIdCity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Route_City2");

            entity.HasOne(d => d.TransportIdTransportNavigation).WithMany(p => p.Routes)
                .HasForeignKey(d => d.TransportIdTransport)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Route_Transport1");
        });

        modelBuilder.Entity<Transport>(entity =>
        {
            entity.HasKey(e => e.IdTransport).HasName("PRIMARY");

            entity.ToTable("transport");

            entity.Property(e => e.IdTransport)
                .ValueGeneratedNever()
                .HasColumnName("idTransport");
            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.UserRoleIdUserRole, "fk_User_UserRole1_idx");

            entity.Property(e => e.IdUser)
                .ValueGeneratedNever()
                .HasColumnName("idUser");
            entity.Property(e => e.Login).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.PasswordSha256)
                .HasMaxLength(32)
                .HasColumnName("PasswordSHA256");
            entity.Property(e => e.Patronymic).HasMaxLength(45);
            entity.Property(e => e.Surname).HasMaxLength(45);
            entity.Property(e => e.UserRoleIdUserRole).HasColumnName("UserRole_idUserRole");

            entity.HasOne(d => d.UserRoleIdUserRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleIdUserRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_User_UserRole1");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => e.IdUserRole).HasName("PRIMARY");

            entity.ToTable("userrole");

            entity.Property(e => e.IdUserRole)
                .ValueGeneratedNever()
                .HasColumnName("idUserRole");
            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<Usertour>(entity =>
        {
            entity.HasKey(e => e.IdUserTour).HasName("PRIMARY");

            entity.ToTable("usertour");

            entity.HasIndex(e => e.ArrivalIdEntry, "fk_UserTour_Entry1_idx");

            entity.HasIndex(e => e.DepartureIdEntry, "fk_UserTour_Entry2_idx");

            entity.HasIndex(e => e.HotelIdHotel, "fk_UserTour_Hotel1_idx");

            entity.Property(e => e.IdUserTour)
                .ValueGeneratedNever()
                .HasColumnName("idUserTour");
            entity.Property(e => e.ArrivalIdEntry).HasColumnName("Arrival_idEntry");
            entity.Property(e => e.DepartureIdEntry).HasColumnName("Departure_idEntry");
            entity.Property(e => e.HotelIdHotel).HasColumnName("Hotel_idHotel");

            entity.HasOne(d => d.ArrivalIdEntryNavigation).WithMany(p => p.UsertourArrivalIdEntryNavigations)
                .HasForeignKey(d => d.ArrivalIdEntry)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_UserTour_Entry1");

            entity.HasOne(d => d.DepartureIdEntryNavigation).WithMany(p => p.UsertourDepartureIdEntryNavigations)
                .HasForeignKey(d => d.DepartureIdEntry)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_UserTour_Entry2");

            entity.HasOne(d => d.HotelIdHotelNavigation).WithMany(p => p.Usertours)
                .HasForeignKey(d => d.HotelIdHotel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_UserTour_Hotel1");
        });

        modelBuilder.Entity<UsertourAttraction>(entity =>
        {
            entity.HasKey(e => e.IdUserTourAttraction).HasName("PRIMARY");

            entity.ToTable("usertour_attraction");

            entity.HasIndex(e => e.AttractionIdAttraction, "fk_UserTour_Attraction_Attraction1_idx");

            entity.HasIndex(e => e.UserTourIdUserTour, "fk_UserTour_Attraction_UserTour1_idx");

            entity.Property(e => e.IdUserTourAttraction)
                .ValueGeneratedNever()
                .HasColumnName("idUserTour_Attraction");
            entity.Property(e => e.AttractionIdAttraction).HasColumnName("Attraction_idAttraction");
            entity.Property(e => e.UserTourIdUserTour).HasColumnName("UserTour_idUserTour");

            entity.HasOne(d => d.AttractionIdAttractionNavigation).WithMany(p => p.UsertourAttractions)
                .HasForeignKey(d => d.AttractionIdAttraction)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_UserTour_Attraction_Attraction1");

            entity.HasOne(d => d.UserTourIdUserTourNavigation).WithMany(p => p.UsertourAttractions)
                .HasForeignKey(d => d.UserTourIdUserTour)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_UserTour_Attraction_UserTour1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
