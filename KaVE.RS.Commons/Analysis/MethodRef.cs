/*
 * Copyright 2014 Technische Universitšt Darmstadt
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

using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using KaVE.Commons.Model.Naming.CodeElements;
using KaVE.Commons.Utils;

namespace KaVE.RS.Commons.Analysis
{
    public class MethodRef
    {
        public IMethodName Name { get; private set; }
        public IMethodDeclaration Declaration { get; private set; }
        public IMethod Method { get; private set; }
        public IConstructorDeclaration ConstructorDeclaration { get; private set; }
        public IConstructor Constructor { get; private set; }
        public bool IsAssemblyReference { get; private set; }

        public static MethodRef CreateLocalReference(IMethodName methodName,
            IMethod method,
            IMethodDeclaration methodDecl)
        {
            return new MethodRef
            {
                Name = methodName,
                Declaration = methodDecl,
                Method = method,
                IsAssemblyReference = false
            };
        }

        public static MethodRef CreateAssemblyReference(IMethodName methodName, IMethod method)
        {
            return new MethodRef
            {
                Name = methodName,
                Declaration = null,
                Method = method,
                IsAssemblyReference = true
            };
        }

        public static MethodRef CreateConstructorReference(IMethodName methodName, IConstructor ctor, IConstructorDeclaration ctorDecl)
        {
            return new MethodRef
            {
                Name = methodName,
                Constructor = ctor,
                ConstructorDeclaration = ctorDecl,
                IsAssemblyReference = false
            };
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, Equals);
        }

        private bool Equals(MethodRef other)
        {
            return Equals(Name, other.Name) && Equals(Declaration, other.Declaration) && Equals(Method, other.Method) &&
                   (IsAssemblyReference == other.IsAssemblyReference);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Declaration != null ? Declaration.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Method != null ? Method.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ IsAssemblyReference.GetHashCode();
                return hashCode;
            }
        }
    }
}