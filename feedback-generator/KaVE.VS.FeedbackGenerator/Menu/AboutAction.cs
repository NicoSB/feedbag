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

using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.UI.ActionsRevised;
using KaVE.Commons.Utils;
using KaVE.RS.Commons.Utils;
using KaVE.VS.FeedbackGenerator.UserControls.AboutWindow;

namespace KaVE.VS.FeedbackGenerator.Menu
{
    [Action(Id, "About KaVE", Id = 21394587)]
    public class AboutAction : IExecutableAction
    {
        public const string Id = "KaVE.VsFeedbackGenerator.About";

        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            // return true or false to enable/disable this action
            return true;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            var versionUtil = Registry.GetComponent<VersionUtil>();

            new AboutWindowControl(versionUtil).Show();
        }
    }
}