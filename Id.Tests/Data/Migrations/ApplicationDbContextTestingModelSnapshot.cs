﻿// <auto-generated />
using System;
using Id.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Id.Tests.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContextTesting))]
    partial class ApplicationDbContextTestingModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("Id.Models.DatabaseModels.Application", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ApplicationCookiesPolicy")
                        .HasColumnType("TEXT");

                    b.Property<string>("ApplicationEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("ApplicationPrivacyPolicy")
                        .HasColumnType("TEXT");

                    b.Property<string>("ApplicationTermsAndConditions")
                        .HasColumnType("TEXT");

                    b.Property<string>("ApplicationWebsite")
                        .HasColumnType("TEXT");

                    b.Property<int>("BrandId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClientSecret")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Logo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PostLogoutRedirectUri")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Published")
                        .HasColumnType("TEXT");

                    b.Property<string>("RedirectUri")
                        .HasColumnType("TEXT");

                    b.Property<bool>("RequireCookiePolicy")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("RequirePrivacyPolicy")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("RequireTerms")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationPolicy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApplicationId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalLanguage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PolicyType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("ApplicationPolicies");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AllignToAll")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApplicationId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("CanAdministerRoles")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("ApplicationRoles");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationSmtpSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApplicationId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("AuthorizationRequired")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<int>("Port")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Secure")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SenderEmail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SenderName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId")
                        .IsUnique();

                    b.ToTable("ApplicationSmtpSettings");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AgreedSharingUserDetails")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AgreedToCookies")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AgreedToTerms")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApplicationId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Locked")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LockedReason")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LockedUntil")
                        .HasColumnType("TEXT");

                    b.Property<bool>("ShowAddress")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ShowBirthday")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ShowEmail")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ShowGender")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ShowName")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ShowPhone")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ShowProfilePicture")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("UserId");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Logo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Website")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Dial_code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Emoji")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Native")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Rtl")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.PolicyTranslation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("LanguageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OldContent")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PolicyId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UnapprovedContent")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.HasIndex("PolicyId");

                    b.ToTable("PolicyTranslations");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedByIp")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReasonRevoked")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReplacedByToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("TEXT");

                    b.Property<string>("RevokedByIp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.SuspendedApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApplicationId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Reason")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("SuspendedFrom")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SuspendedUntil")
                        .HasColumnType("TEXT");

                    b.Property<string>("SuspenderId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("SuspensionActive")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("SuspenderId");

                    b.ToTable("SuspendedApplications");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.SuspendedUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApplicationId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ApplicationUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Reason")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("SuspendedFrom")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SuspendedUntil")
                        .HasColumnType("TEXT");

                    b.Property<string>("SuspenderId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("SuspensionActive")
                        .HasColumnType("INTEGER");

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
                        .HasColumnType("INTEGER");

                    b.Property<bool>("DefaultLanguageFound")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("OldTranslationsFound")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PhrazesCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PhrazesToRemoveCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PhrazesToTranslateCount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("TargetLanguagesCount")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("TotalTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("TranslatedPhrazesCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TranslationErrorCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("UiTranslatorLogs");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly?>("BirthDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool?>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MiddleName")
                        .HasColumnType("TEXT");

                    b.Property<string>("OTPToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("OTPTokenExpiration")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PasswordResetTokenExpiration")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.UserAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("CountryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PostCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StreetAddress2")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAddresses");
                });

            modelBuilder.Entity("Id.Models.DatabaseModels.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AddedToRole")
                        .HasColumnType("TEXT");

                    b.Property<int>("ApplicationRoleId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ApplicationUserId")
                        .HasColumnType("INTEGER");

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
