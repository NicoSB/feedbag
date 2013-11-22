﻿using System;
using System.Reflection;
using EnvDTE;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.UI.BulbMenu;
using KaVE.VsFeedbackGenerator.Generators.ReSharper;
using KaVE.VsFeedbackGenerator.MessageBus;

namespace KaVE.VsFeedbackGenerator
{
    internal static class BulbItemInstrumentationExtensions
    {
        private static readonly MethodInfo MyExecutableProxiBulbActionSetter =
            typeof (IntentionAction.MyExecutableProxi).GetProperty("BulbAction").GetSetMethod(true);

        private static readonly FieldInfo ExecutableItemActionField =
            typeof (ExecutableItem).GetField("myAction", BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Wraps the <see cref="IBulbAction"/> contained in the passed proxy in a <see cref="EventGeneratingBulbActionProxy"/>.
        /// </summary>
        public static void WrapBulbAction(this IntentionAction.MyExecutableProxi proxy, DTE dte, IMessageBus messageBus)
        {
            var originalBulbAction = proxy.BulbAction;
            var bulbActionWrapper = new EventGeneratingBulbActionProxy(originalBulbAction, dte, messageBus);
            MyExecutableProxiBulbActionSetter.Invoke(proxy, new object[] {bulbActionWrapper});
        }

        public static void WrapBulbAction(this ExecutableItem executable, DTE dte, IMessageBus messageBus)
        {
            var originalAction = (Action) ExecutableItemActionField.GetValue(executable);
            var wrapper = new EventGeneratingActionWrapper(originalAction, dte, messageBus);
            var newAction = (Action) (wrapper.Execute);
            ExecutableItemActionField.SetValue(executable, newAction);
        }
    }
}