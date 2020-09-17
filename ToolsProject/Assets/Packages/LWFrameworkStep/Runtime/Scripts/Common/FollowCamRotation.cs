using UnityEngine;
using System.Collections;
/**
FollowCamRotation 、HeadUI 脚本两个脚本可以制作全跟随性VRUI
结构如下
FollowCamRotation --跟随眼镜的位置 posiIsTrack需要勾选
    HeadUI        -- 控制检测视角的范围headTracking为自身     
        FollowCamRotation --控制UI的旋转
     
     */
public class FollowCamRotation : MonoBehaviour
{
    public GameObject cam;
    public float speed = 0.5f;
    public bool isLockX;
    public bool isLockY;
    public bool isLockZ;
    private Vector3 v3Offset;
    private Vector3 _eulerAngles;
    public Vector3 _eulerOffset;
    public bool IsUp =false;
    public bool posiIsTrack = false;
    
    // Use this for initialization
    void Start()
    {

        if (cam == null)
        {
            return;
        }
        v3Offset = transform.position - cam.transform.position;
    }

  
    private void Update()
    {
        if (cam == null && Camera.main)
        {
            cam = Camera.main.gameObject;
            v3Offset = transform.position - cam.transform.position;
        }
        if (cam == null)
        {
            return;
        }
        if(posiIsTrack)
            transform.position = cam.transform.position ;
        if (!IsUp)
            _eulerAngles = cam.transform.rotation.eulerAngles;
        else
        {
            _eulerAngles = Vector3.left;
        }
        if (isLockX)
            _eulerAngles.x = 0;
        if (isLockY)
            _eulerAngles.y = 0;
        if (isLockZ)
            _eulerAngles.z = 0;
        _eulerAngles += _eulerOffset;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(_eulerAngles), speed * Time.deltaTime);
    }

}
