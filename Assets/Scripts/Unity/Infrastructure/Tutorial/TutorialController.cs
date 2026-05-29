using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application.DataStorage;
using Core.Application.DataStorage.StorageItems;
using Core.Application.Interfaces;
using Unity.Infrastructure.ResourceManager;
using Unity.Presentation.Components;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Unity.Infrastructure.Tutorial
{
    public class TutorialController : MonoBehaviour, ITutorialOverlayController, IBootstrapStep
    {
        [SerializeField]
        private AssetReference _taskWrapperPrefab;
        [SerializeField]
        private List<TutorialTasksChain> _tutorialChains;
        [SerializeField]
        private OverlayComponent _overlay;
        
        [Inject] 
        private IDataStorage _dataStorage;
        
        DiContainer _curSceneContext;
        
        public TutorialStorageData StorageData => _dataStorage.TutorialStorage;

        public List<TutorialTasksChain> TutorialChains => _tutorialChains;

        private readonly Dictionary<TutorialTask, TutorialTaskWrapper> _taskToWrapper = new();
        
        private readonly List<TutorialTasksChain> _activeChains = new();
        
        public void ShowOverlay(float alpha = -1) => _overlay.Show(alpha);
        public void HideOverlay() => _overlay.Hide();

        private bool _initialized = false;
        
        public async Task Initialize()
        {
            await TryStartChains();
            Debug.Log($"{this.GetType().Name} Initialized");
        }
        
        public void SetSceneContext(DiContainer context)
        {
            _curSceneContext = context;
            foreach (var task in _taskToWrapper)
            {
                task.Value.SetContext(context);
            }
        }

        private async Task TryStartChains()
        {
            if (TutorialChains.Count == 0)
            {
                return;
            }

            foreach (var chain in TutorialChains)
            {
                
                await TryStartNextTask(chain);
            }
            
        }

        private async Task TryStartNextTask(TutorialTasksChain chain)
        {
            if (IsChainComplete(chain))
            {
                return;
            }
            
            if (_activeChains.Contains(chain))
            {
                return;
            }
            
            _activeChains.Add(chain);
            var progress = StorageData.GetChainProgress(chain.ID);
            var task = chain.Tasks[progress];
            await StartTutorialTask(task);
            
        }
        

        private bool IsChainComplete(TutorialTasksChain chain)
        {
            return StorageData.GetChainProgress(chain.ID) >= chain.Tasks.Count; 
        }
        
        private async Task StartTutorialTask(TutorialTask task)
        {
            if (_curSceneContext == null)
            {
                return;
            }
            var taskPrefab = await _taskWrapperPrefab.LoadAssetReference<GameObject>(_taskWrapperPrefab.AssetGUID);
            var taskWrapper = _curSceneContext.InstantiatePrefabForComponent<TutorialTaskWrapper>(taskPrefab, transform);
            taskWrapper.OnComplete += OnTutorialTaskCompleted;
            _taskToWrapper.Add(task, taskWrapper);
            await taskWrapper.StartTask(task, _curSceneContext);
        }

        private bool TryGetChainByTask(TutorialTask task, out TutorialTasksChain chain)
        {
            chain = _activeChains.FirstOrDefault(chain => chain.Tasks.Contains(task));
            return chain != null;
        }
        
        private void OnTutorialTaskCompleted(TutorialTaskWrapper taskWrapper)
        {
            taskWrapper.OnComplete -= OnTutorialTaskCompleted;
            var task = taskWrapper.TutorialTask;
            _taskToWrapper.Remove(task);
            AddressableExtention.ReleaseTag(_taskWrapperPrefab.AssetGUID);
            if (TryGetChainByTask(task, out var chain))
            {
                StorageData.SetChainProgress(chain.ID, chain.Tasks.IndexOf(task)+1);
                _activeChains.Remove(chain);
            }
            Destroy(taskWrapper.gameObject);
            TryStartNextTask(chain);
        }
       
    }
}