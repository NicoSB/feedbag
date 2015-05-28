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
 *    - Sven Amann
 */

using System;
using System.Collections.Generic;
using KaVE.JetBrains.Annotations;

namespace KaVE.Commons.Model.Names.CSharp
{
    public class DelegateTypeName : Name, IDelegateTypeName
    {
        private const string Prefix = "d:";

        internal static bool IsDelegateTypeIdentifier(string identifier)
        {
            return identifier.StartsWith(Prefix);
        }

        [UsedImplicitly]
        public new static IDelegateTypeName Get(string identifier)
        {
            return (IDelegateTypeName) TypeName.Get(identifier);
        }

        internal static string FixLegacyDelegateNames(string identifier)
        {
            // fix legacy delegate names
            if (!identifier.Contains("("))
            {
                identifier = Prefix + "[Unknown, A] [" + identifier.Substring(Prefix.Length) + "].()";
            }
            return identifier;
        }

        internal DelegateTypeName(string identifier) : base(identifier) {}

        private IMethodName DelegateMethod
        {
            get { return MethodName.Get(Identifier.Substring(Prefix.Length)); }
        }

        public ITypeName DelegateType
        {
            get { return DelegateMethod.DeclaringType; }
        }

        public bool IsInterfaceType
        {
            get { return false; }
        }

        public bool IsDelegateType
        {
            get { return true; }
        }

        public bool IsNestedType
        {
            get { return DelegateType.IsNestedType; }
        }

        public bool IsArrayType
        {
            get { return false; }
        }

        public ITypeName ArrayBaseType
        {
            get { return null; }
        }

        public ITypeName DeriveArrayTypeName(int rank)
        {
            return ArrayTypeName.From(this, rank);
        }

        public bool IsTypeParameter
        {
            get { return false; }
        }

        public string TypeParameterShortName
        {
            get { return null; }
        }

        public ITypeName TypeParameterType
        {
            get { return null; }
        }

        public IAssemblyName Assembly
        {
            get { return DelegateType.Assembly; }
        }

        public INamespaceName Namespace
        {
            get { return DelegateType.Namespace; }
        }

        public ITypeName DeclaringType
        {
            get { return DelegateType.DeclaringType; }
        }

        public string FullName
        {
            get { return DelegateMethod.DeclaringType.FullName; }
        }

        public string Name
        {
            get { return DelegateType.Name; }
        }

        public bool IsUnknownType
        {
            get { return false; }
        }

        public bool IsVoidType
        {
            get { return false; }
        }

        public bool IsValueType
        {
            get { return false; }
        }

        public bool IsSimpleType
        {
            get { return false; }
        }

        public bool IsEnumType
        {
            get { return false; }
        }

        public bool IsStructType
        {
            get { return false; }
        }

        public bool IsNullableType
        {
            get { return false; }
        }

        public bool IsReferenceType
        {
            get { return true; }
        }

        public bool IsClassType
        {
            get { return false; }
        }

        public string Signature
        {
            get
            {
                var endOfValueType = Identifier.EndOfNextTypeIdentifier(2);
                var endOfDelegateType = Identifier.EndOfNextTypeIdentifier(endOfValueType);
                return Name + Identifier.Substring(endOfDelegateType + 1);
            }
        }

        public IList<IParameterName> Parameters
        {
            get { return DelegateMethod.Parameters; }
        }

        public bool HasParameters
        {
            get { return DelegateMethod.HasParameters; }
        }

        public ITypeName ReturnType
        {
            get { return DelegateMethod.ReturnType; }
        }

        public bool IsGenericEntity
        {
            get { return DelegateType.IsGenericEntity; }
        }

        public bool HasTypeParameters
        {
            get { return DelegateType.HasTypeParameters; }
        }

        public IList<ITypeName> TypeParameters
        {
            get { return DelegateType.TypeParameters; }
        }
    }
}