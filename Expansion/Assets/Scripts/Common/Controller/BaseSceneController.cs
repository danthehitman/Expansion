using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common.Controller
{
    public abstract class BaseSceneController : MonoBehaviour
    {
        protected List<ILifecycleEventAware> lifecycleEventAwares = new List<ILifecycleEventAware>();

        protected virtual void Awake()
        {
            foreach (var aware in lifecycleEventAwares) aware.Awake();
        }

        protected virtual void Start()
        {
            foreach (var aware in lifecycleEventAwares) aware.Start();
        }

        protected virtual void Update()
        {
            foreach (var aware in lifecycleEventAwares) aware.Update();
        }

        protected virtual void FixedUpdate()
        {
            foreach (var aware in lifecycleEventAwares) aware.FixedUpdate();
        }

        protected virtual void OnEnable()
        {
            foreach (var aware in lifecycleEventAwares) aware.OnEnable();
        }

        protected virtual void OnDisable()
        {
            foreach (var aware in lifecycleEventAwares) aware.OnDisable();
        }

        protected virtual void OnDestroy()
        {
            foreach (var aware in lifecycleEventAwares) aware.OnDestroy();
        }
    }
}
