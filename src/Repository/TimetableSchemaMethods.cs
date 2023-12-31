﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities.Timetables;
using Core.Entities.Timetables.Cells;
using Core.Entities.Timetables.Cells.CellMembers;

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
            entity.HasOne(e => e.Cabinet).WithMany(e => e.ActualTimetableCells).IsRequired();
            entity.HasOne(e => e.Subject).WithMany(e => e.ActualTimetableCells).IsRequired();
            entity.HasOne(e => e.Teacher).WithMany(e => e.ActualTimetableCells).IsRequired();
            entity.HasOne(e => e.LessonTime).WithMany(e => e.ActualTimetableCells).IsRequired();
        }

        public static void ConfigureStableTimetableCell(EntityTypeBuilder<StableTimetableCell> entity)
        {
            entity.ToTable("StableTimetableCell", "timetable");
            entity.HasKey(e => e.TimetableCellId);
            entity.HasOne(e => e.Cabinet).WithMany(e => e.StableTimetableCells).IsRequired();
            entity.HasOne(e => e.Subject).WithMany(e => e.StableTimetableCells).IsRequired();
            entity.HasOne(e => e.Teacher).WithMany(e => e.StableTimetableCells).IsRequired();
            entity.HasOne(e => e.LessonTime).WithMany(e => e.StableTimetableCells).IsRequired();
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
            entity.HasIndex(e => e.AscId).IsUnique().AreNullsDistinct();
        }

        public static void ConfigureSubject(EntityTypeBuilder<Subject> entity)
        {
            entity.ToTable("Subject", "timetable");
            entity.HasKey(s => s.SubjectId);
            entity.Property(s => s.Name);
            entity.HasIndex(e => e.AscId).IsUnique().AreNullsDistinct();
        }

        public static void ConfigureTeacher(EntityTypeBuilder<TeacherCM> entity)
        {
            entity.ToTable("TeacherCM", "timetable");
            entity.HasKey(t => t.TeacherId);
            entity.Property(t => t.Firstname);
            entity.Property(t => t.Middlename);
            entity.Property(t => t.Lastname);
            entity.HasIndex(e => e.AscId).IsUnique().AreNullsDistinct();
        }

        public static void ConfigureGroup(EntityTypeBuilder<Group> entity)
        {
            entity.ToTable("Group", "timetable");
            entity.HasKey(g => g.GroupId);
            entity.Property(g => g.Name);
            entity.HasIndex(e => e.AscId).IsUnique().AreNullsDistinct();
        }
    }
}
