using DanielSitarz.MyLog;
using Machine.Enums;
using Machine.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Machine.Components
{
    ///<summary>
    /// Brew Module handles brewing coffees from the CoffeeMakeModels. Raports brew success and status change.
    ///</summary>
    public abstract class BrewModule : MonoBehaviour, ITurnable
    {
        public bool debug;

        [SerializeField, Tooltip("Amount in grams used by this brew module.")]
        protected float coffeeAmount = 12;

        public Status Status { get { return status; } }
        protected Status status = Status.Off;

        [HideInInspector]
        public UnityEvent OnBrewSuccess;
        [Space()]
        public StatusEvent OnStatusChange;

        private Coroutine brewingProcess;

        public abstract void StartBrew(CoffeeMakeModel currentCoffeeModel);
        public abstract void StopBrewing();

        public virtual void TurnOn()
        {
            SetStatus(Status.Idle);

            MyLog.TryLog(this, "Turn on", debug);
        }

        public virtual void TurnOff()
        {
            StopBrewing();

            SetStatus(Status.Off);

            MyLog.TryLog(this, "Turn off", debug);
        }

        protected virtual void SetStatus(Status newStatus)
        {
            status = newStatus;
            OnStatusChange.Invoke(status);

            MyLog.TryLog(this, $"Changed status - {newStatus}", debug);
        }
    }
}