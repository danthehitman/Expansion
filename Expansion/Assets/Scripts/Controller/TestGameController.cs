using UnityEngine;

public class TestGameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameStateController.Instance.HasVisitedTest = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
