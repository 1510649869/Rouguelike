using UnityEngine;
using System.Collections;


//定义成一个公有的抽象类；
public abstract class MovingObject : MonoBehaviour {
    public float MoveTime=0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

    protected virtual void Start()
    {
        boxCollider=GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1 / MoveTime;
    }
    protected bool Move(int xDir,int yDir,out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);//结束位置
        boxCollider.enabled = false;//防止射线碰撞自身
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
        if(hit.transform==null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }
    //移动
    protected IEnumerator SmoothMovement(Vector2 end)
    {
        Vector2 start = transform.position;
        float sqrRemainingDistance = (start - end).sqrMagnitude;
        //Debug.Log();
        while(sqrRemainingDistance>float.Epsilon)
        {
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPostion);
            Vector2 start1 = transform.position;
            sqrRemainingDistance = (start1 - end).sqrMagnitude;
            yield return null;
        }
    }

    //用于交互 敌人-》主角    主角-》墙壁
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
            return;
        T hitComponent = hit.transform.GetComponent<T>();
        if(!canMove&&hitComponent!=null)//对象被阻挡
        {
            OncantMove(hitComponent);
            //Debug.Log("~~~");
        }
    }
    protected abstract void OncantMove<T>(T component)
        where T : Component;

}
