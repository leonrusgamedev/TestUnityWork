using AxGrid;
using AxGrid.FSM;
using Rusleo.TestTask.Core;

namespace Rusleo.TestTask.Fsm.States
{
    [State(SlotStates.Result)]
    public class SlotResultState : FSMState
    {
        [Enter]
        private void Enter()
        {
            Settings.Model.Set(SlotKeys.SlotState, SlotStates.Result);
            Settings.Model.Set(SlotKeys.BtnStartEnable, true);
            Settings.Model.Set(SlotKeys.BtnStopEnable, false);
        }

        [One(0.8f)]
        private void BackToIdle()
        {
            Parent.Change(SlotStates.Idle);
        }
    }
}