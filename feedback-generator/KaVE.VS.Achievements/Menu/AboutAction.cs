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

using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.UI.ActionsRevised;
using KaVE.VS.Achievements.Properties;

namespace KaVE.VS.Achievements.Menu
{
    [Action(Id, "About", Id = 8645780)]
    public class AboutAction : IExecutableAction
    {
        public const string Id = "KaVE.BP.Achievements.About";

        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            // return true or false to enable/disable this action
            return true;
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            MessageBox.Show(
                UIText.AboutAction_Text,
                UIText.AboutAction_Title,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}