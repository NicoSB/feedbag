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
 */
using NUnit.Framework;

namespace KaVE.VsFeedbackGenerator.RS8Tests.Analysis.SmokeTests
{
    [TestFixture]
    internal class CodeCompletionWithMissingTokensSmokeTest : BaseTest
    {
        [Test]
        public void MissingClassBodyOpeningBrace()
        {
            CompleteInFile(@"
                class C $
                }");
        }

        [Test]
        public void MissingClassBodyClosingBrace()
        {
            CompleteInFile(@"
                class C
                {
                    $
            ");
        }

        [Test]
        public void DuplicatedClassBodyOpeningBrace()
        {
            CompleteInFile(@"
                class C
                {
                    {$
                }
            ");
        }

        [Test]
        public void DuplicatedClassBodyClosingBrace()
        {
            CompleteInFile(@"
                class C
                {
                    }$
                }
            ");
        }

        [Test]
        public void MissingConditionOpeningBrace()
        {
            CompleteInMethod(@"
                if ($ {}
            ");
        }

        [Test]
        public void MissingConditionClosingBrace()
        {
            CompleteInMethod(@"
                if $) {}
            ");
        }

        [Test]
        public void MissingOperator()
        {
            CompleteInMethod(@"
                if (true fa$) {}
            ");
        }

        [Test]
        public void MissingDeclarationName()
        {
            CompleteInMethod(@"
                var = $;
            ");
        }

        [Test]
        public void MissingDeclarationType()
        {
            CompleteInMethod(@"
                v = $;
            ");
        }

        [Test]
        public void MissingAssignmentOperator()
        {
            CompleteInMethod(@"
                var v $;
            ");
        }

        [Test]
        public void MissingSemicolonInFor()
        {
            CompleteInMethod(@"
                for (;$) {}
            ");
        }

        [Test]
        public void MissingConditionInIf()
        {
            CompleteInMethod(@"
                if ($) {}
            ");
        }

        [Test]
        public void MissingConditionInWhile()
        {
            CompleteInMethod(@"
                while ($) {}
            ");
        }

        [Test]
        public void MissingSemicolon()
        {
            CompleteInMethod(@"
                object o = new object()
                $
            ");
        }
    }
}