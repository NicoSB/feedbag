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

using KaVE.Commons.Model.SSTs.Impl.Blocks;
using KaVE.Commons.Model.SSTs.Impl.Expressions.Simple;
using KaVE.Commons.Model.SSTs.Impl.Statements;
using NUnit.Framework;
using Fix = KaVE.VsFeedbackGenerator.RS8Tests.Analysis.SSTAnalysisTestSuite.SSTAnalysisFixture;

namespace KaVE.VsFeedbackGenerator.RS8Tests.Analysis.SSTAnalysisTestSuite.Statements
{
    internal class BreakStatementAnalysisTest : BaseSSTAnalysisTest
    {
        [Test]
        public void BreakInLoop()
        {
            CompleteInMethod(@"
                while (true)
                {
                    break;
                }
                $
            ");

            AssertBody(
                new WhileLoop
                {
                    Condition = new ConstantValueExpression(),
                    Body =
                    {
                        new BreakStatement(),
                    }
                },
                Fix.EmptyCompletion);
        }

        // TODO: completion cases

        [Test, Ignore]
        public void LonelyBreak()
        {
            // this is a syntax error!
            CompleteInMethod(@"
                break;
                $
            ");

            AssertBody(
                new BreakStatement(),
                Fix.EmptyCompletion);
        }
    }
}