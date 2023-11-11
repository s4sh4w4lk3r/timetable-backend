using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using Models.Entities.Timetables.Cells.CellMembers;

namespace Repository
{
    internal static class TimetableSchemaMethods
    {
        public static void ConfigureLessonTime(EntityTypeBuilder<LessonTime> entity)
        {
            entity.ToTable("LessonTime", "timetable");
            entity.HasKey(e => e.LessonTimeId);
        }

        public static void ConfigureActualTimetableCell(EntityTypeBuilder<ActualTimetableCell> entity)
        {
            entity.ToTable("ActualTimetableCell", "timetable");
            entity.HasKey(e => e.TimetableCellId);
            entity.HasOne(e => e.Cabinet).WithMany(e => e.ActualTimetableCells);
            entity.HasOne(e => e.Subject).WithMany(e => e.ActualTimetableCells);
            entity.HasOne(e => e.Teacher).WithMany(e => e.ActualTimetableCells);
            entity.HasOne(e => e.LessonTime).WithMany(e => e.ActualTimetableCells);
        }

        public static void ConfigureStableTimetableCell(EntityTypeBuilder<StableTimetableCell> entity)
        {
            entity.ToTable("StableTimetableCell", "timetable");
            entity.HasKey(e => e.TimetableCellId);
            entity.HasOne(e => e.Cabinet).WithMany(e => e.StableTimetableCells);
            entity.HasOne(e => e.Subject).WithMany(e => e.StableTimetableCells);
            entity.HasOne(e => e.Teacher).WithMany(e => e.StableTimetableCells);
            entity.HasOne(e => e.LessonTime).WithMany(e => e.StableTimetableCells);
        }

        public static void ConfigureActualTimetable(EntityTypeBuilder<ActualTimetable> entity)
        {
            entity.ToTable("ActualTimetable", "timetable");
            entity.HasKey(e => e.TimetableId);
        }

        public static void ConfigureStableTimetable(EntityTypeBuilder<StableTimetable> entity)
        {
            entity.ToTable("StableTimetable", "timetable");
            entity.HasKey(e => e.TimetableId);
        }

        public static void ConfigureCabinet(EntityTypeBuilder<Cabinet> entity)
        {
            entity.ToTable("Cabinet", "timetable");
            entity.HasKey(c => c.CabinetId);
            entity.Property(c => c.Address);
            entity.Property(c => c.Number);
        }

        public static void ConfigureSubject(EntityTypeBuilder<Subject> entity)
        {
            entity.ToTable("Subject", "timetable");
            entity.HasKey(s => s.SubjectId);
            entity.Property(s => s.Name);
        }

        public static void ConfigureTeacher(EntityTypeBuilder<TeacherCM> entity)
        {
            entity.ToTable("TeacherCM", "timetable");
            entity.HasKey(t => t.TeacherId);
            entity.Property(t => t.FirstName);
            entity.Property(t => t.MiddleName);
            entity.Property(t => t.Surname);
        }

        public static void ConfigureGroup(EntityTypeBuilder<Group> entity)
        {
            entity.ToTable("Group", "timetable");
            entity.HasKey(g => g.GroupId);
            entity.Property(g => g.Name);
        }
    }
}
