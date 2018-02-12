// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-12 10:39:09
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;

public class CitySceneCtrl : MonoBehaviour {

    void Awake()
    {
        SceneUIMgr.Instance.LoadSceneUI(SceneUIMgr.SceneUIType.MainCity);
    }

	void Start () {

        if (FingerEvent.Instance != null)
        {
            Debug.Log("fingerEvent ");
            FingerEvent.Instance.OnFingerDrag += OnFingerDrag;
            FingerEvent.Instance.OnPlayerClickGround += OnPlayerClickGround;
            FingerEvent.Instance.OnZoom += OnZoom;
        }

        //加载玩家
        GameObject obj = RoleMgr.Instance.LoadPlayer("Role_MainPlayer");
        GlobalInit.Instance.CurrPlayer = obj.GetComponent<RoleCtrl>();
        GlobalInit.Instance.CurrPlayer.Init(RoleType.MainPlayer,new RoleInfoBase(),new RoleMainPlayerCityAI());

    }

    #region OnZoom 摄像机缩放
    /// <summary>
    /// 摄像机缩放
    /// </summary>
    /// <param name="obj"></param>
    private void OnZoom(FingerEvent.ZoomType obj)
    {
        switch (obj)
        {
            case FingerEvent.ZoomType.In:
                CameraCtr.Instance.SetCameraZoom(0);
                break;
            case FingerEvent.ZoomType.Out:
                CameraCtr.Instance.SetCameraZoom(1);
                break;
            default:
                break;
        }
    }
    #endregion

    #region OnPlayerClickGround 鼠标点击地面
    /// <summary>
    /// 鼠标点击地面
    /// </summary>
    private void OnPlayerClickGround()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.name.Equals("Ground"))
            {
                Debug.Log("点击到了地面");
                if(GlobalInit.Instance.CurrPlayer != null)
                {
                    Debug.Log("开始");
                    GlobalInit.Instance.CurrPlayer.MoveTo(hit.point);
                }
            }
        }
    }
    #endregion

    #region OnFingerDrag 手指滑动
    /// <summary>
    /// 手指滑动
    /// </summary>
    /// <param name="obj"></param>
    private void OnFingerDrag(FingerEvent.FingerDir obj)
    {
        switch (obj)
        {
            case FingerEvent.FingerDir.Up:
                CameraCtr.Instance.SetCameraUpAndDown(1);
                break;
            case FingerEvent.FingerDir.Down:
                CameraCtr.Instance.SetCameraUpAndDown(0);
                break;
            case FingerEvent.FingerDir.Left:
                CameraCtr.Instance.SetCameraRotate(1);
                break;
            case FingerEvent.FingerDir.Right:
                CameraCtr.Instance.SetCameraRotate(0);
                break;
            default:
                break;
        }
    }
    #endregion


    #region OnDestroy 销毁
    /// <summary>
    /// 销毁
    /// </summary>
    private void OnDestroy()
    {
        FingerEvent.Instance.OnFingerDrag -= OnFingerDrag;
        FingerEvent.Instance.OnPlayerClickGround -= OnPlayerClickGround;
        FingerEvent.Instance.OnZoom -= OnZoom;
    }
    #endregion
}
