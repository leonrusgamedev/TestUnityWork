using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;
using Rusleo.TestTask.Core;

namespace Rusleo.TestTask.Fsm.States
{
    [State(SlotStates.Spinning)]
    public class SlotSpinningState : FSMState
    {
        private bool _canStop;

        [Enter]
        private void Enter()
        {
            Settings.Model.Set(SlotKeys.SlotState, SlotStates.Spinning);
            Settings.Model.Set(SlotKeys.BtnStartEnable, false);
            Settings.Model.Set(SlotKeys.BtnStopEnable, false);
            _canStop = false;
        }

        [One(2.65f)]
        private void AllowStop()
        {
            _canStop = true;
            Settings.Model.Set(SlotKeys.BtnStopEnable, true);
        }

        [Bind("OnBtn")]
        private void OnBtn(string btnName)
        {
            if (btnName != "Stop") return;
            if (!_canStop) return;

            Settings.Model.Set(SlotKeys.BtnStopEnable, false);
            Parent.Change(SlotStates.Stopping);
        }
    }
}