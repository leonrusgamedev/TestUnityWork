using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;
using Rusleo.TestTask.Core;

namespace  Rusleo.TestTask.Fsm.States
{
    [State(SlotStates.Idle)]
    public class SlotIdleState : FSMState
    {
        [Enter]
        private void Enter()
        {
            Settings.Model.Set(SlotKeys.SlotState, SlotStates.Idle);
            Settings.Model.Set(SlotKeys.BtnStartEnable, true);
            Settings.Model.Set(SlotKeys.BtnStopEnable, false);
        }

        [Bind("OnBtn")]
        private void OnBtn(string btnName)
        {
            if (btnName != "Start") return;

            Settings.Model.Set(SlotKeys.BtnStartEnable, false);
            Parent.Change(SlotStates.SpinUp);
        }
    }
}