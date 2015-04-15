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

using System.Runtime.Serialization;
using KaVE.Commons.Model.Names;
using KaVE.Commons.Model.Names.CSharp;
using KaVE.Commons.Model.SSTs.Declarations;
using KaVE.Commons.Model.SSTs.Impl.References;
using KaVE.Commons.Model.SSTs.References;
using KaVE.Commons.Model.SSTs.Visitor;
using KaVE.Commons.Utils;

namespace KaVE.Commons.Model.SSTs.Impl.Declarations
{
    [DataContract]
    public class VariableDeclaration : IVariableDeclaration
    {
        [DataMember]
        public IVariableReference Reference { get; set; }

        [DataMember]
        public ITypeName Type { get; set; }

        public bool IsMissing
        {
            get { return Reference.IsMissing; }
        }

        public VariableDeclaration()
        {
            Reference = new VariableReference();
            Type = TypeName.UnknownName;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        private bool Equals(VariableDeclaration other)
        {
            return Equals(Reference, other.Reference) && Equals(Type, other.Type);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return 20 + (Reference.GetHashCode()*397) ^
                       Type.GetHashCode();
            }
        }

        public void Accept<TContext>(ISSTNodeVisitor<TContext> visitor, TContext context)
        {
            visitor.Visit(this, context);
        }

        public TReturn Accept<TContext, TReturn>(ISSTNodeVisitor<TContext, TReturn> visitor, TContext context)
        {
            return visitor.Visit(this, context);
        }
    }
}