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

using KaVE.Commons.Model.Naming;
using KaVE.Commons.Utils;
using KaVE.JetBrains.Annotations;

namespace KaVE.Commons.Model.ObjectUsage
{
    // ReSharper disable InconsistentNaming

    public class CallSite
    {
        public CallSiteKind kind { get; set; }

        [NotNull]
        public CoReMethodName method { get; set; }

        public int argIndex { get; set; }

        public CallSite()
        {
            kind = CallSiteKind.RECEIVER;
            argIndex = 0;
            method = Names.UnknownMethod.ToCoReName();
        }

        /// <summary>
        ///     This method is used by the serialization to determine if the property should be serialized or not.
        ///     As the method's name depends directly on the property's name, the method must not be renamed.
        /// </summary>
        public bool ShouldSerializeargIndex()
        {
            return argIndex != 0;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        private bool Equals(CallSite oth)
        {
            return kind.Equals(oth.kind) && argIndex == oth.argIndex && Equals(method, oth.method);
        }

        public override int GetHashCode()
        {
            var hashCode = 397;
            hashCode = (hashCode*397) ^ kind.GetHashCode();
            hashCode = (hashCode*397) ^ method.GetHashCode();
            hashCode = (hashCode*397) ^ argIndex.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return this.ToStringReflection();
        }
    }
}