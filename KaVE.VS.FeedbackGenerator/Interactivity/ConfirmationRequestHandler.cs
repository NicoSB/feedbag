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

using System.Windows;

namespace KaVE.VS.FeedbackGenerator.Interactivity
{
    public class ConfirmationRequestHandler
    {
        private readonly Window _window;

        public ConfirmationRequestHandler(DependencyObject parent)
        {
            _window = Window.GetWindow(parent);
        }

        public void Handle(object sender, InteractionRequestedEventArgs<Confirmation> args)
        {
            var confirmation = args.Notification;
            var answer = _window == null
                ? MessageBox.Show(confirmation.Message, confirmation.Caption, MessageBoxButton.YesNo)
                : MessageBox.Show(_window, confirmation.Message, confirmation.Caption, MessageBoxButton.YesNo);
            confirmation.Confirmed = answer == MessageBoxResult.Yes;
            args.Callback();
        }
    }
}