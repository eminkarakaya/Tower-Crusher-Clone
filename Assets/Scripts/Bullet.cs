using System.Collections;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public int earnedMoney;
    [SerializeField] private float _speed;
    public IEnumerator Fire(Brick targetBrick,Transform bulletExitPoint)
    {
         // mermiyi olusturup posizyon rotasyonunu ayarlýyorum
        while(Vector3.Distance(transform.position, targetBrick.transform.position) > .1f)
        {
            transform.Translate(transform.forward * Time.deltaTime * _speed,Space.World);
            transform.LookAt(targetBrick.transform.position);
            if (Vector3.Distance(transform.position, targetBrick.transform.position) < .2f)
            {

                ObjectPool.Instance.SetPooledObject(this.gameObject, 0);
                GameManager.Instance.SetGold(earnedMoney);
                targetBrick.Destroy();
                break;
            }
            yield return null;
        }
    }
}
