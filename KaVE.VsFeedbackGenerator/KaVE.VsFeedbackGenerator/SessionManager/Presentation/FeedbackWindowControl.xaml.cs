﻿using System.Windows.Controls;

namespace KaVE.VsFeedbackGenerator.SessionManager.Presentation
{
    /// <summary>
    /// Interaktionslogik für FeedbackWindowControl.xaml
    /// </summary>
    public partial class FeedbackWindowControl : UserControl
    {
        public FeedbackWindowControl(SessionHolder holder)
        {
            InitializeComponent();
            DataContext = holder;
        }
    }
}
