using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities.Identity;
using Models.Entities.Identity.Users;

namespace Repository
{
    internal static class IdentitySchemaMethods
    {
        public static void ConfigureUser(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("User", "identity");
            entity.HasKey(u => u.UserId);
            entity.UseTphMappingStrategy();
            entity.HasIndex(e=>e.Email).IsUnique();
        }

        public static void ConfigureAdmin(EntityTypeBuilder<Admin> entity)
        {
            entity.HasBaseType<User>();
        }

        public static void ConfigureTeacher(EntityTypeBuilder<Teacher> entity)
        {
            entity.HasBaseType<User>();
        }

        public static void ConfigureStudent(EntityTypeBuilder<Student> entity)
        {
            entity.HasBaseType<User>();
            entity.HasOne(e => e.Group).WithMany(e => e.Students).HasForeignKey(e => e.GroupId);
            entity.Property(e => e.GroupId).HasColumnName("StudentGroupId");
        }

        public static void ConfigureUserSession(EntityTypeBuilder<UserSession> entity)
        {
            entity.ToTable("UserSession", "identity");
            entity.HasKey(e => e.UserSessionId);
            entity.HasOne(e => e.User).WithMany(e => e.UserSessions).HasForeignKey(e => e.UserId).IsRequired();
        }

        public static void ConfigureApproval(EntityTypeBuilder<Approval> entity)
        {
            entity.ToTable("Approval", "identity");
            entity.HasKey(e => e.AprrovalId);
            entity.HasOne(e => e.User).WithMany(e => e.Approvals).HasForeignKey(e => e.UserId).IsRequired();
        }

        public static void ConfigureEmailUpdateEntity(EntityTypeBuilder<EmailUpdateEntity> entity)
        {
            entity.ToTable("EmailUpdateEntity", "identity");
            entity.HasKey(e => e.EmailUpdateEntityId);
            entity.HasOne(e => e.User).WithMany(e => e.EmailUpdateEntities).HasForeignKey(e => e.UserId).IsRequired();
            entity.HasOne(e => e.Approval).WithOne(e => e.EmailUpdateEntity).IsRequired();
            entity.Property(e => e.OldEmail).IsRequired();
            entity.Property(e => e.NewEmail).IsRequired();
        }

        public static void ConfigureRegistrationEntity(EntityTypeBuilder<RegistrationEntity> entity)
        {
            entity.ToTable("RegistrationEntity", "identity");
            entity.HasKey(e => e.RegistrationEntityId);
            entity.Property(e => e.SecretKey).IsRequired();
        }
    }
}
