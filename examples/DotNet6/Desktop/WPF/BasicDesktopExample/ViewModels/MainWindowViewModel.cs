﻿using g2.Animation.Core.Timing;

namespace g2.Animation.TestWPFDesktopApp.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    private DateAndTimeViewModel lbl_DateAndTime;
    private FPSUIViewModel lbl_FPSCounterUI;
    private FPSCounter lbl_FPSCounterUpdate;

    public MainWindowViewModel() // ToDo: Inject ViewModels? Init in code behind and pass down everthing? Tooo many Viewmodels?
    {
        lbl_DateAndTime = new();
        lbl_FPSCounterUI = new();
        lbl_FPSCounterUpdate = new();

    }

    public DateAndTimeViewModel Lbl_DateAndTime
    {
        get
        {
            return lbl_DateAndTime;
        }

        set
        {
            if (lbl_DateAndTime != value)
            {
                lbl_DateAndTime = value;
                NotifyPropertyChanged();
            }
        }
    }

    public FPSUIViewModel Lbl_FPSCounterUI
    {
        get
        {
            return lbl_FPSCounterUI;
        }

        set
        {
            if (lbl_FPSCounterUI != value)
            {
                lbl_FPSCounterUI = value;
                NotifyPropertyChanged();
            }
        }
    }

    public FPSCounter Lbl_FPSCounterUpdate
    {
        get
        {
            return lbl_FPSCounterUpdate;
        }

        set
        {
            if (lbl_FPSCounterUpdate != value)
            {
                lbl_FPSCounterUpdate = value;
                NotifyPropertyChanged();
            }
        }
    }

    public void Update()
    {
        lbl_DateAndTime.UpdateContent();
        lbl_FPSCounterUI.UpdateContent();
        //lbl_FPSCounterUpdate.UpdateContent();
    }
}
