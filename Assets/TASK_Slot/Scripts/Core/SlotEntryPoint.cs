using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using Rusleo.TestTask.Fsm;
using UnityEngine;

namespace Rusleo.TestTask.Core
{
    public class SlotEntryPoint : MonoBehaviourExt
    {
        [SerializeField] private SlotConfig config;

        [OnStart]
        private void StartThis()
        {
            Model.Set(SlotKeys.BtnStartEnable, true);
            Model.Set(SlotKeys.BtnStopEnable, false);
            Model.Set(SlotKeys.SlotConfig, config);
            Model.EventManager.Invoke(SlotEvents.SlotConfigReady);

            Settings.Fsm = new FSM();
            SlotFsmInstaller.Install(Settings.Fsm);

            Settings.Fsm.Start(SlotStates.Idle);
        }

        [OnUpdate]
        private void UpdateThis()
        {
            Settings.Fsm?.Update(Time.deltaTime);
        }
    }
}

