using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAnimatorEvent : MonoBehaviour
{

    private void Start()
    {
        children = new List<GameObject>();
    }
    /* 자식의 name의 트리거를 셋팅합니다. */
    public void CildSetTrigger(string name)
    {
        Animator[] childAnimators = transform.GetComponentsInChildren<Animator>();
        foreach (var animator in childAnimators)
        {
            if (animator == GetComponent<Animator>())
                continue;
            bool isFindParam = false;

            /* 우선 해당 파라메타가 존재하는지 체크. 무작정 다 트리거링하니깐 워닝 로그 뜸 */
            foreach (var param in animator.parameters)
            {
                if (param.name == name && param.type == AnimatorControllerParameterType.Trigger)
                {
                    isFindParam = true;
                    break;
                }
            }

            /* 찾았다면 트리거링 */
            if (isFindParam)
            {
                animator.SetTrigger(name);
            }
        }
    }

    private List<GameObject> children;
    /* 일시적으로 자식을 모두 제거합니다. */
    public void RemoveChildren_Temp()
    {
        foreach(Transform child in transform)
        {
            children.Add(child.gameObject);
            child.SetParent(transform.parent);
        }
    }

    /* 일시적으로 제거했던 자식을 복구합니다. */
    public void RemoveChilden_Return()
    {
        foreach(GameObject child in children)
        {
            child.transform.SetParent(transform);
        }
    }
}
