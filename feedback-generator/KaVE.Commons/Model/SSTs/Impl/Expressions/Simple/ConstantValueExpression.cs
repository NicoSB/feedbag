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

using System.Collections.Generic;
using System.Runtime.Serialization;
using KaVE.Commons.Model.SSTs.Expressions.Simple;
using KaVE.Commons.Model.SSTs.Visitor;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.Collections;

namespace KaVE.Commons.Model.SSTs.Impl.Expressions.Simple
{
    [DataContract]
    public class ConstantValueExpression : IConstantValueExpression
    {
        public const string Null = "null";
        public const string Sizeof = "sizeof";
        public const string Typeof = "typeof";
        public const string Default = "default";
        public const string True = "true";
        public const string False = "false";

        [DataMember]
        public string Value { get; set; }

        public IEnumerable<ISSTNode> Children
        {
            get { return Lists.NewList<ISSTNode>(); }
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, other => string.Equals(Value, other.Value));
        }

        public override int GetHashCode()
        {
            return 102 + (Value != null ? Value.GetHashCode() : 0);
        }

        public void Accept<TContext>(ISSTNodeVisitor<TContext> visitor, TContext context)
        {
            visitor.Visit(this, context);
        }

        public TReturn Accept<TContext, TReturn>(ISSTNodeVisitor<TContext, TReturn> visitor, TContext context)
        {
            return visitor.Visit(this, context);
        }

        public override string ToString()
        {
            return Value == null
                ? "Const()"
                : string.Format("Const('{0}')", Value);
        }
    }
}