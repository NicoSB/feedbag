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

using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.TestFramework;
using JetBrains.TestFramework.Application.Zones;
using NUnit.Framework;

[assembly: RequiresSTA]

namespace KaVE.ReSharper.Commons.Tests_Integration.Properties
{
    /// <summary>
    ///     Test environment. Must be in the global namespace.
    /// </summary>
    [SetUpFixture]
    // ReSharper disable once CheckNamespace
    // ReSharper disable once InconsistentNaming
    public class AssemblyInfo : TestEnvironmentAssembly<VsFeedbackGeneratorRS8Zone>
    {
        // TODO RS9
        /*
        /// <summary>
        ///     Gets the assemblies to load into test environment.
        ///     Should include all assemblies which contain components.
        /// </summary>
        private static IEnumerable<Assembly> GetAssembliesToLoad()
        {
            // Test assembly
            yield return Assembly.GetExecutingAssembly();

            yield return typeof (AboutAction).Assembly;
        }

        public override void SetUp()
        {
            CodeCompletionContextAnalysisTrigger.Disabled = true;
            base.SetUp();
            ReentrancyGuard.Current.Execute(
                "LoadAssemblies",
                () => Shell.Instance.GetComponent<AssemblyManager>().LoadAssemblies(
                    GetType().Name,
                    GetAssembliesToLoad()));
        }

        public override void TearDown()
        {
            ReentrancyGuard.Current.Execute(
                "UnloadAssemblies",
                () => Shell.Instance.GetComponent<AssemblyManager>().UnloadAssemblies(
                    GetType().Name,
                    GetAssembliesToLoad()));
            base.TearDown();
        }
         * */
    }

    // TODO RS9: zones?
    [ZoneDefinition]
    public class VsFeedbackGeneratorRS8Zone : ITestsZone {}
}