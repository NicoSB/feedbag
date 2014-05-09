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
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Resolve;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using KaVE.Model.Names;
using KaVE.Utils.Assertion;
using KaVE.VsFeedbackGenerator.Utils.Names;
using NuGet;

namespace KaVE.VsFeedbackGenerator.Analysis
{
    internal class CalledMethodsAnalysis
    {
        public ISet<IMethodName> Analyze(IMethodDeclaration methodDeclaration, ITypeName enclosingType)
        {
            var context = new CollectionContext();
            if (methodDeclaration.Body != null)
            {
                methodDeclaration.Body.Accept(new MethodInvocationCollector(enclosingType), context);
            }
            return context.CalledMethods;
        }

        private class CollectionContext
        {
            public readonly ISet<IMethodName> CalledMethods = new HashSet<IMethodName>();
            public readonly ISet<IMethodName> AnalyzedMethods = new HashSet<IMethodName>();
        }

        private class MethodInvocationCollector : TreeNodeVisitor<CollectionContext>
        {
            private readonly ITypeName _enclosingType;

            public MethodInvocationCollector(ITypeName enclosingType)
            {
                _enclosingType = enclosingType;
            }

            public override void VisitNode(ITreeNode node, CollectionContext context)
            {
                foreach (var childNode in node.Children<ICSharpTreeNode>())
                {
                    childNode.Accept(this, context);
                }
            }

            public override void VisitInvocationExpression(IInvocationExpression invocation, CollectionContext context)
            {
                VisitSubexpressions(invocation, context);
                AnalyzeInvocation(invocation, context);
            }

            private void VisitSubexpressions(IInvocationExpression invocation, CollectionContext context)
            {
                base.VisitInvocationExpression(invocation, context);
            }

            private void AnalyzeInvocation(IInvocationExpression invocation, CollectionContext context)
            {
                var invocationRef = invocation.Reference;
                if (invocationRef != null)
                {
                    AnalyzeInvocationReference(context, invocationRef);
                }
            }

            private void AnalyzeInvocationReference(CollectionContext context, ICSharpInvocationReference invocationRef)
            {
                var method = ResolveMethod(invocationRef);
                if (method == null)
                {
                    return;
                }

                var methodName = method.GetName<IMethodName>();
                if (IsEntryPoint(methodName, method))
                {
                    if (method.Element.IsOverride)
                    {
                        methodName = method.Element.FindFirstMethodInSuperTypes();
                    }
                    context.CalledMethods.Add(methodName);
                }
                else
                {
                    TrackCallsInMethod(context, methodName, method);
                }
            }

            private bool IsEntryPoint(IMethodName methodName, DeclaredElementInstance<IMethod> method)
            {
                return !IsLocalHelper(methodName) || method.Element.IsOverride || method.Element.IsAbstract;
            }

            private bool IsLocalHelper(IMemberName method)
            {
                return _enclosingType == method.DeclaringType;
            }

            private void TrackCallsInMethod(CollectionContext context, IMethodName methodName, DeclaredElementInstance<IMethod> method)
            {
                if (context.AnalyzedMethods.Contains(methodName) || method.Element.IsAbstract)
                {
                    return;
                }
                context.AnalyzedMethods.Add(methodName);
                var declaration = GetDeclaration(method.Element);
                declaration.Body.Accept(this, context);
            }

            private static DeclaredElementInstance<IMethod> ResolveMethod(ICSharpInvocationReference invocationRef)
            {
                var resolvedRef = invocationRef.Resolve().Result;
                IMethod declaration = null;
                ISubstitution substitution = null;
                if (resolvedRef.DeclaredElement != null)
                {
                    declaration = (IMethod) resolvedRef.DeclaredElement;
                    substitution = resolvedRef.Substitution;
                }
                else if (!resolvedRef.Candidates.IsEmpty())
                {
                    // TODO reconsider this, maybe switch to "invocations" as analysis result, where an invocation can have zero, one, or more methods as its target
                    declaration = (IMethod) resolvedRef.Candidates.First();
                    substitution = resolvedRef.CandidateSubstitutions.First();
                }

                if (declaration != null)
                {
                    return new DeclaredElementInstance<IMethod>(declaration, substitution);
                }
                return null;
            }

            private static IMethodDeclaration GetDeclaration(IMethod method)
            {
                var declarations = method.GetDeclarations();
                Asserts.That(declarations.Count <= 1, "more than one declaration for invoked method");
                return (IMethodDeclaration) declarations.First();
            }
        }
    }
}