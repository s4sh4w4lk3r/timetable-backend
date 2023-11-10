using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository
{
    internal static class AccountingSchemaMethods
    {
#warning указать названия схем.
        /*public static void ConfigureUser(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(u => u.UserId);
            entity.HasIndex(u => u.Email, "email-unique").IsUnique();
            entity.Property(u => u.Email).HasMaxLength(255).IsRequired();
            entity.Property(u => u.Password).HasMaxLength(72).IsRequired();
        }

        public static void ConfigureApprovalCode(EntityTypeBuilder<ApprovalCode> entity)
        {
            entity.HasKey(e => e.AprrovalCodeId);
            entity.HasOne(e => e.User).WithMany(e => e.ApprovalCodes).HasForeignKey(e => e.UserId).IsRequired();
        }

        public static void ConfigureUserSession(EntityTypeBuilder<UserSession> entity)
        {
            entity.HasKey(e => e.UserSessionId);
            entity.HasOne(e => e.User).WithMany(e => e.UserSessions).HasForeignKey(e => e.UserId).IsRequired();
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
