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

using KaVE.Commons.Model.Names;
using KaVE.Commons.Model.Names.CSharp;
using KaVE.Commons.Model.SSTs.Blocks;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Collections;

namespace KaVE.Commons.Model.SSTs.Impl.Blocks
{
    public class CatchBlock : ICatchBlock
    {
        public IParameterName Parameter { get; set; }
        public IKaVEList<IStatement> Body { get; set; }
        public bool IsGeneral { get; set; }
        public bool IsUnnamed { get; set; }

        public CatchBlock()
        {
            Parameter = ParameterName.UnknownName;
            Body = Lists.NewList<IStatement>();
        }

        private bool Equals(CatchBlock other)
        {
            return Body.Equals(other.Body) && Equals(Parameter, other.Parameter) && IsGeneral == other.IsGeneral &&
                   IsUnnamed == other.IsUnnamed;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Parameter.GetHashCode();
                hashCode = (hashCode*397) ^ Body.GetHashCode();
                hashCode = (hashCode*397) ^ IsGeneral.GetHashCode();
                hashCode = (hashCode*397) ^ IsUnnamed.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return this.ToStringReflection();
        }
    }
}