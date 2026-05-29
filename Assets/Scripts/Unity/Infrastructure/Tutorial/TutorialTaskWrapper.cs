using System;
using System.Threading.Tasks;
using Unity.Infrastructure.ResourceManager;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Unity.Infrastructure.Tutorial
{
    public class TutorialTaskWrapper : MonoBehaviour
    {
        public event Action<TutorialTaskWrapper> OnComplete;
        
        
        [SerializeField, HideInInspector] 
        private ScriptMachine _scriptMachine;
        [SerializeField, HideInInspector] 
        private StateMachine _stateMachine;

        private EventHook _completeHook;
        private Action<string> _completeHandler;
        private Object _graph;
        private DiContainer _curContext;
        public TutorialTask TutorialTask { get; private set; }

        private void OnValidate()
        {
            _scriptMachine = GetComponent<ScriptMachine>();
            _stateMachine = GetComponent<StateMachine>();
        }

        public async Task StartTask(TutorialTask tutorialTask, DiContainer curContext)
        {
            SetContext(curContext);
            TutorialTask = tutorialTask;
            _curContext = curContext;
            var graphAsset = await tutorialTask.TaskGraph.LoadAssetReference<Object>(tutorialTask.TaskGraph.AssetGUID);
            _graph = Instantiate(graphAsset);//TODO: должно быть перенесено в scene context
            _completeHook = new EventHook(TutorialTask.TASK_COMPLETE_EVENT);
            _completeHandler = new Action<string>(OnTutorialCompleted);
            EventBus.Register<string>(_completeHook, OnTutorialCompleted);
            
            switch (_graph)
            {
                case ScriptGraphAsset scriptGraph:
                    scriptGraph.graph.title ??= ((IMacro)_graph).graph.title;
                    _scriptMachine.nest.SwitchToMacro(scriptGraph);
                    Destroy(_stateMachine);
                    break;
                case StateGraphAsset stateGraph:
                    stateGraph.graph.title ??=  ((IMacro)_graph).graph.title;
                    _stateMachine.nest.SwitchToMacro(stateGraph);
                    Destroy(_scriptMachine);
                    break;
                default:
                    Debug.LogError("unknown graph type");
                    break;
            }
            SetContext(_curContext);
            Debug.Log($"Tutorial Task started: {_graph.name}");
        }

        public void SetContext(DiContainer curContext)
        {
            Variables.Object(gameObject).Set("context", curContext);
        }
        
        private void OnTutorialCompleted(string taskName)
        {
            if (_graph == null)
            {
                return;
            }
            if (taskName != ((IMacro)_graph).graph.title) 
            {
                return;
            }
            
            AddressableExtention.ReleaseTag(TutorialTask.TaskGraph.AssetGUID);
            EventBus.Unregister(_completeHook, _completeHandler);
            OnComplete?.Invoke(this);
        }
    }
}