﻿using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables
{
    public abstract class Timetable
    {
        public required int TimetableId { get; init; }
        public required Group? Group { get; init; }

        protected Timetable() { }

        [SetsRequiredMembers]
        protected Timetable(int timetableId, Group group)
        {
            timetableId.Throw().IfDefault();
            group.ThrowIfNull();

            TimetableId = timetableId;
            Group = group;
        }
    }
}
#warning может юзать прокси и попробовать избежать рефренс лупа https://www.google.com/search?q=reference+loop+proxies+efcore&oq=reference+loop+proxies+efcore&gs_lcrp=EgZjaHJvbWUyBggAEEUYOTIJCAEQIRgKGKABMgkIAhAhGAoYoAEyBggDECEYFTIHCAQQIRiPAjIHCAUQIRiPAtIBCTIzMDI0ajFqOagCALACAA&sourceid=chrome&ie=UTF-8