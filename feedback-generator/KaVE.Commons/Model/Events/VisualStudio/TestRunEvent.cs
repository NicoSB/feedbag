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
using System.Runtime.Serialization;
using KaVE.Commons.Model.Names;
using KaVE.Commons.Model.Names.CSharp;
using KaVE.Commons.Utils.Collections;

namespace KaVE.Commons.Model.Events.VisualStudio
{
    public class TestRunEvent : IDEEvent
    {
        [DataMember]
        public bool WasAborted { get; set; }

        [DataMember]
        public IKaVEList<TestCaseResult> Tests { get; private set; }

        public TestRunEvent()
        {
            Tests = Lists.NewList<TestCaseResult>();
        }

        protected bool Equals(TestRunEvent other)
        {
            return base.Equals(other) && WasAborted == other.WasAborted && Equals(Tests, other.Tests);
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
            return Equals((TestRunEvent) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ WasAborted.GetHashCode();
                hashCode = (hashCode*397) ^ (Tests != null ? Tests.GetHashCode() : 0);
                return hashCode;
            }
        }
    }

    public class TestCaseResult
    {
        [DataMember]
        public IMethodName TestMethod { get; set; }

        [DataMember]
        public string Parameters { get; set; }

        [DataMember]
        public TimeSpan Duration { get; set; }

        [DataMember]
        public TestResult Result { get; set; }

        public TestCaseResult()
        {
            TestMethod = MethodName.UnknownName;
            Parameters = "";
        }

        protected bool Equals(TestCaseResult other)
        {
            return Equals(TestMethod, other.TestMethod) && string.Equals(Parameters, other.Parameters) &&
                   Duration.Equals(other.Duration) && Result == other.Result;
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
            return Equals((TestCaseResult) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (TestMethod != null ? TestMethod.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Parameters != null ? Parameters.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Duration.GetHashCode();
                hashCode = (hashCode*397) ^ (int) Result;
                return hashCode;
            }
        }
    }

    public enum TestResult
    {
        Unknown,
        Success,
        Failed,
        Ignored,
        Error
    }
}