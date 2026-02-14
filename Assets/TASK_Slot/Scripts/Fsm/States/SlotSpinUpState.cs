using AxGrid;
using AxGrid.FSM;
using Rusleo.TestTask.Core;
using UnityEngine;

namespace Rusleo.TestTask.Fsm.States
{
    [State(SlotStates.SpinUp)]
    public class SlotSpinUpState : FSMState
    {
        [Enter]
        private void Enter()
        {
            Settings.Model.Set(SlotKeys.SlotState, SlotStates.SpinUp);
            Settings.Model.Set(SlotKeys.BtnStartEnable, false);
            Settings.Model.Set(SlotKeys.BtnStopEnable, false);

            Settings.Model.EventManager.Invoke(SlotEvents.SlotStart);
        }

        [One(0.35f)]
        private void GoSpinning()
        {
            Parent.Change(SlotStates.Spinning);
        }
    }
}