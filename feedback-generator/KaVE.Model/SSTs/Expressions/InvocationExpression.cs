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
 * 
 * Contributors:
 *    - Sebastian Proksch
 */

using KaVE.Model.Names;
using KaVE.Utils;
using KaVE.Utils.Assertion;

namespace KaVE.Model.SSTs.Expressions
{
    public class InvocationExpression : Expression
    {
        public string Identifier { get; set; }
        public IMethodName Name { get; set; }
        public string[] Parameters { get; set; }

        public InvocationExpression() {}

        public InvocationExpression(IMethodName name, params string[] parameters)
        {
            Asserts.That(name.IsStatic);
            Identifier = "";
            Name = name;

            Parameters = parameters;
        }

        public InvocationExpression(string id, IMethodName name, params string[] parameters)
        {
            Asserts.Not(name.IsStatic);
            Identifier = id;
            Name = name;
            Parameters = parameters;
        }

        private bool Equals(InvocationExpression other)
        {
            return string.Equals(Identifier, other.Identifier) && Equals(Name, other.Name) &&
                   Parameters.DeepEquals(other.Parameters);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Identifier != null ? Identifier.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Parameters != null ? HashCodeUtils.For(398, Parameters) : 0);
                return hashCode;
            }
        }
    }
}