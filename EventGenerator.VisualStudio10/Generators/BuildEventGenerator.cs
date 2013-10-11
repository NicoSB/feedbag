﻿using System;
using System.ComponentModel.Composition;
using CodeCompletion.Utils.Assertion;
using EnvDTE;
using KAVE.EventGenerator_VisualStudio10.Model;

namespace KAVE.EventGenerator_VisualStudio10.Generators
{
    internal class BuildEventGenerator : VisualStudioEventGenerator
    {
        private BuildEvents _buildEvents;
        private BuildEvent _currentEvent;
        private BuildTarget _currentTarget;

        public BuildEventGenerator(DTE dte) : base(dte) {}

        public override void Initialize()
        {
            _buildEvents = DTEEvents.BuildEvents;
            _buildEvents.OnBuildBegin += _buildEvents_OnBuildBegin;
            _buildEvents.OnBuildDone += _buildEvents_OnBuildDone;
            _buildEvents.OnBuildProjConfigBegin += _buildEvents_OnBuildProjConfigBegin;
            _buildEvents.OnBuildProjConfigDone += _buildEvents_OnBuildProjConfigDone;
        }

        /// <summary>
        /// Invoked when a user starts a build (clean, rebuild, ...) of a solution, project, or batch build.
        /// </summary>
        void _buildEvents_OnBuildBegin(vsBuildScope scope, vsBuildAction action)
        {
            Asserts.Null(_currentEvent, "another build is running.");
            _currentEvent = Create<BuildEvent>();
            _currentEvent.Scope = scope.ToString();
            _currentEvent.Action = action.ToString();
        }

        /// <summary>
        /// Called for each combination of project and configuration that is part of the build.
        /// </summary>
        void _buildEvents_OnBuildProjConfigBegin(string project, string projectConfig, string platform, string solutionConfig)
        {
            Asserts.Null(_currentTarget, "another build target is currently being processed.");
            _currentTarget = new BuildTarget
            {
                Project = project,
                ProjectConfiguration = projectConfig,
                Platform = platform,
                SolutionConfiguration = solutionConfig,
                StartedAt = DateTime.Now
            };
        }

        void _buildEvents_OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {
            Asserts.NotNull(_currentTarget, "no build-target processing has been started.");
            _currentTarget.FinishedAt = DateTime.Now;
            _currentTarget.Successful = success;
            _currentEvent.Targets.Add(_currentTarget);
            _currentTarget = null;
        }

        void _buildEvents_OnBuildDone(vsBuildScope scope, vsBuildAction action)
        {
            Asserts.NotNull(_currentEvent, "no build processing has been started");
            Fire(_currentEvent);
            _currentEvent = null;
        }
    }
}
