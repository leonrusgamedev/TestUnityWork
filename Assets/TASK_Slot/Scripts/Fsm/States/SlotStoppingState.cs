using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;
using Rusleo.TestTask.Core;
using Rusleo.TestTask.Utils;

namespace Rusleo.TestTask.Fsm.States
{
    [State(SlotStates.Stopping)]
    public class SlotStoppingState : FSMState
    {
        [Enter]
        private void Enter()
        {
            Settings.Model.Set(SlotKeys.SlotState, SlotStates.Stopping);
            Settings.Model.Set(SlotKeys.BtnStartEnable, false);
            Settings.Model.Set(SlotKeys.BtnStopEnable, false);
            Settings.Model.EventManager.AddAction(SlotEvents.SlotResult, OnResult);

            var slots = (SlotConfig)Settings.Model.Get(SlotKeys.SlotConfig);
            if (slots == null)
            {
                Log.Error("Slot config is empty");
                Settings.Model.EventManager.Invoke(SlotEvents.SlotStopping);
                return;
            }

            // Settings.Model.EventManager.Invoke(SlotEvents.SlotStopping,
            //     WeightRandomItemUtil.GetRandomItem(slots.Items));
            Settings.Model.EventManager.Invoke(SlotEvents.SlotStopping,slots.Items[0]);
        }
        
        private void OnResult()
        {
            Parent.Change(SlotStates.Result);
            Log.Info($"Slot {SlotStates.Result} is now stopped.");
        }
    }
}