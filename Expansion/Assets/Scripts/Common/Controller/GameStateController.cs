using Assets.Scripts.World.Model;
using UnityEngine;

namespace Assets.Scripts.Test.Controller
{
    public class GameStateController : MonoBehaviour
    {
        public static GameStateController Instance { get; set; }
        public WorldModel World { get; set; }
        public bool HasVisitedTest { get; set; }


        // Start is called before the first frame update
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
        }

        void Start()
        {
        }

        public void OnEnable()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }


    }
}