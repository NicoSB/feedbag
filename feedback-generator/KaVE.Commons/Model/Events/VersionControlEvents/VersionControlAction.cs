/*
 * Copyright 2014 Technische Universit�t Darmstadt
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
using System.Runtime.Serialization;
using KaVE.Commons.Utils;

namespace KaVE.Commons.Model.Events.VersionControlEvents
{
    public interface IVersionControlAction
    {
        DateTime ExecutedAt { get; set; }
        VersionControlActionType ActionType { get; set; }
    }

    [DataContract]
    public class VersionControlAction : IVersionControlAction
    {
        [DataMember]
        public DateTime ExecutedAt { get; set; }

        [DataMember]
        public VersionControlActionType ActionType { get; set; }

        private bool Equals(VersionControlAction other)
        {
            return Equals(ExecutedAt, other.ExecutedAt) && Equals(ActionType, other.ActionType);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ExecutedAt.GetHashCode();
                hashCode = (hashCode*397) ^ (int) ActionType;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return this.ToStringReflection();
        }
    }
}