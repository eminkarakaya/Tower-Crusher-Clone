using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Animations_Circle
{
    public class Animations : MonoBehaviour
    {
        public static System.Action OnComplate;
        // listedekiler setparent(null)
        // parentler destroy()
        // 
        
        
        Sequence seq;
        public void CircleMergeAnimation(Transform parent,List<GameObject> objects,List<Transform> startPositions , Transform cameraTransform , System.Action onComplateAction = null,float moveTime = 2f)
        {
            seq = DOTween.Sequence();
            for (int i = 0; i < startPositions.Count; i++)
            {
                
            Debug.Log("wpjýdsýJAFAP");
                seq.Join(objects[i].transform.DOLookAt(cameraTransform.position, moveTime));
                //seq.Join(objects[i].transform.DOMove(startPositions[i].position, moveTime));
                seq.Join(objects[i].transform.DOMove(startPositions[i].position, moveTime)).OnComplete(() => onComplateAction?.Invoke());
                //MergeAnim(parent, objects[i].transform,onComplateAction);
                //seq.Append(()=> CircleMove(objects[i].transform, 2, i * 60, 1, 5));


            }
            onComplateAction?.Invoke();
        }
        void qwe(Transform qwe)
        {

        }
        void CircleMove(Transform parent,Transform transform2,Transform transform1,float speed, float startRadius, float radius , float minRadius)
        {
            Transform _transform = transform2;
            transform1.SetParent(parent);
            float counter = 0f;
            var _radius = radius;
            var _speed = speed;
            counter = startRadius;
            IEnumerator Move(){
                yield return new WaitForSeconds(2f);
                while(_radius > minRadius )
                {
                    Debug.Log(_transform.position);
                    _radius -= Time.deltaTime;
                    counter += Time.deltaTime* _speed;
                    float x = (Mathf.Cos(counter)) * _radius;
                    float y = 0;
                    float z = (Mathf.Sin(counter) ) * _radius;
                    transform1.localPosition = new Vector3(x /*+ _transform.position.x*/, y, z /*+ _transform.position.z*/);
                    yield return null;
                }
            }
            
            StartCoroutine(Move());
        }

        
    }
    
}

