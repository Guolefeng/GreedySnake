using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {

	// 下一个身子的引用
	public Body next;

	// 是当前身子移动
	public void Move (Vector3 pos) {
		// 记录移动前的位置
		Vector3 nextPos = transform.position;
		// 移动当前身子
		transform.position = pos;
		// 如果后面还有身子
		if (next != null) {
			// 让后面的身子移动
			next.Move(nextPos);
		}
	}
}
