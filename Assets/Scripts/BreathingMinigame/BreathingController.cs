using Controls;
using UnityEngine;
using UnityEngine.UI;

namespace BreathingMinigame
{
    public class CircleController : MonoBehaviour
    {
        private PlayerPanicControl playerPanicControl;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField, Range(0.0f, 100.0f)] private float _stressReduction = 50.0f;
        public float durationTime;
        private float _speedOfScale = 6f;
        [SerializeField] private Vector3 maxScale = new Vector3(25f, 25f, 25f);
        [SerializeField] private Vector3 minScale = new Vector3(1f, 1f, 1f);

        private Image MyImage;
        public KeyCode key;


        [SerializeField] private GameObject _minigameCanvas;
        [SerializeField] private GameObject _otherUICanvas;

        [Header("Colors")]
        public Color defaultColor;

        public Color pressedColor;
        public Color outOfRangeColor;
        public Color perfectRangeColor;
        private Color currentColor;

        public bool isPerfectRange { get; private set; }

        public bool outOfRange { get; private set; }

        public bool inRange { get; private set; }

        public float timerTime;

        private float CalculateDurationTime()
        {
            switch (playerPanicControl.panicLevel)
            {
                case < 20:
                    return 2f;
                case < 40:
                    return 4f;
                case < 60:
                    return 5f;
                case < 80:
                    return 6f;
                default:
                    return 8f;
            }

        }

        private void Awake()
        {
            MyImage = GetComponent<Image>();
            playerPanicControl = PlayerPanicControl.instance;
        }

        private void OnEnable()
        {
            playerPanicControl.isPaused = true;
            durationTime = CalculateDurationTime();
            timerTime = durationTime;
            transform.localScale = Vector3.one;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(key))
            {
                if (transform.localScale.x < maxScale.x && transform.localScale.y < maxScale.y
                    && transform.localScale.z < maxScale.z)
                {
                    //changes the collor and scaling the circle
                    currentColor = pressedColor;
                    transform.localScale += new Vector3(_speedOfScale, _speedOfScale, _speedOfScale) * Time.deltaTime;
                }

            }
            else
            {
                //changes the collor and reducing scale of the circle when its not being pressed
                //makes a check for the minimum scale before reducing it
                currentColor = defaultColor;
                if (transform.localScale.x > minScale.x && transform.localScale.y > minScale.y &&
                    transform.localScale.z > minScale.z)
                {
                    transform.localScale -= new Vector3(_speedOfScale, _speedOfScale, _speedOfScale) * Time.deltaTime;
                }
            }
            //changes the color of the circle when its perfect or out of range 
            if (isPerfectRange)
            {
                currentColor = perfectRangeColor;
            }
            else if (outOfRange)
            {
                currentColor = outOfRangeColor;
            }

            //assigning a current color
            MyImage.color = currentColor;

            if (!isPerfectRange)
            {
                if (timerTime < durationTime)
                {
                    timerTime += Time.deltaTime * 2;
                }
            }

        }

        public void Play()
        {
            _otherUICanvas.SetActive(false);
            _minigameCanvas.SetActive(true);
            _playerMovement.CanMove = false;
            
        }

        /// <summary>
        /// checks for the tag of gameObject that is colliding with the circle and sets the bools accordingly
        /// </summary>
        /// <param name="collision">object that is colliding with the circle</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("PerfectRange"))
            {
                isPerfectRange = true;
            }
            else if (collision.gameObject.CompareTag("InRange"))
            {
                inRange = true;
                outOfRange = false;
            }
        }

        /// <summary>
        /// checks for the tag of gameObject that is colliding with the circle and makes the timer go down or up accordingly
        /// </summary>
        /// <param name="collision">object that is colliding with the circle</param>
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("PerfectRange"))
            {
                if (timerTime <= 0)
                {
                    End();
                }
                timerTime -= Time.deltaTime;
            }


        }

        /// <summary>
        /// checks for the tag of gameObject that is colliding with the circle and sets the bools accordingly
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("PerfectRange"))
            {
                isPerfectRange = false;
            }
            else if (collision.gameObject.CompareTag("InRange"))
            {
                outOfRange = true;
                inRange = false;
            }

        }

        private void End()
        {
            playerPanicControl.isPaused = false;
            playerPanicControl.panicLevel = Mathf.Max(playerPanicControl.panicLevel - _stressReduction, 0.0f);
            _minigameCanvas.SetActive(false);
            _otherUICanvas.SetActive(true);
            _playerMovement.CanMove = true;
        }
    }
}
