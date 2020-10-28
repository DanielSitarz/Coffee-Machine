using System.Collections;
using DanielSitarz.MyLog;
using Machine.Enums;
using UnityEngine;

namespace Machine.Components
{
    ///<summary>
    /// Basic version just takes from coffee beans container and water container. With the settings from CoffeeMakeModel.
    /// Advanced version could take list of commands:
    /// like: Pump water -> Grind coffee -> pump water -> pump milk.
    ///</summary>
    public class BasicBrewModule : BrewModule
    {
        [SerializeField]
        private Supplier coffeeGrinder;
        [SerializeField]
        private Supplier waterPump;

        private Coroutine brewingProcess;

        public override void StartBrew(CoffeeMakeModel coffeeModel)
        {
            if (status != Status.Idle)
            {
                Debug.LogWarning($"Can't brew because machine is {status}");
                return;
            }

            SetStatus(Status.Busy);

            brewingProcess = StartCoroutine(Brew(coffeeModel));
        }

        public override void StopBrewing()
        {
            if (status != Status.Busy || brewingProcess == null) return;

            StopCoroutine(brewingProcess);

            coffeeGrinder.StopProcessing();
            waterPump.StopProcessing();

            SetStatus(Status.Idle);

            MyLog.TryLog(this, $"Stopped brewing", debug);
        }

        private IEnumerator Brew(CoffeeMakeModel coffeeModel)
        {
            MyLog.TryLog(this, $"Brewing...", debug);
            MyLog.TryLog(this, $"{coffeeModel.waterFlowRate}", debug);
            MyLog.TryLog(this, $"{coffeeModel.waterAmount}", debug);

            coffeeGrinder.StartProcessing(coffeeAmount);

            yield return new WaitUntil(() => coffeeGrinder.Status == Status.Idle);

            waterPump.StartProcessing(coffeeModel.waterAmount, coffeeModel.waterFlowRate);

            yield return new WaitUntil(() => waterPump.Status == Status.Idle);

            OnBrewSuccess.Invoke();
            SetStatus(Status.Idle);
        }
    }
}