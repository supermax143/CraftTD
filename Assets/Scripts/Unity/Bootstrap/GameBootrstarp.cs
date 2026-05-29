using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Application.DataStorage;
using Core.Application.DataStorage.StorageItems;
using Core.Application.Interfaces;
using Unity.Infrastructure.Localization;
using Unity.Infrastructure.Purchases;
using Unity.Infrastructure.Tutorial;
using Zenject;

namespace Unity.Bootstrap
{
   internal class GameBootrstarp : IBootstrapProgress, IGameBootstrap
   {
      [Inject] private LocalizationController _localization;
      [Inject] private IDataStorage _dataStorage;
      [Inject] private TutorialController _tutorialController;
      [Inject] private IPurchasesController _purchasesController;
      
      public event Action OnStepStarted;
      public event Action OnInitializationComplete;


      public string CurStepIdent { get; private set; } = "";
      public float Progress { get; private set; } = 0;
      
      
      private readonly Queue<BootstrapStepWrapper> _steps = new();
      
      
      public async Task Initialize()
      {
         AddSteps();

         int index = 1;
         while (_steps.Count > 0)
         {
            var step = _steps.Dequeue();
            CurStepIdent = step.StepIdent;
            OnStepStarted?.Invoke();
            await step.Execute();
            Progress = (float)index / _steps.Count;
            index++;
         }
         
         OnInitializationComplete?.Invoke();
      }

     
      private void AddSteps()
      {
           _steps.Enqueue(new BootstrapStepWrapper(_localization, "init_localization"));
           _steps.Enqueue(new BootstrapStepWrapper(_dataStorage, "init_data_storage"));
           _steps.Enqueue(new BootstrapStepWrapper(_purchasesController, "init_purchases" ));
           _steps.Enqueue(new BootstrapStepWrapper(_tutorialController, "init_tutorial"));
      }
      
      
   }
}