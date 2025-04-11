﻿// <auto-generated />
using System;
using Id.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Id.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250411112435_TranslationTablesAdded")]
    partial class TranslationTablesAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Id.Models.DatabaseModels.Application", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApplicationCookiesPolicy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplicationEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplicationPrivacyPolicy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplicationTermsAndConditions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplicationWebsite")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<string>("ClientSecret")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("Logo")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PostLogoutRedirectUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Published")
                        .HasColumnType("datetime2");

                    b.Property<string>("RedirectUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RequireCookiePolicy")
                        .HasColumnType("bit");

                    b.Property<bool>("RequirePrivacyPolicy")
                        .HasColumnType("bit");

                    b.Property<bool>("RequireTerms")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationDescriptionTranslation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AutoTranslated")
                        .HasColumnType("bit");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ChangesAccepted")
                        .HasColumnType("bit");

                    b.Property<string>("EditorId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsOriginal")
                        .HasColumnType("bit");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<DateTime>("LastChange")
                        .HasColumnType("datetime2");

                    b.Property<string>("ObjectId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousBody")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApplicationDescriptionTranslations");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationPolicy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("OriginalLanguage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PolicyType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("ApplicationPolicies");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AllignToAll")
                        .HasColumnType("bit");

                    b.Property<string>("ApplicationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("CanAdministerRoles")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("ApplicationRoles");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationSmtpSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("AuthorizationRequired")
                        .HasColumnType("bit");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.Property<int>("Secure")
                        .HasColumnType("int");

                    b.Property<string>("SenderEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId")
                        .IsUnique();

                    b.ToTable("ApplicationSmtpSettings");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AgreedSharingUserDetails")
                        .HasColumnType("bit");

                    b.Property<bool>("AgreedToCookies")
                        .HasColumnType("bit");

                    b.Property<bool>("AgreedToTerms")
                        .HasColumnType("bit");

                    b.Property<string>("ApplicationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<bool>("Locked")
                        .HasColumnType("bit");

                    b.Property<string>("LockedReason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LockedUntil")
                        .HasColumnType("datetime2");

                    b.Property<bool>("ShowAddress")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowBirthday")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowEmail")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowGender")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowName")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowPhone")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowProfilePicture")
                        .HasColumnType("bit");

                    b.Property<string>("UserDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("UserId");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationUserDescriptionTranslation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AutoTranslated")
                        .HasColumnType("bit");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ChangesAccepted")
                        .HasColumnType("bit");

                    b.Property<string>("EditorId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsOriginal")
                        .HasColumnType("bit");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<DateTime>("LastChange")
                        .HasColumnType("datetime2");

                    b.Property<string>("ObjectId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousBody")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUserDescriptionTranslations");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Logo")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.BrandDescriptionTranslation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AutoTranslated")
                        .HasColumnType("bit");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ChangesAccepted")
                        .HasColumnType("bit");

                    b.Property<string>("EditorId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsOriginal")
                        .HasColumnType("bit");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<DateTime>("LastChange")
                        .HasColumnType("datetime2");

                    b.Property<string>("ObjectId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreviousBody")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BrandDescriptionTranslations");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Dial_code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Emoji")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Native")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rtl")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.PolicyTranslation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LanguageId")
                        .HasColumnType("int");

                    b.Property<string>("OldContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PolicyId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UnapprovedContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.HasIndex("PolicyId");

                    b.ToTable("PolicyTranslations");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedByIp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReasonRevoked")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("datetime2");

                    b.Property<string>("RevokedByIp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.SuspendedApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Reason")
                        .HasColumnType("int");

                    b.Property<DateTime>("SuspendedFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SuspendedUntil")
                        .HasColumnType("datetime2");

                    b.Property<string>("SuspenderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("SuspensionActive")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("SuspenderId");

                    b.ToTable("SuspendedApplications");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.SuspendedUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ApplicationUserId")
                        .HasColumnType("int");

                    b.Property<int>("Reason")
                        .HasColumnType("int");

                    b.Property<DateTime>("SuspendedFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SuspendedUntil")
                        .HasColumnType("datetime2");

                    b.Property<string>("SuspenderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("SuspensionActive")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("SuspenderId");

                    b.ToTable("SuspendedUsers");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.UiTranslatorLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("DefaultLanguageFound")
                        .HasColumnType("bit");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("OldTranslationsFound")
                        .HasColumnType("bit");

                    b.Property<int>("PhrazesCount")
                        .HasColumnType("int");

                    b.Property<int>("PhrazesToRemoveCount")
                        .HasColumnType("int");

                    b.Property<int>("PhrazesToTranslateCount")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TargetLanguagesCount")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("TotalTime")
                        .HasColumnType("time");

                    b.Property<int>("TranslatedPhrazesCount")
                        .HasColumnType("int");

                    b.Property<int>("TranslationErrorCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UiTranslatorLogs");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("BirthDate")
                        .HasColumnType("date");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OTPToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("OTPTokenExpiration")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PasswordResetTokenExpiration")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.UserAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("PostCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAddress2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAddresses");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddedToRole")
                        .HasColumnType("datetime2");

                    b.Property<int>("ApplicationRoleId")
                        .HasColumnType("int");

                    b.Property<int>("ApplicationUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationRoleId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.Application", b =>
                {
                    b.HasOne("Id.Models.DatabaseModels.Brand", "Brand")
                        .WithMany("Applications")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Id.Models.DatabaseModels.User", "Owner")
                        .WithMany("OwnedApplications")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationPolicy", b =>
                {
                    b.HasOne("Id.Models.DatabaseModels.Application", "Application")
                        .WithMany("Policies")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationRole", b =>
                {
                    b.HasOne("Id.Models.DatabaseModels.Application", "Application")
                        .WithMany("Roles")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationSmtpSettings", b =>
                {
                    b.HasOne("Id.Models.DatabaseModels.Application", "Application")
                        .WithOne("SmtpSettings")
                        .HasForeignKey("Id.Models.DatabaseModels.ApplicationSmtpSettings", "ApplicationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationUser", b =>
                {
                    b.HasOne("Id.Models.DatabaseModels.Application", "Application")
                        .WithMany("Users")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Id.Models.DatabaseModels.User", "User")
                        .WithMany("ApplicationUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Application");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.Brand", b =>
                {
                    b.HasOne("Id.Models.DatabaseModels.User", "Owner")
                        .WithMany("Brands")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.PolicyTranslation", b =>
                {
                    b.HasOne("Id.Models.DatabaseModels.Language", "Language")
                        .WithMany("PolicyTranslations")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Id.Models.DatabaseModels.ApplicationPolicy", "Policy")
                        .WithMany("Translations")
                        .HasForeignKey("PolicyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("Policy");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.RefreshToken", b =>
                {
                    b.HasOne("Id.Models.DatabaseModels.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.SuspendedApplication", b =>
                {
                    b.HasOne("Id.Models.DatabaseModels.Application", "Application")
                        .WithMany("SuspendedApplications")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Id.Models.DatabaseModels.User", "Suspender")
                        .WithMany("SuspendedApplications")
                        .HasForeignKey("SuspenderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Application");

                    b.Navigation("Suspender");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.SuspendedUser", b =>
                {
                    b.HasOne("Id.Models.DatabaseModels.Application", "Application")
                        .WithMany("SuspendedUsers")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Id.Models.DatabaseModels.ApplicationUser", "Suspended")
                        .WithMany("SuspendedUsers")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Id.Models.DatabaseModels.User", "Suspender")
                        .WithMany("Suspenders")
                        .HasForeignKey("SuspenderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Application");

                    b.Navigation("Suspended");

                    b.Navigation("Suspender");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.UserAddress", b =>
                {
                    b.HasOne("Id.Models.DatabaseModels.Country", "Country")
                        .WithMany("Addresses")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Id.Models.DatabaseModels.User", "User")
                        .WithMany("UserAddresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.UserRole", b =>
                {
                    b.HasOne("Id.Models.DatabaseModels.ApplicationRole", "ApplicationRole")
                        .WithMany("Roles")
                        .HasForeignKey("ApplicationRoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Id.Models.DatabaseModels.ApplicationUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ApplicationRole");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.Application", b =>
                {
                    b.Navigation("Policies");

                    b.Navigation("Roles");

                    b.Navigation("SmtpSettings");

                    b.Navigation("SuspendedApplications");

                    b.Navigation("SuspendedUsers");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationPolicy", b =>
                {
                    b.Navigation("Translations");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationRole", b =>
                {
                    b.Navigation("Roles");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationUser", b =>
                {
                    b.Navigation("SuspendedUsers");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.Brand", b =>
                {
                    b.Navigation("Applications");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.Country", b =>
                {
                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.Language", b =>
                {
                    b.Navigation("PolicyTranslations");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.User", b =>
                {
                    b.Navigation("ApplicationUsers");

                    b.Navigation("Brands");

                    b.Navigation("OwnedApplications");

                    b.Navigation("RefreshTokens");

                    b.Navigation("SuspendedApplications");

                    b.Navigation("Suspenders");

                    b.Navigation("UserAddresses");
                });
#pragma warning restore 612, 618
        }
    }
}
