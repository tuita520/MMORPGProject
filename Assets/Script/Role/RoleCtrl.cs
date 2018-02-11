// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-01-12 10:21:52
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 角色控制器
/// </summary>
public class RoleCtrl : MonoBehaviour
{
    //移动速度
    private float m_Speed = 0.1f;
    //移动的目标位置
    private Vector3 m_TargetPos;
    //角色控制组件
    private CharacterController m_CharacterController;
    //转身速度
    private float m_RosSpeed = 10f;

    #region Start 组件初始化
    /// <summary>
    /// 组件初始化
    /// </summary>
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        if(CameraCtr.Instance != null)
        {
            CameraCtr.Instance.Init();
        }

        FingerEvent.Instance.OnFingerDrag += OnFingerDrag;
        FingerEvent.Instance.OnPlayerClickGround += OnPlayerClickGround;
        FingerEvent.Instance.OnZoom += OnZoom;
    }
    #endregion

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
                m_TargetPos = hit.point;
                m_RosSpeed = 0;
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

    #region Update 角色移动
    /// <summary>
    /// 角色移动
    /// </summary>
    void Update()
    {

        if (m_CharacterController == null) return;

        //判断当前CharacterController组件是否接地
        if (!m_CharacterController.isGrounded)
        {
            //让角色贴着地面
            m_CharacterController.Move((transform.position + new Vector3(0,-1000,0))- transform.position);
        }

        if(m_TargetPos != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, m_TargetPos) > 0.5f)
            {
                //移动的方向
                Vector3 direction = m_TargetPos - transform.position;
                direction = direction.normalized;
                Quaternion m_TargetRos = Quaternion.identity;

                if(m_RosSpeed <= 1f)
                {
                    m_RosSpeed += 4f * Time.deltaTime;
                    m_TargetRos = Quaternion.LookRotation(direction);
                    //当m_RosSpeed==1的时候根据lerp函数此时已经将transform转身完毕了
                    transform.rotation = Quaternion.Lerp(transform.rotation, m_TargetRos,m_RosSpeed);
                }

                //transform.LookAt(new Vector3(m_TargetPos.x, transform.position.y, m_TargetPos.z));
                m_CharacterController.Move(direction * m_Speed);
            }
        }

        //摄像机自动跟随
        CameraAutoFollow();
    }
    #endregion

    #region CameraAutoFollow 摄像机自动跟随
    /// <summary>
    /// 摄像机自动跟随
    /// </summary>
    private void CameraAutoFollow()
    {
        if (CameraCtr.Instance == null) return;

        CameraCtr.Instance.transform.position = transform.position;
        CameraCtr.Instance.AutoLookAt(gameObject.transform.position);
    }
    #endregion
}
