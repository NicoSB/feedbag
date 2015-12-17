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

using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using JetBrains.Application;
using JetBrains.DataFlow;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using KaVE.Commons.Model.Events;
using KaVE.Commons.Model.Names;
using KaVE.Commons.Model.Names.CSharp;
using KaVE.Commons.Utils;
using KaVE.Commons.Utils.IO;
using KaVE.JetBrains.Annotations;
using KaVE.VS.FeedbackGenerator.MessageBus;

namespace KaVE.VS.FeedbackGenerator.Generators.Navigation
{
    [SolutionComponent]
    public class KeyboardClickNavigationEventGenerator : NavigationEventGeneratorBase
    {
        private readonly INavigationUtils _navigationUtils;
        private readonly IKeyUtil _keyUtil;

        [CanBeNull]
        private IName _oldLocation;

        public KeyboardClickNavigationEventGenerator([NotNull] IRSEnv env,
            [NotNull] IMessageBus messageBus,
            [NotNull] IDateUtils dateUtils,
            [NotNull] ITextControlManager textControlManager,
            [NotNull] INavigationUtils navigationUtils,
            [NotNull] Lifetime lifetime,
            [NotNull] IKeyUtil keyUtil) : base(env, messageBus, dateUtils, textControlManager, lifetime)
        {
            _navigationUtils = navigationUtils;
            _keyUtil = keyUtil;
        }

        public void OnClick(TextControlMouseEventArgs args)
        {
            var newLocation = _navigationUtils.GetLocation(args.TextControl);

            if (_oldLocation != null && !Equals(newLocation, _oldLocation))
            {
                FireNavigationEvent(
                    Name.UnknownName,
                    newLocation,
                    NavigationType.Click,
                    IDEEvent.Trigger.Click);
            }

            _oldLocation = newLocation;
        }

        public void OnKeyPress(EventArgs<ITextControl> args)
        {
            var newLocation = _navigationUtils.GetLocation(args.Value);

            if (NavigationKeys.Any(key => _keyUtil.IsPressed(key)))
            {
                if (_oldLocation != null && !Equals(newLocation, _oldLocation))
                {
                    FireNavigationEvent(
                        Name.UnknownName,
                        newLocation,
                        NavigationType.Keyboard,
                        IDEEvent.Trigger.Typing);
                }
            }

            _oldLocation = newLocation;
        }

        protected override void Advice(ITextControlWindow textControlWindow, Lifetime lifetime)
        {
            textControlWindow.MouseDown.Advise(lifetime, OnClick);
            textControlWindow.Keyboard.Advise(lifetime, OnKeyPress);
        }

        #region keyboard helpers

        private static readonly ISet<Key> NavigationKeys = new HashSet<Key>
        {
            Key.Up,
            Key.Down,
            Key.Left,
            Key.Right,
            Key.PageUp,
            Key.PageDown
        };

        public interface IKeyUtil
        {
            bool IsPressed(Key key);
        }

        [ShellComponent]
        public class KeyUtil : IKeyUtil
        {
            public bool IsPressed(Key key)
            {
                return key.IsPressed();
            }
        }

        #endregion
    }
}