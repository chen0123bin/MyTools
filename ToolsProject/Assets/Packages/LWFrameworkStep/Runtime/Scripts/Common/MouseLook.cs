using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public enum Interactions
{
    None = 0,
    Keyboard = 1,
    MouseWheel = 2,
    MouseX = 4,
    MouseY = 8,
    MouseMove = 16,
    All = 31
}

public class MouseLook : MonoBehaviour
{
    public static CameraMode Mode = CameraMode.BirdEye;
    public static bool MouseLookEnabled = true;
    public static float minY = 0.5f;
    public static Interactions SceneInteraction = Interactions.All;
    public static MouseLook Instance;

    public MonoBehaviour CameraOpertor { get; set; }
    public Vector3 TargetPosition { get; set; }
    public Quaternion TargetRotation { get; set; }

    public MeshRenderer sceneOutline;
    public Vector3 prevPosition;
    public Quaternion prevRotation;

    public float xRotationSpeed = 6F;
    public float yRotationSpeed = 6F;

    public float xMoveSpeed = 10F;
    public float yMoveSpeed = 10F;
    public float initMinY = 0.5f;
    public float maxElevationAngle = 60f;

    public float modeCriticalValue = 8f;

    private float mouseMoveDelay = 0;

    //public float minimumX = -360F;
    //public float maximumX = 360F;

    //public float minimumY = -60F;
    //public float maximumY = 60F;

    //float rotationY = 0F;

    public MouseLook()
    {
        Instance = this;
    }

    void Start()
    {
		if(Mode == CameraMode.Wander)
		{	
			Mode = CameraMode.BirdEye;
			SceneInteraction = Interactions.All;
            minY = initMinY;

        }		

        TargetPosition = transform.position;
        TargetRotation = transform.rotation;

        CameraOpertor = this;
    }
    public void SetTarget(Vector3 v,Quaternion q) {
        TargetPosition = v;
        TargetRotation = q;
        transform.position = v;
        transform.localRotation = q;
    }
    private bool isDownUI = false;
    

    void Update()
    {
        if (Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                isDownUI = true;
            }
            else {
                isDownUI = false;
            }
        }
       
        if (MouseLookEnabled)
        {
            mouseMoveDelay++;
            
            if (mouseMoveDelay < 20 ||isDownUI)
            {
                return;
            }
            if (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject()||(EventSystem.current.currentSelectedGameObject&&EventSystem.current.currentSelectedGameObject.transform.root.GetComponent<Canvas>().renderMode==RenderMode.WorldSpace))
            {
                if (Input.GetMouseButton(0) && Input.GetAxis("Mouse X") != 0)
                {
                    if ((SceneInteraction & Interactions.MouseMove) == Interactions.MouseMove)
                    {
                        CameraOpertor = this;
                        TargetPosition = transform.position - transform.right * Input.GetAxis("Mouse X") * xMoveSpeed * (transform.position.y / 10 + 0.2f) - transform.up * Input.GetAxis("Mouse Y") * yMoveSpeed * (transform.position.y / 10);
                    }
                }
                else if (Input.GetMouseButton(1))
                {
                    Vector3 angles = transform.eulerAngles;
                    if ((SceneInteraction & Interactions.MouseX) == Interactions.MouseX && Input.GetAxis("Mouse X") != 0)
                    {
                        CameraOpertor = this;
                        angles.y += Input.GetAxis("Mouse X") * xRotationSpeed;
                        TargetRotation = Quaternion.Euler(angles);
                    }

                    if ((SceneInteraction & Interactions.MouseY) == Interactions.MouseY && Input.GetAxis("Mouse Y") != 0)
                    {
                        CameraOpertor = this;
                        angles.x -= Input.GetAxis("Mouse Y") * yRotationSpeed;// Mathf.Clamp(angles.x-Input.GetAxis("Mouse Y") * sensitivityRotationY, minimumY, maximumY);
                        angles.x %= 360f;
                        if (angles.x < -maxElevationAngle || (angles.x > 180 && angles.x < 360 - maxElevationAngle))
                        {

                            angles.x = -maxElevationAngle;
                        }
                        if (angles.x < 270 && angles.x > 80)
                        {
                            angles.x = 80;
                        }

                        TargetRotation = Quaternion.Euler(angles.x, angles.y, 0);
                    }
                }
                else if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    if ((SceneInteraction & Interactions.MouseWheel) == Interactions.MouseWheel)
                    {
                        CameraOpertor = this;
                        TargetPosition = transform.position + transform.forward * Input.GetAxis("Mouse ScrollWheel") * yMoveSpeed * 6 * (transform.position.y / 10 + 0.2f);
                    }
                }
            }
        }
        else
        {
            mouseMoveDelay = 0;
        }

        if ((SceneInteraction & Interactions.Keyboard) == Interactions.Keyboard)
        {
            Vector3 currentPosition = transform.position;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                CameraOpertor = this;
                currentPosition += transform.forward * yMoveSpeed*0.5f;
                TargetPosition = currentPosition;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                CameraOpertor = this;
                currentPosition -= transform.forward * yMoveSpeed * 0.5f;
                TargetPosition = currentPosition;
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                CameraOpertor = this;
                currentPosition -= transform.right * xMoveSpeed * 0.5f;
                TargetPosition = currentPosition;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                CameraOpertor = this;
                currentPosition += transform.right * xMoveSpeed * 0.5f;
                TargetPosition = currentPosition;
            }
        }

        if (TargetPosition.y < minY)
        {
            TargetPosition = new Vector3(TargetPosition.x, minY, TargetPosition.z);
        }

        if (CameraOpertor == this)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation, Time.deltaTime * 15);

            //if (!sceneOutline.isVisible)
            //{
            //    print("UN________________");
            //    transform.position = prevPosition;
            //    transform.rotation = prevRotation;
            //}
            //else if (transform.position == TargetPosition && transform.rotation == TargetRotation&&sceneOutline.isVisible)
            //{
            //    print("Target__________________");
            //    prevPosition = TargetPosition;
            //    prevRotation = TargetRotation;
            //}
        }

        if(Mode == CameraMode.BirdEye&&transform.position.y< modeCriticalValue)
        {
            Mode = CameraMode.Wander;
        }
    }
}

public enum CameraMode
{
    BirdEye=0,
    Wander =1,
    Patrol=2,
    Other=3
}
