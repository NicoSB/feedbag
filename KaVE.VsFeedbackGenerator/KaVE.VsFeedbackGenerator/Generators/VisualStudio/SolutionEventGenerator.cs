﻿using EnvDTE;
using JetBrains.Application.Components;
using JetBrains.ProjectModel;
using KaVE.Model.Events.VisualStudio;
using KaVE.Model.Names.VisualStudio;
using KaVE.VsFeedbackGenerator.MessageBus;
using KaVE.VsFeedbackGenerator.Utils.Names;
using KaVE.VsFeedbackGenerator.VsIntegration;

namespace KaVE.VsFeedbackGenerator.Generators.VisualStudio
{
    [SolutionComponent(ProgramConfigurations.VS_ADDIN)]
    internal class SolutionEventGenerator : AbstractEventGenerator
    {
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
        private readonly SolutionEvents _solutionEvents;
        private readonly ProjectItemsEvents _solutionItemsEvents;
        private readonly ProjectItemsEvents _projectItemsEvents;
        private readonly SelectionEvents _selectionEvents;
        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable

        public SolutionEventGenerator(IIDESession session, IMessageBus messageBus)
            : base(session, messageBus)
        {
            // SolutionComponents are created after the solution is opened, i.e., after SolutionEvents.Opened is fired.
            _solutionEvents_Opened();

            _solutionEvents = DTE.Events.SolutionEvents;
            _solutionEvents.ProjectAdded += _solutionEvents_ProjectAdded;
            _solutionEvents.ProjectRenamed += _solutionEvents_ProjectRenamed;
            _solutionEvents.ProjectRemoved += _solutionEvents_ProjectRemoved;
            _solutionEvents.Renamed += _solutionEvents_Renamed;
            _solutionEvents.BeforeClosing += _solutionEvents_BeforeClosing;

            _solutionItemsEvents = DTE.Events.SolutionItemsEvents;
            _solutionItemsEvents.ItemAdded += _solutionItemsEvents_ItemAdded;
            _solutionItemsEvents.ItemRenamed += _solutionItemsEvents_ItemRenamed;
            _solutionItemsEvents.ItemRemoved += _solutionItemsEvents_ItemRemoved;

            _projectItemsEvents = DTE.Events.MiscFilesEvents;
            _projectItemsEvents.ItemAdded += _projectItemsEvents_ItemAdded;
            _projectItemsEvents.ItemRenamed += _projectItemsEvents_ItemRenamed;
            _projectItemsEvents.ItemRemoved += _projectItemsEvents_ItemRemoved;

            _selectionEvents = DTE.Events.SelectionEvents;
            _selectionEvents.OnChange += _selectionEvents_OnChange;
        }

        private void _solutionItemsEvents_ItemAdded(ProjectItem projectItem)
        {
            Fire(SolutionEvent.SolutionAction.AddSolutionItem, VsComponentNameFactory.GetName(projectItem));
        }

        private void _solutionItemsEvents_ItemRenamed(ProjectItem projectItem, string oldName)
        {
            Fire(SolutionEvent.SolutionAction.RenameSolutionItem, VsComponentNameFactory.GetName(projectItem));
        }

        private void _solutionItemsEvents_ItemRemoved(ProjectItem projectItem)
        {
            Fire(SolutionEvent.SolutionAction.RemoveSolutionItem, VsComponentNameFactory.GetName(projectItem));
        }

        private void _solutionEvents_Opened()
        {
            Fire(SolutionEvent.SolutionAction.OpenSolution, VsComponentNameFactory.GetName(DTE.Solution));
        }

        private void _solutionEvents_ProjectAdded(Project project)
        {
            Fire(SolutionEvent.SolutionAction.AddProject, VsComponentNameFactory.GetName(project));
        }

        private void _projectItemsEvents_ItemAdded(ProjectItem projectItem)
        {
            Fire(SolutionEvent.SolutionAction.AddProjectItem, VsComponentNameFactory.GetName(projectItem));
        }

        private void _projectItemsEvents_ItemRenamed(ProjectItem projectItem, string oldName)
        {
            Fire(SolutionEvent.SolutionAction.RenameProjectItem, VsComponentNameFactory.GetName(projectItem));
        }

        private void _projectItemsEvents_ItemRemoved(ProjectItem projectItem)
        {
            Fire(SolutionEvent.SolutionAction.RemoveProjectItem, VsComponentNameFactory.GetName(projectItem));
        }

        private void _solutionEvents_ProjectRenamed(Project project, string oldName)
        {
            Fire(SolutionEvent.SolutionAction.RenameProject, VsComponentNameFactory.GetName(project));
        }

        private void _solutionEvents_ProjectRemoved(Project project)
        {
            Fire(SolutionEvent.SolutionAction.RemoveProject, VsComponentNameFactory.GetName(project));
        }

        private void _solutionEvents_Renamed(string oldName)
        {
            Fire(SolutionEvent.SolutionAction.RenameSolution, VsComponentNameFactory.GetName(DTE.Solution));
        }

        private void _solutionEvents_BeforeClosing()
        {
            Fire(SolutionEvent.SolutionAction.CloseSolution, VsComponentNameFactory.GetName(DTE.Solution));
        }

        private void _selectionEvents_OnChange()
        {
            // TODO fire project item selection event?
            // this method behaves strange... e.g., selection in solution explorer is recognized. Adding to that
            // selection by ctrl+click is recognized. Any further additions to the selection, using ctrl+click, are
            // not recognized.
        }

        private void Fire(SolutionEvent.SolutionAction action, IIDEComponentName target)
        {
            var solutionEvent = Create<SolutionEvent>();
            solutionEvent.Action = action;
            solutionEvent.Target = target;
            FireNow(solutionEvent);
        }
    }
}