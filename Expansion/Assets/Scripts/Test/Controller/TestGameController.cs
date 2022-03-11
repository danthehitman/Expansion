using Assets.Scripts.Test.View;
using UnityEngine;

namespace Assets.Scripts.Test.Controller
{
    public class TestGameController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var goBackButtonView = new GoBackButtonView(transform);

            GameStateController.Instance.HasVisitedTest = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}