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
        }


        public static void ConfigureUserSession(EntityTypeBuilder<UserSession> entity)
        {
            entity.ToTable("UserSession", "identity");
            entity.HasKey(e => e.UserSessionId);
            entity.HasOne(e=>e.User).WithMany(e=>e.UserSessions).HasForeignKey(e=>e.UserId);
        }

        /*public static void ConfigureApprovalCode(EntityTypeBuilder<ApprovalCode> entity)
        {
            entity.HasKey(e => e.AprrovalCodeId);
            entity.HasOne(e => e.User).WithMany(e => e.ApprovalCodes).HasForeignKey(e => e.UserId).IsRequired();
        }


        public static void ConfigureEmailUpdateEntity(EntityTypeBuilder<EmailUpdateEntity> entity)
        {
            entity.HasKey(e => e.EmailUpdateEntityId);
            entity.HasOne(e => e.User).WithMany(e => e.EmailUpdateEntities).HasForeignKey(e => e.UserId).IsRequired();
            entity.HasOne(e => e.Approval).WithOne(e => e.EmailUpdateEntity).IsRequired();
            entity.Property(e => e.OldEmail).IsRequired().HasMaxLength(255);
            entity.Property(e => e.NewEmail).IsRequired().HasMaxLength(255);
        }*/
    }
}
