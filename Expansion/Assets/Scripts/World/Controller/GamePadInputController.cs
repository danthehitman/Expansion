//using UnityEngine;

//namespace Assets.Scripts.Controller
//{
//    public class GamePadInputController : OldeInputController
//    {
//        private Vector3 MouseLastPosition;
//        private int LastWorldX;
//        private int LastWorldY;
//        private bool isPanning = false;
//        private float panningThreshold = .015f;
//        private Vector3 panningMouseStart = Vector3.zero;

//        public GamePadInputController(PlayerControls playerControls) : base(playerControls)
//        {
//        }

//        // Use this for initialization
//        public override void Start()
//        {
//            base.Start();
//        }

//        // Update is called once per frame
//        public override void Update()
//        {
//            base.Update();

//            HandleControllerUpdates();
//        }

//        private void HandleControllerUpdates()
//        {
//            //var speed = 1;
//            //var x = Input.GetAxis("Pitch") * speed * Time.deltaTime;
//            //var y = Input.GetAxis("Roll") * speed * Time.deltaTime;
//            //var newPosition = new Vector2(x, y);
//            //Mouse.current.WarpCursorPosition(newPosition);


//            //float moveSpeed = 10;
//            ////Define the speed at which the object moves.

//            //float horizontalInput = Input.GetAxis("Horizontal");
//            ////Get the value of the Horizontal input axis.

//            //float verticalInput = Input.GetAxis("Vertical");
//            ////Get the value of the Vertical input axis.

//            //Camera.main.transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * moveSpeed * Time.deltaTime);
//            ////Move the object to XYZ coordinates defined as horizontalInput, 0, and verticalInput respectively.
//        }
//    }
//}
