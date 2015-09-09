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

using System.Runtime.Serialization;
using KaVE.Commons.Model.Names.VisualStudio;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Collections;

namespace KaVE.Commons.Model.Events.GitEvents
{
    [DataContract]
    public class GitEvent : IDEEvent
    {
        [DataMember]
        public IKaVEList<GitAction> Content { get; set; }

        [DataMember]
        public SolutionName Solution { get; set; }

        public GitEvent()
        {
            Content = new KaVEList<GitAction>();
            Solution = SolutionName.Get("");
        }

        private bool Equals(GitEvent other)
        {
            return base.Equals(other) && Equals(Content, other.Content) && Equals(Solution, other.Solution);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ Content.GetHashCode();
                hashCode = (hashCode*397) ^ Solution.GetHashCode();
                return hashCode;
            }
        }
    }
}