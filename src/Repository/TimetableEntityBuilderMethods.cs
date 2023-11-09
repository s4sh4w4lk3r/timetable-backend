using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using Models.Entities.Timetables.Cells.CellMembers;

namespace Repository
{
    internal static class TimetableEntityBuilderMethods
    {
        public static void ConfigureLessonTime(EntityTypeBuilder<LessonTime> entity)
        {
            entity.HasKey(e => e.LessonTimeId).HasName("LessonTimePRIMARY");
        }

        public static void ConfigureActualTimetableCell(EntityTypeBuilder<ActualTimetableCell> entity)
        {
            entity.HasBaseType<ITimetableCell>();
            entity.HasOne(e => e.Cabinet).WithMany(e => e.ActualTimetableCells);
            entity.HasOne(e => e.Subject).WithMany(e => e.ActualTimetableCells);
            entity.HasOne(e => e.Teacher).WithMany(e => e.ActualTimetableCells);
            entity.HasOne(e => e.LessonTime).WithMany(e => e.ActualTimetableCells);
        }

        public static void ConfigureStableTimetableCell(EntityTypeBuilder<StableTimetableCell> entity)
        {
            entity.HasBaseType<ITimetableCell>();
            entity.HasOne(e => e.Cabinet).WithMany(e => e.StableTimetableCells);
            entity.HasOne(e => e.Subject).WithMany(e => e.StableTimetableCells);
            entity.HasOne(e => e.Teacher).WithMany(e => e.StableTimetableCells);
            entity.HasOne(e => e.LessonTime).WithMany(e => e.StableTimetableCells);
        }

        public static void ConfigureTimetableCell(EntityTypeBuilder<ITimetableCell> entity)
        {
            entity.HasKey(e => e.TimetableCellId).HasName("TimetableCellPRIMARY");
            entity.UseTphMappingStrategy();
        }

        public static void ConfigureActualTimetable(EntityTypeBuilder<ActualTimetable> entity)
        {
            entity.HasBaseType<ITimetable>();
        }

        public static void ConfigureStableTimetable(EntityTypeBuilder<StableTimetable> entity)
        {
            entity.HasBaseType<ITimetable>();
        }

        public static void ConfigureTimetable(EntityTypeBuilder<ITimetable> entity)
        {
            entity.HasKey(e => e.TimetableId).HasName("TimetablePRIMARY");
            entity.UseTphMappingStrategy();
        }

        public static void ConfigureCabinet(EntityTypeBuilder<Cabinet> entity)
        {
            entity.HasKey(c => c.CabinetId).HasName("CabinetPRIMARY");
            entity.Property(c => c.Address).HasMaxLength(255);
            entity.Property(c => c.Number).HasMaxLength(255);
        }

        public static void ConfigureSubject(EntityTypeBuilder<Subject> entity)
        {
            entity.HasKey(s => s.SubjectId).HasName("SubjectPRIMARY");
            entity.Property(s => s.Name).HasMaxLength(255);
        }

        public static void ConfigureTeacher(EntityTypeBuilder<Teacher> entity)
        {
            entity.HasKey(t => t.TeacherId).HasName("TeacherPRIMARY");
            entity.Property(t => t.FirstName).HasMaxLength(255);
            entity.Property(t => t.MiddleName).HasMaxLength(255);
            entity.Property(t => t.Surname).HasMaxLength(255);
        }

        public static void ConfigureGroup(EntityTypeBuilder<Group> entity)
        {
            entity.HasKey(g => g.GroupId).HasName("GroupPRIMARY");
            entity.Property(g => g.Name).HasMaxLength(255);
        }
    }
}
