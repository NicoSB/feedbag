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

using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using KaVE.Commons.Model.Naming;
using KaVE.Commons.Model.Naming.CodeElements;
using KaVE.Commons.Model.Naming.Types;
using KaVE.Commons.Model.SSTs.Blocks;
using KaVE.Commons.Model.SSTs.Expressions;
using KaVE.Commons.Model.SSTs.Impl;
using KaVE.Commons.Model.SSTs.Impl.Blocks;
using KaVE.Commons.Model.SSTs.Impl.Expressions.Assignable;
using KaVE.Commons.Model.SSTs.Impl.Expressions.Simple;
using KaVE.Commons.Model.SSTs.Impl.References;
using KaVE.Commons.Model.SSTs.Impl.Statements;
using KaVE.Commons.Model.SSTs.References;
using KaVE.Commons.Model.SSTs.Statements;
using KaVE.Commons.Utils.Assertion;
using KaVE.Commons.Utils.Collections;
using KaVE.Commons.Utils.Exceptions;
using KaVE.RS.Commons.Analysis.CompletionTarget;
using KaVE.RS.Commons.Analysis.Util;
using KaVE.RS.Commons.Utils.Naming;
using IBreakStatement = JetBrains.ReSharper.Psi.CSharp.Tree.IBreakStatement;
using IContinueStatement = JetBrains.ReSharper.Psi.CSharp.Tree.IContinueStatement;
using IExpressionStatement = JetBrains.ReSharper.Psi.CSharp.Tree.IExpressionStatement;
using IReturnStatement = JetBrains.ReSharper.Psi.CSharp.Tree.IReturnStatement;
using IStatement = KaVE.Commons.Model.SSTs.IStatement;
using IThrowStatement = JetBrains.ReSharper.Psi.CSharp.Tree.IThrowStatement;

namespace KaVE.RS.Commons.Analysis.Transformer
{
    public class BodyVisitor : TreeNodeVisitor<IList<IStatement>>
    {
        private readonly CompletionTargetMarker _marker;
        private readonly ExpressionVisitor _exprVisitor;
        private readonly UniqueVariableNameGenerator _nameGen;

        private static ExpressionStatement EmptyCompletionExpression
        {
            get { return new ExpressionStatement {Expression = new CompletionExpression()}; }
        }

        public BodyVisitor(UniqueVariableNameGenerator nameGen, CompletionTargetMarker marker)
        {
            _marker = marker;
            _nameGen = nameGen;
            _exprVisitor = new ExpressionVisitor(_nameGen, marker);
        }

        public override void VisitNode(ITreeNode node, IList<IStatement> context)
        {
            node.Children<ICSharpTreeNode>().ForEach(
                child =>
                {
                    try
                    {
                        child.Accept(this, context);
                    }
                    catch (NullReferenceException) {}
                    catch (AssertException) {}
                });
        }

        #region statements

        public override void VisitBreakStatement(IBreakStatement stmt, IList<IStatement> body)
        {
            AddIf(stmt, CompletionCase.EmptyCompletionBefore, body);

            body.Add(new BreakStatement());

            AddIf(stmt, CompletionCase.EmptyCompletionAfter, body);
        }

        public override void VisitLocalVariableDeclaration(ILocalVariableDeclaration decl, IList<IStatement> body)
        {
            if (IsTargetMatch(decl, CompletionCase.EmptyCompletionBefore))
            {
                body.Add(EmptyCompletionExpression);
            }

            var id = decl.DeclaredName;
            ITypeName type;
            try
            {
                type = decl.Type.GetName();
            }
            catch (AssertException)
            {
                // TODO this is an intermediate "fix"... the analysis sometimes fails here ("cannot create name for anonymous type")
                type = Names.UnknownType;
            }
            body.Add(SSTUtil.Declare(id, type));

            IAssignableExpression initializer = null;
            if (decl.Initial != null)
            {
                initializer = _exprVisitor.ToAssignableExpr(decl.Initial, body);
            }
            else if (_marker.AffectedNode == decl && _marker.Case == CompletionCase.Undefined)
            {
                initializer = new CompletionExpression();
            }

            if (initializer != null)
            {
                if (!IsSelfAssign(id, initializer))
                {
                    body.Add(SSTUtil.AssignmentToLocal(id, initializer));
                }
            }

            if (decl == _marker.AffectedNode && _marker.Case == CompletionCase.EmptyCompletionAfter)
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        public override void VisitDeclarationStatement(IDeclarationStatement decl, IList<IStatement> body)
        {
            if (IsTargetMatch(decl, CompletionCase.EmptyCompletionBefore))
            {
                body.Add(EmptyCompletionExpression);
            }

            VisitNode(decl.Declaration, body);

            if (decl == _marker.AffectedNode && _marker.Case == CompletionCase.EmptyCompletionAfter)
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        public override void VisitLocalConstantDeclaration(ILocalConstantDeclaration decl, IList<IStatement> body)
        {
            if (IsTargetMatch(decl, CompletionCase.EmptyCompletionBefore))
            {
                body.Add(EmptyCompletionExpression);
            }

            var id = decl.DeclaredName;
            ITypeName type;
            try
            {
                type = decl.Type.GetName();
            }
            catch (AssertException)
            {
                // TODO this is an intermediate "fix"... the analysis sometimes fails here ("cannot create name for anonymous type")
                type = Names.UnknownType;
            }
            body.Add(SSTUtil.Declare(id, type));

            IAssignableExpression initializer = null;
            if (decl.ValueExpression != null)
            {
                initializer = _exprVisitor.ToAssignableExpr(decl.ValueExpression, body);
            }
            else if (_marker.AffectedNode == decl && _marker.Case == CompletionCase.Undefined)
            {
                initializer = new CompletionExpression();
            }

            if (initializer != null)
            {
                if (!IsSelfAssign(id, initializer))
                {
                    body.Add(SSTUtil.AssignmentToLocal(id, initializer));
                }
            }

            if (decl == _marker.AffectedNode && _marker.Case == CompletionCase.EmptyCompletionAfter)
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        private bool IsTargetMatch(ICSharpTreeNode o, CompletionCase completionCase)
        {
            var isValid = _marker.AffectedNode != null;
            var isMatch = o == _marker.AffectedNode;
            var isRightCase = _marker.Case == completionCase;
            return isValid && isMatch && isRightCase;
        }

        public override void VisitAssignmentExpression(IAssignmentExpression expr, IList<IStatement> body)
        {
            if (IsTargetMatch(expr, CompletionCase.EmptyCompletionBefore))
            {
                body.Add(EmptyCompletionExpression);
            }

            var isTarget = IsTargetMatch(expr, CompletionCase.Undefined);

            var sstRef = _exprVisitor.ToAssignableRef(expr.Dest, body) ?? new UnknownReference();

            var sstExpr =
                isTarget
                    ? new CompletionExpression()
                    : _exprVisitor.ToAssignableExpr(expr.Source, body);

            var operation = TryGetEventSubscriptionOperation(expr);
            if (operation.HasValue)
            {
                body.Add(
                    new EventSubscriptionStatement
                    {
                        Reference = sstRef,
                        Operation = operation.Value,
                        Expression = sstExpr
                    });
            }
            else
            {
                if (!IsSelfAssign(sstRef, sstExpr))
                {
                    body.Add(
                        new Assignment
                        {
                            Reference = sstRef,
                            Expression = IsFancyAssign(expr) ? new ComposedExpression() : sstExpr
                        });
                }
            }

            if (IsTargetMatch(expr, CompletionCase.EmptyCompletionAfter))
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        private static bool IsSelfAssign(string id, IAssignableExpression sstExpr)
        {
            return IsSelfAssign(new VariableReference {Identifier = id}, sstExpr);
        }

        private static bool IsSelfAssign(IAssignableReference sstRef, IAssignableExpression sstExpr)
        {
            // TODO add test!
            var refExpr = sstExpr as KaVE.Commons.Model.SSTs.Expressions.Simple.IReferenceExpression;
            return refExpr != null && sstRef.Equals(refExpr.Reference);
        }

        private static bool IsFancyAssign(IAssignmentExpression expr)
        {
            return expr.AssignmentType != AssignmentType.EQ;
        }

        private EventSubscriptionOperation? TryGetEventSubscriptionOperation(IAssignmentExpression expr)
        {
            var isRegularEq = expr.AssignmentType == AssignmentType.EQ;
            if (isRegularEq)
            {
                return null;
            }

            var refExpr = expr.Dest as IReferenceExpression;
            if (refExpr == null)
            {
                return null;
            }

            var elem = refExpr.Reference.Resolve().DeclaredElement;
            if (elem == null)
            {
                return null;
            }

            var loc = elem as ITypeOwner;
            if (loc != null)
            {
                var type = loc.Type.GetName();
                if (!type.IsDelegateType)
                {
                    return null;
                }
            }

            var isAdd = expr.AssignmentType == AssignmentType.PLUSEQ;
            if (isAdd)
            {
                return EventSubscriptionOperation.Add;
            }

            var isRemove = expr.AssignmentType == AssignmentType.MINUSEQ;
            if (isRemove)
            {
                return EventSubscriptionOperation.Remove;
            }

            return null;
        }

        /*private static AssignmentOperation ToOperation(AssignmentType assignmentType)
        {
            switch (assignmentType)
            {
                case AssignmentType.EQ:
                    return AssignmentOperation.Equals;
                case AssignmentType.PLUSEQ:
                    return AssignmentOperation.Add;
                case AssignmentType.MINUSEQ:
                    return AssignmentOperation.Remove;
                default:
                    return AssignmentOperation.Unknown;
            }
        }*/

        public override void VisitExpressionStatement(IExpressionStatement stmt, IList<IStatement> body)
        {
            if (stmt.Expression != null)
            {
                var assignment = stmt.Expression as IAssignmentExpression;
                var prefix = stmt.Expression as IPrefixOperatorExpression;
                var postfix = stmt.Expression as IPostfixOperatorExpression;
                if (assignment != null)
                {
                    assignment.Accept(this, body);
                }
                else if (prefix != null)
                {
                    prefix.Accept(this, body);
                }
                else if (postfix != null)
                {
                    postfix.Accept(this, body);
                }
                else
                {
                    body.Add(
                        new ExpressionStatement
                        {
                            Expression = stmt.Expression.Accept(_exprVisitor, body) ?? new UnknownExpression()
                        });

                    if (IsTargetMatch(stmt.Expression, CompletionCase.EmptyCompletionAfter))
                    {
                        body.Add(
                            new ExpressionStatement
                            {
                                Expression = new CompletionExpression()
                            });
                    }
                }
            }
        }

        public override void VisitPrefixOperatorExpression(IPrefixOperatorExpression expr, IList<IStatement> body)
        {
            if (IsTargetMatch(expr, CompletionCase.EmptyCompletionBefore))
            {
                body.Add(EmptyCompletionExpression);
            }
            var varRef = _exprVisitor.ToVariableRef(expr.Operand, body);
            if (varRef.IsMissing)
            {
                body.Add(new UnknownStatement());
            }
            else
            {
                body.Add(
                    new Assignment
                    {
                        Reference = varRef,
                        Expression = new ComposedExpression
                        {
                            References = {varRef}
                        }
                    });
            }
            if (IsTargetMatch(expr, CompletionCase.EmptyCompletionAfter))
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        public override void VisitPostfixOperatorExpression(IPostfixOperatorExpression expr, IList<IStatement> body)
        {
            if (IsTargetMatch(expr, CompletionCase.EmptyCompletionBefore))
            {
                body.Add(EmptyCompletionExpression);
            }
            var varRef = _exprVisitor.ToVariableRef(expr.Operand, body);
            if (varRef.IsMissing)
            {
                body.Add(new UnknownStatement());
            }
            else
            {
                body.Add(
                    new Assignment
                    {
                        Reference = varRef,
                        Expression = new ComposedExpression
                        {
                            References = {varRef}
                        }
                    });
            }
            if (IsTargetMatch(expr, CompletionCase.EmptyCompletionAfter))
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        public override void VisitReturnStatement(IReturnStatement stmt, IList<IStatement> body)
        {
            if (IsTargetMatch(stmt, CompletionCase.EmptyCompletionBefore))
            {
                body.Add(EmptyCompletionExpression);
            }

            if (stmt.Value == null)
            {
                body.Add(
                    new ReturnStatement
                    {
                        IsVoid = true
                    });
            }
            else
            {
                body.Add(
                    new ReturnStatement
                    {
                        Expression = _exprVisitor.ToSimpleExpression(stmt.Value, body) ?? new UnknownExpression()
                    });
            }

            if (IsTargetMatch(stmt, CompletionCase.EmptyCompletionAfter))
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        public override void VisitThrowStatement(IThrowStatement stmt, IList<IStatement> body)
        {
            if (IsTargetMatch(stmt, CompletionCase.EmptyCompletionBefore))
            {
                body.Add(EmptyCompletionExpression);
            }

            IVariableReference varRef = new VariableReference();

            if (stmt.Semicolon == null && IsTargetMatch(stmt, CompletionCase.EmptyCompletionAfter))
            {
                varRef = new VariableReference {Identifier = _nameGen.GetNextVariableName()};
                body.Add(
                    new VariableDeclaration
                    {
                        Type = Names.Type("System.Exception, mscorlib, 4.0.0.0"),
                        Reference = varRef
                    });
                body.Add(new Assignment {Reference = varRef, Expression = new CompletionExpression()});
            }
            else if (stmt.Exception != null)
            {
                varRef = _exprVisitor.ToVariableRef(stmt.Exception, body);
            }

            body.Add(new ThrowStatement {Reference = varRef});

            if (IsTargetMatch(stmt, CompletionCase.EmptyCompletionAfter))
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        public override void VisitEmptyStatement(IEmptyStatement stmt, IList<IStatement> body)
        {
            if (stmt == _marker.AffectedNode)
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        public override void VisitContinueStatement(IContinueStatement stmt, IList<IStatement> body)
        {
            if (IsTargetMatch(stmt, CompletionCase.EmptyCompletionBefore))
            {
                body.Add(EmptyCompletionExpression);
            }
            body.Add(new ContinueStatement());
            if (IsTargetMatch(stmt, CompletionCase.EmptyCompletionAfter))
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        #endregion

        #region blocks

        public override void VisitIfStatement(IIfStatement stmt, IList<IStatement> body)
        {
            if (IsTargetMatch(stmt, CompletionCase.EmptyCompletionBefore))
            {
                body.Add(EmptyCompletionExpression);
            }
            var ifElseBlock = new IfElseBlock
            {
                Condition = _exprVisitor.ToSimpleExpression(stmt.Condition, body) ?? new UnknownExpression()
            };
            if (IsTargetMatch(stmt, CompletionCase.InBody))
            {
                ifElseBlock.Then.Add(EmptyCompletionExpression);
            }
            if (IsTargetMatch(stmt, CompletionCase.InElse))
            {
                ifElseBlock.Else.Add(EmptyCompletionExpression);
            }
            if (stmt.Then != null)
            {
                stmt.Then.Accept(this, ifElseBlock.Then);
            }
            if (stmt.Else != null)
            {
                stmt.Else.Accept(this, ifElseBlock.Else);
            }

            body.Add(ifElseBlock);

            if (IsTargetMatch(stmt, CompletionCase.EmptyCompletionAfter))
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        public override void VisitWhileStatement(IWhileStatement rsLoop, IList<IStatement> body)
        {
            if (_marker.AffectedNode == rsLoop && _marker.Case == CompletionCase.EmptyCompletionBefore)
            {
                body.Add(EmptyCompletionExpression);
            }

            var loop = new WhileLoop
            {
                Condition = _exprVisitor.ToLoopHeaderExpression(rsLoop.Condition, body)
            };

            body.Add(loop);

            rsLoop.Body.Accept(this, loop.Body);

            if (_marker.AffectedNode == rsLoop && _marker.Case == CompletionCase.InBody)
            {
                loop.Body.Add(EmptyCompletionExpression);
            }

            if (_marker.AffectedNode == rsLoop && _marker.Case == CompletionCase.EmptyCompletionAfter)
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        public override void VisitForStatement(IForStatement stmt, IList<IStatement> body)
        {
            if (IsTargetMatch(stmt, CompletionCase.EmptyCompletionBefore))
            {
                body.Add(EmptyCompletionExpression);
            }

            var forLoop = new ForLoop();
            body.Add(forLoop);

            if (IsTargetMatch(stmt, CompletionCase.InBody))
            {
                forLoop.Body.Add(EmptyCompletionExpression);
            }

            VisitForStatement_Init(stmt.Initializer, forLoop.Init, body);
            forLoop.Condition = _exprVisitor.ToLoopHeaderExpression(stmt.Condition, body);
            foreach (var expr in stmt.IteratorExpressionsEnumerable)
            {
                expr.Accept(this, forLoop.Step);
            }

            if (stmt.Body != null)
            {
                stmt.Body.Accept(this, forLoop.Body);
            }

            if (IsTargetMatch(stmt, CompletionCase.EmptyCompletionAfter))
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        private void VisitForStatement_Init(IForInitializer init, IKaVEList<IStatement> forInit, IList<IStatement> body)
        {
            if (init == null)
            {
                return;
            }

            // case 1: single declaration
            var isDeclaration = init.Declaration != null;
            if (isDeclaration)
            {
                var decl = init.Declaration.Declarators[0];
                decl.Accept(this, forInit);
            }

            // case 2: multiple statements
            var hasStatements = init.Expressions.Count > 0;
            if (hasStatements)
            {
                foreach (var expr in init.ExpressionsEnumerable)
                {
                    expr.Accept(this, forInit);
                }
            }
        }

        public override void VisitForeachStatement(IForeachStatement stmt, IList<IStatement> body)
        {
            if (IsTargetMatch(stmt, CompletionCase.EmptyCompletionBefore))
            {
                body.Add(EmptyCompletionExpression);
            }

            var loop = new ForEachLoop
            {
                LoopedReference = _exprVisitor.ToVariableRef(stmt.Collection, body)
            };
            body.Add(loop);

            foreach (var itDecl in stmt.IteratorDeclarations)
            {
                var localVar = itDecl.DeclaredElement.GetName<ILocalVariableName>();
                loop.Declaration = new VariableDeclaration
                {
                    Reference = new VariableReference {Identifier = localVar.Name},
                    Type = localVar.ValueType
                };
            }

            if (IsTargetMatch(stmt, CompletionCase.InBody))
            {
                loop.Body.Add(EmptyCompletionExpression);
            }

            if (stmt.Body != null)
            {
                stmt.Body.Accept(this, loop.Body);
            }


            if (IsTargetMatch(stmt, CompletionCase.EmptyCompletionAfter))
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        public override void VisitTryStatement(ITryStatement block, IList<IStatement> body)
        {
            AddIf(block, CompletionCase.EmptyCompletionBefore, body);

            var tryBlock = new TryBlock();
            body.Add(tryBlock);

            AddIf(block, CompletionCase.InBody, tryBlock.Body);
            AddIf(block, CompletionCase.InFinally, tryBlock.Finally);
            VisitBlock(block.Try, tryBlock.Body);
            VisitBlock(block.FinallyBlock, tryBlock.Finally);

            foreach (var clause in block.Catches)
            {
                var catchBlock = new CatchBlock();
                tryBlock.CatchBlocks.Add(catchBlock);

                AddIf(clause, CompletionCase.InBody, catchBlock.Body);

                VisitBlock(clause.Body, catchBlock.Body);

                var generalClause = clause as IGeneralCatchClause;
                if (generalClause != null)
                {
                    catchBlock.Kind = CatchBlockKind.General;
                    continue;
                }

                var specificClause = clause as ISpecificCatchClause;
                if (specificClause != null)
                {
                    var varDecl = specificClause.ExceptionDeclaration;
                    var isUnnamed = varDecl == null;

                    var typeName = specificClause.ExceptionType.GetName();
                    var varName = isUnnamed ? "?" : varDecl.DeclaredName;
                    catchBlock.Parameter = Names.Parameter("[{0}] {1}", typeName, varName);
                    catchBlock.Kind = isUnnamed ? CatchBlockKind.Unnamed : CatchBlockKind.Default;
                }
            }

            AddIf(block, CompletionCase.EmptyCompletionAfter, body);
        }

        private void AddIf(ICSharpTreeNode node, CompletionCase completionCase, IList<IStatement> body)
        {
            if (IsTargetMatch(node, completionCase))
            {
                body.Add(EmptyCompletionExpression);
            }
        }

        public override void VisitUsingStatement(IUsingStatement block, IList<IStatement> body)
        {
            AddIf(block, CompletionCase.EmptyCompletionBefore, body);

            var usingBlock = new UsingBlock();

            IVariableReference varRef = new VariableReference();

            // case 1: variable declarations
            if (block.VariableDeclarations.Any())
            {
                var decl = block.VariableDeclarations[0];
                decl.Accept(this, body);
                varRef = new VariableReference {Identifier = decl.DeclaredName};
            }
            // case 2: expressions (var refs, method calls ...)
            else if (block.Expressions.Any())
            {
                var expr = block.Expressions[0];
                varRef = _exprVisitor.ToVariableRef(expr, body);
            }

            usingBlock.Reference = varRef;

            var bodyAsIBlock = block.Body as IBlock;
            if (bodyAsIBlock != null && !bodyAsIBlock.Statements.Any() && IsTargetMatch(block, CompletionCase.InBody))
            {
                usingBlock.Body.Add(new ExpressionStatement {Expression = new CompletionExpression()});
            }
            else
            {
                block.Body.Accept(this, usingBlock.Body);
            }

            body.Add(usingBlock);

            AddIf(block, CompletionCase.EmptyCompletionAfter, body);
        }

        public override void VisitSwitchStatement(ISwitchStatement block, IList<IStatement> body)
        {
            AddIf(block, CompletionCase.EmptyCompletionBefore, body);

            var switchBlock = new SwitchBlock {Reference = _exprVisitor.ToVariableRef(block.Condition, body)};

            foreach (var section in block.Sections)
            {
                IKaVEList<IStatement> currentSection = null;

                foreach (var label in section.CaseLabels)
                {
                    currentSection = new KaVEList<IStatement>();
                    if (label.IsDefault)
                    {
                        switchBlock.DefaultSection = currentSection;
                    }
                    else
                    {
                        switchBlock.Sections.Add(
                            new CaseBlock
                            {
                                Label = _exprVisitor.ToSimpleExpression(label.ValueExpression, body),
                                Body = currentSection
                            });
                    }
                    AddIf(label, CompletionCase.InBody, currentSection);
                }

                AddIf(section, CompletionCase.InBody, currentSection);
                foreach (var statement in section.Statements)
                {
                    statement.Accept(this, currentSection);
                }

                switch (1)
                {
                    case 1*2:
                    case 0:
                        break;
                }
            }

            body.Add(switchBlock);

            AddIf(block, CompletionCase.EmptyCompletionAfter, body);
        }

        public override void VisitUncheckedStatement(IUncheckedStatement block, IList<IStatement> body)
        {
            AddIf(block, CompletionCase.EmptyCompletionBefore, body);

            var uncheckedBlock = new UncheckedBlock();
            AddIf(block, CompletionCase.InBody, uncheckedBlock.Body);
            block.Body.Accept(this, uncheckedBlock.Body);
            body.Add(uncheckedBlock);

            AddIf(block, CompletionCase.EmptyCompletionAfter, body);
        }

        public override void VisitBlock(IBlock block, IList<IStatement> body)
        {
            // TODO NameUpdate: changed another helper to overriding this method, check if Null check is really necessary now
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (block == null)
            {
                return;
            }
            // TODO NameUpdate: untested addition
            AddIf(block, CompletionCase.EmptyCompletionBefore, body);
            // TODO NameUpdate: untested addition
            AddIf(block, CompletionCase.InBody, body);

            foreach (var stmt in block.Statements)
            {
                Execute.AndSupressExceptions(
                    delegate { stmt.Accept(this, body); });
            }
            // TODO NameUpdate: untested addition
            AddIf(block, CompletionCase.EmptyCompletionAfter, body);
        }

        #endregion
    }
}