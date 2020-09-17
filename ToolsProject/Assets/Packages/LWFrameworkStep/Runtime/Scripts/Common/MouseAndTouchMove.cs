using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseAndTouchMove : MonoBehaviour {
   
	    //用于绑定参照物对象
    public Transform target  ;
    /// <summary>
    /// 最大缩放距离
    /// </summary>
    public float maxDis = 6.0f;
    /// <summary>
    /// 最小缩放距离
    /// </summary>
    public float minDis = 1.0f;
    /// <summary>
    /// 距离缩放速度
    /// </summary>
    public float disSpeed = 0.05f;
    //缩放系数
    public float distance = 0.2f;
    //左右滑动移动速度
    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    //缩放限制系数
    public float xMinLimit = -20f;
    public float xMaxLimit = 80f;
    //缩放限制系数
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    //摄像头的位置
    public float x = 0.0f;
    public float y = 0.0f;
    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势
    private Vector2 oldPosition1  ;
    private Vector2 oldPosition2  ;
    private float delayTime=0.05f;
    private bool canControl = true;
    private Touch touch;
    //初始化游戏信息设置
    void Start () {
        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
 
	    // Make the rigid body not change rotation
      // 	if (rigidbody)
	    //	rigidbody.freezeRotation = true;
    }
 
    void  Update ()
    {
        //旋转
        if ((Input.GetMouseButton(1) || Input.GetMouseButton(0)) &&!isOnUI())
        {

            x += Input.GetAxis("Mouse X") * xSpeed * 0.05f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.08f;

        }
        if ( !isOnUI()) {
            distance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 30 * Mathf.Abs(distance);
            distance = Mathf.Clamp(distance, minDis, maxDis);
        }
       
        //Debug.Log("distance：" + distance);

    }
    IEnumerator SetCanControl() {
        canControl = false;
        yield return new WaitForSeconds(delayTime);
        canControl = true;
    }
    //函数返回真为放大，返回假为缩小
    bool isEnlarge(Vector2 oP1 ,Vector2 oP2  ,Vector2 nP1  ,Vector2 nP2  )
    {
	    //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势
        var leng1 =Mathf.Sqrt((oP1.x-oP2.x)*(oP1.x-oP2.x)+(oP1.y-oP2.y)*(oP1.y-oP2.y));
        var leng2 =Mathf.Sqrt((nP1.x-nP2.x)*(nP1.x-nP2.x)+(nP1.y-nP2.y)*(nP1.y-nP2.y));
        if(leng1<leng2)
        {
    	     //放大手势
             return true;
        }else
        {
    	    //缩小手势
            return false;
        }
    }
 
    //Update方法一旦调用结束以后进入这里算出重置摄像机的位置
    void  LateUpdate () {
 
        //target为我们绑定的箱子变量，缩放旋转的参照物
        if (target )
        {		
 
    	    //重置摄像机的位置
 		    y = ClampAngle(y, yMinLimit, yMaxLimit);
            var rotation = Quaternion.Euler(y, x, 0);
            var position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
 
            transform.rotation = rotation;
            transform.position = position;
        }
    }
    bool isOnUI() {
        bool ret = false;
        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                ret = true;
        }
        else {
            if (EventSystem.current.IsPointerOverGameObject())
                ret = true;
        }
        return ret;
    }
    static float  ClampAngle (float angle ,float min  ,float max  ) {
	    if (angle < -360)
		    angle += 360;
	    if (angle > 360)
		    angle -= 360;
	    return Mathf.Clamp (angle, min, max);
    }
}
