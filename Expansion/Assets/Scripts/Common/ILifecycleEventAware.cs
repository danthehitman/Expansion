namespace Assets.Scripts.Common
{
    public interface ILifecycleEventAware
    {
        public void Awake();
        public void Start();
        public void Update();
        public void FixedUpdate();
        public void OnEnable();
        public void OnDisable();
        public void OnDestroy();
    }
}
