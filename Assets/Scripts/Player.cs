using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Player : MovingObject {
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoad = 20;
    public float restartLevelDelay = 1f;

    public Text foodText;
    private Animator animator;
    private int food;
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;
        foodText.text = "Food: " + food;
        base.Start();
    }

    void Update()
    {
        if (!GameManager.instance.playerTurn) return;
        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));
        if (horizontal != 0)
            vertical = 0;
        if(horizontal!=0||vertical!=0)
        {
            //Debug.Log(horizontal+" "+vertical);
            AttemptMove<Walls>(horizontal, vertical);
        }

    }
    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;//主角没移动一步，减少一点；
        //Debug.Log(food);
        foodText.text = "Food: " + food;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        CheckIfGameOver();

        GameManager.instance.playerTurn = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Exit")
        {
            Invoke("Restart",restartLevelDelay);
            enabled = false;
        }
        else if(other.tag=="Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            other.gameObject.SetActive(false);
        }
        else if(other.tag=="Soda")
        {
            food +=pointsPerSoad;
            foodText.text = "+" + pointsPerSoad + " Food: " + food;
            other.gameObject.SetActive(false);
        }

    }
    protected override void OncantMove<T>(T component)
    {
        Walls hitwall = component as Walls;
        hitwall.DamageWall(wallDamage);
        animator.SetTrigger("PlayerChop");
    }
    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    public void loseFood(int loss)
    {
        animator.SetTrigger("PlayerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver();
    }
    private void  CheckIfGameOver()
    {
        if(food<0)
        {
            GameManager.instance.GameOver();
        }
    }
}
