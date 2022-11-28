using System.Collections;
using UnityEngine;
public class Bullet : MonoBehaviour
{

    [SerializeField] private float _speed;
    public IEnumerator Fire(Brick targetBrick,Transform bulletExitPoint)
    {
         // mermiyi olusturup posizyon rotasyonunu ayarlýyorum
        while(Vector3.Distance(transform.position, targetBrick.transform.position) > .2f)
        {
            transform.Translate(transform.forward * Time.deltaTime * _speed,Space.World);
            transform.LookAt(targetBrick.transform.position);
            if (Vector3.Distance(transform.position, targetBrick.transform.position) < .2f)
            {
                ObjectPool.Instance.SetPooledObject(this.gameObject, 0);
                targetBrick.Destroy();
            }
            yield return null;
        }
    }
}
