﻿/*
 * Copyright 2014 Technische Universität Darmstadt
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *    http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using KaVE.JetBrains.Annotations;

namespace KaVE.FeedbackProcessor.Intervals.Model
{
    public abstract class Interval
    {
        [NotNull]
        public DateTime StartTime { get; set; }

        [NotNull]
        public TimeSpan Duration { get; set; }

        public DateTime EndTime
        {
            get { return StartTime + Duration; }
        }

        [NotNull]
        public string UserId { get; set; }

        [NotNull]
        public string IDESessionId { get; set; }

        protected Interval()
        {
            StartTime = DateTime.MinValue;
            Duration = TimeSpan.Zero;
            UserId = string.Empty;
            IDESessionId = string.Empty;
        }

        protected bool Equals(Interval other)
        {
            return StartTime.Equals(other.StartTime) && Duration.Equals(other.Duration) &&
                   string.Equals(UserId, other.UserId) && string.Equals(IDESessionId, other.IDESessionId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((Interval) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StartTime.GetHashCode();
                hashCode = (hashCode*397) ^ Duration.GetHashCode();
                hashCode = (hashCode*397) ^ UserId.GetHashCode();
                hashCode = (hashCode*397) ^ IDESessionId.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Format(
                "{0}: {1} ~ {2} ({3}d {4}h {5}m {6}s)",
                GetType().Name,
                StartTime,
                EndTime,
                Duration.Days,
                Duration.Hours,
                Duration.Minutes,
                Duration.Seconds);
        }
    }
}