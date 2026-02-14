using AxGrid.FSM;
using Rusleo.TestTask.Fsm.States;

namespace Rusleo.TestTask.Fsm
{
    public static class SlotFsmInstaller
    {
        public static void Install(FSM fsm)
        {
            fsm.Add(new SlotIdleState());
            fsm.Add(new SlotSpinUpState());
            fsm.Add(new SlotSpinningState());
            fsm.Add(new SlotStoppingState());
            fsm.Add(new SlotResultState());
        }
    }
}