using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 枚举表示蛇头移动的方向
public enum HeadDir {
	Up,     // 上
	Down,   // 下
	Left,   // 左
	Right,  // 右
}

public class Controller : MonoBehaviour {

	// 食物的预设体
	public GameObject foodPrefab;
	// 身子的预设体
	public GameObject bodyPrefab;
	// 移动速度, 米/秒
	public float speed;

	// 计时器用来记录移动的时间
	private float _Timer = 0f;
	// 蛇头当前移动的方向
	private HeadDir _CurrentDir = HeadDir.Up;
	// 接下来蛇头移动的方向
	private HeadDir _NextDir = HeadDir.Up;
	// 游戏是否结束
	private bool _IsOver = false;
	// 第一节身子的引用
	private Body _FirstBody;
	// 最后一节身子的引用
	private Body _LastBody;

	// 创建出食物
	public void CreateFood () {
		// 随机出食物的坐标
		float x = Random.Range (-9.5f, 9.5f);
		float z = Random.Range (-9.5f, 9.5f);
		// 动态创建出食物
		GameObject obj = Instantiate (foodPrefab, new Vector3 (x, 0f, z), Quaternion.identity) as GameObject;
	}

	// 触发事件, 当触发开始的时候
	public void OnTriggerEnter (Collider other) {
		// 如果碰到边界
		if (other.tag.Equals ("Bound") || other.tag.Equals ("Body")) {
			// 游戏结束
			_IsOver = true;
		}

		// 如果碰到了食物
		if (other.tag.Equals ("Food")) {
			// 将之前的食物销毁
			Destroy (other.gameObject);
			// 增长一节身体
			Grow ();
			// 随机创建食物
			CreateFood ();
		}
	}

	// 增长一节身体
	private void Grow () {
		// 动态创建出身子
		GameObject obj = Instantiate (bodyPrefab, new Vector3 (1000f, 1000f, 1000f), Quaternion.identity) as GameObject;
		// 获取身子上的 Body 脚本
		Body b = obj.GetComponent <Body> ();
		// 如果头后面还没有身子
		if (_FirstBody == null) {
			// 当前创建出的身子就是第一节身子
			_FirstBody = b;
		}
		// 如果有最后一节身子
		if (_LastBody != null) {
			// 就新创建出的身子设置在当前最后一节身子的后面
			_LastBody.next = b;
		}

		// 更新最后一节身子
		_LastBody = b;
	}

	private void Start () {
		// 游戏开始时需要一个食物
		CreateFood ();
	}

	// 如果游戏没有结束就执行主循环
	// 如果我们不控制物体但想让他持续移动的话, 就放在update中
	private void Update () {
		if (!_IsOver) {
			Turn ();
			Move ();
		}
	}

	// 改变蛇头方向
	private void Turn () {
		// 监听用户按键事件 W
		if (Input.GetKey (KeyCode.W)) {
			// 设定接下来蛇头移动方向
			_NextDir = HeadDir.Up;
			// 检测按键是否有效
			if (_CurrentDir == HeadDir.Down) {
				// 如果按键无效修正接下来移动的方向
				_NextDir = _CurrentDir;
			}
		}
		// 监听用户按键事件 S
		if (Input.GetKey (KeyCode.S)) {
			// 设定接下来蛇头移动方向
			_NextDir = HeadDir.Down;
			// 检测按键是否有效
			if (_CurrentDir == HeadDir.Up) {
				// 如果按键无效修正接下来移动的方向
				_NextDir = _CurrentDir;
			}
		}
		// 监听用户按键事件 A
		if (Input.GetKey (KeyCode.A)) {
			// 设定接下来蛇头移动方向
			_NextDir = HeadDir.Left;
			// 检测按键是否有效
			if (_CurrentDir == HeadDir.Right) {
				// 如果按键无效修正接下来移动的方向
				_NextDir = _CurrentDir;
			}
		}
		// 监听用户按键事件 D 
		if (Input.GetKey (KeyCode.D)) {
			// 设定接下来蛇头移动方向
			_NextDir = HeadDir.Right;
			// 检测按键是否有效
			if (_CurrentDir == HeadDir.Left) {
				// 如果按键无效修正接下来移动的方向
				_NextDir = _CurrentDir;
			}
		}
	}

	private void Move () {
		
		// 把当前这一帧和上一帧的时间间隔累加上去
		_Timer += Time.deltaTime;
		// 判断当前是否应该移动
		if (_Timer >= (1f / speed)) {

			// 是蛇头旋转
			switch (_NextDir) {
			case HeadDir.Up:
				transform.forward = Vector3.forward;
				_CurrentDir = HeadDir.Up;
				break;
			case HeadDir.Down:
				transform.forward = Vector3.back;
				_CurrentDir = HeadDir.Down;
				break;
			case HeadDir.Left:
				transform.forward = Vector3.left;
				_CurrentDir = HeadDir.Left;
				break;
			case HeadDir.Right:
				transform.forward = Vector3.right;
				_CurrentDir = HeadDir.Right;
				break;
			}

			// 记录头部移动之前的位置
			Vector3 nextPos = transform.position;

			// 持续超前移动一个单位
			transform.Translate (Vector3.forward);
			// 重置计时器
			_Timer = 0f;

			// 如果有身子就让第一节身子移动
			if (_FirstBody != null) {
				// 让第一节身子移动
				_FirstBody.Move (nextPos);
			}
		}
	}
}
