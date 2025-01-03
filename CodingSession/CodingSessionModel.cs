using System;

namespace CodingSession
{
    internal class CodingSessionModel
    {
        internal int Id { get; set; }
        internal DateTime StartTime { get; set; }
        internal DateTime EndTime { get; set; }
        internal double Duration { get; set; }

        internal CodingSessionModel (int id, DateTime startTime, DateTime endTime, double duration)
        {
            Id = id;
            StartTime = startTime;
            EndTime = endTime;
            Duration = duration;
        }

    }
}
