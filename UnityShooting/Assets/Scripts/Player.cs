using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;

    public int life;
    public int score;
    public float speed;
    public int maxPower;
    public int power;
    
    public int maxBoom;
    public int boom;

    public float maxShotDelay;
    public float curShotDelay;
    


    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject boomEffect;


    public GameManager gameManager;
    public ObjectManager objectManager;

    public bool isHit;
    public bool isBoomTime;

    public GameObject[] followers;

    public bool isRespawnTime;

    public bool[] joyControl;
    public bool isControl;
    public bool isButtonA;
    public bool isButtonB;



    SpriteRenderer spriteRenderer;
    Animator anim;
    
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        Unbeatable();
        Invoke("Unbeatable", 2); 
    }

    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;

          if (isRespawnTime) // 무적 타임 이펙트 ( 투명 )
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);

            for (int index = 0; index < followers.Length; index++)
            {
                followers[index].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            }
        }

        else // 무적 타임 종료
        {
            
            spriteRenderer.color = new Color(1, 1, 1, 1);

            for (int index = 0; index < followers.Length; index++)
            {
                followers[index].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }

        }
    }

    void Update()
    {
        Move();
        Fire();
        Reload();
        Boom();
    }

    public void JoyPanel(int type)
    {
        for (int index = 0; index < 9; index++)
        {
            joyControl[index] = index == type;
        }
    }

    public void JoyDown()
    {
        isControl = true;
    }

    public void JoyUp()
    {
        isControl = false;
    }

    void Move()
    {
        // Keyboard Control Value
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        //Joy Control Value
        if (joyControl[0]) { h = -1; v = 1; }
        if (joyControl[1]) { h = 0; v = 1; }
        if (joyControl[2]) { h = 1; v = 1; }
        if (joyControl[3]) { h = -1; v = 0; }
        if (joyControl[4]) { h = 0; v = 0; }
        if (joyControl[5]) { h = 1; v = 0; }
        if (joyControl[6]) { h = -1; v = -1; }
        if (joyControl[7]) { h = 0; v = -1; }
        if (joyControl[8]) { h = 1; v = -1; }



        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1) || !isControl)
            h = 0;

        

        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1) || !isControl)
            v = 0;

        Vector3 curPos = transform.position;
        Vector3 nextPox = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPox;


        if (Input.GetButtonDown("Horizontal") ||
            Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Input", (int)h);
        }
    }


    public void ButtonADown()
    {
        isButtonA = true;

    }

    public void ButtonAUp()
    {
        isButtonA = false;
    }

    public void ButtonBDown()
    {
        isButtonB = true;

    }


    void Fire()
    {
         if (!Input.GetButton("Fire1"))
            return;

        if (!isButtonA)
            return;

        if (curShotDelay < maxShotDelay)
            return;

        switch (power)
        {
            case 1: // Power One

                GameObject bullet = objectManager.MakeObj("BulletPlayerA");
                bullet.transform.position = transform.position;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            
                break;

            case 2:

                GameObject bulletR = objectManager.MakeObj("BulletPlayerA");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;
               
                GameObject bulletL = objectManager.MakeObj("BulletPlayerA");
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;

               
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

            case 3:

                GameObject bulletR3 = objectManager.MakeObj("BulletPlayerA");
                bulletR3.transform.position = transform.position + Vector3.right * 0.3f;

                GameObject bulletC3 = objectManager.MakeObj("BulletPlayerA");
                bulletC3.transform.position = transform.position;

                GameObject bulletL3 = objectManager.MakeObj("BulletPlayerA");
                bulletL3.transform.position = transform.position + Vector3.left * 0.3f;


                Rigidbody2D rigidR3 = bulletR3.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidC3 = bulletC3.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL3 = bulletL3.GetComponent<Rigidbody2D>();

                rigidR3.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidC3.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL3.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                break;

            case 4:

                GameObject bulletRR4 = objectManager.MakeObj("BulletPlayerA");
                bulletRR4.transform.position = transform.position + Vector3.right * 0.3f;
                GameObject bulletR4 = objectManager.MakeObj("BulletPlayerA");
                bulletR4.transform.position = transform.position + Vector3.right * 0.1f;



                GameObject bulletLL4 = objectManager.MakeObj("BulletPlayerA");
                bulletLL4.transform.position = transform.position + Vector3.left * 0.3f;
                GameObject bulletL4 = objectManager.MakeObj("BulletPlayerA");
                bulletL4.transform.position = transform.position + Vector3.left * 0.1f;


                Rigidbody2D rigidRR4 = bulletRR4.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidR4 = bulletR4.GetComponent<Rigidbody2D>();


                Rigidbody2D rigidLL4 = bulletLL4.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL4 = bulletL4.GetComponent<Rigidbody2D>();


                rigidRR4.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidR4.AddForce(Vector2.up * 10, ForceMode2D.Impulse);


                rigidLL4.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL4.AddForce(Vector2.up * 10, ForceMode2D.Impulse);



                break;

            case 5: 

                GameObject bulletB = objectManager.MakeObj("BulletPlayerB");              
                bulletB.transform.position = transform.position;

                Rigidbody2D rigidB = bulletB.GetComponent<Rigidbody2D>();
                rigidB.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                break;

            case 6:

                GameObject bulletB6 = objectManager.MakeObj("BulletPlayerB");
                GameObject bulletBR6 = objectManager.MakeObj("BulletPlayerA");

                bulletB6.transform.position = transform.position;
                bulletBR6.transform.position = transform.position + Vector3.right * 0.2f;

                Rigidbody2D rigidB6 = bulletB6.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBR6 = bulletBR6.GetComponent<Rigidbody2D>();

                rigidB6.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidBR6.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                break;

            case 7:

                GameObject bulletB7 = objectManager.MakeObj("BulletPlayerB");
                GameObject bulletBR7 = objectManager.MakeObj("BulletPlayerA");
                GameObject bulletBL7 = objectManager.MakeObj("BulletPlayerA");


                bulletB7.transform.position = transform.position;
                bulletBR7.transform.position = transform.position + Vector3.right * 0.2f;
                bulletBL7.transform.position = transform.position + Vector3.left * 0.2f;


                Rigidbody2D rigidB7 = bulletB7.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBR7 = bulletBR7.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBL7 = bulletBL7.GetComponent<Rigidbody2D>();


                rigidB7.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidBR7.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidBL7.AddForce(Vector2.up * 10, ForceMode2D.Impulse);


                break;

            case 8:

                GameObject bulletB8 = objectManager.MakeObj("BulletPlayerB");
                GameObject bulletBR8 = objectManager.MakeObj("BulletPlayerA");
                GameObject bulletBL8 = objectManager.MakeObj("BulletPlayerA");
                GameObject bulletBRR8 = objectManager.MakeObj("BulletPlayerA");



                bulletB8.transform.position = transform.position;
                bulletBR8.transform.position = transform.position + Vector3.right * 0.2f;
                bulletBL8.transform.position = transform.position + Vector3.left * 0.2f;
                bulletBRR8.transform.position = transform.position + Vector3.right * 0.35f;



                Rigidbody2D rigidB8 = bulletB8.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBR8 = bulletBR8.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBL8 = bulletBL8.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBRR8 = bulletBRR8.GetComponent<Rigidbody2D>();


                rigidB8.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidBR8.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidBL8.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidBRR8.AddForce(Vector2.up * 10, ForceMode2D.Impulse);


                break;

            default:

                GameObject bulletB9 = objectManager.MakeObj("BulletPlayerB");
                GameObject bulletBR9 = objectManager.MakeObj("BulletPlayerA");
                GameObject bulletBL9 = objectManager.MakeObj("BulletPlayerA");
                GameObject bulletBRR9 = objectManager.MakeObj("BulletPlayerA");
                GameObject bulletBLL9 = objectManager.MakeObj("BulletPlayerA");



                bulletB9.transform.position = transform.position;
                bulletBR9.transform.position = transform.position + Vector3.right * 0.2f;
                bulletBL9.transform.position = transform.position + Vector3.left * 0.2f;
                bulletBRR9.transform.position = transform.position + Vector3.right * 0.35f;
                bulletBLL9.transform.position = transform.position + Vector3.left * 0.35f;



                Rigidbody2D rigidB9 = bulletB9.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBR9 = bulletBR9.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBL9 = bulletBL9.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBRR9 = bulletBRR9.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidBLL9 = bulletBLL9.GetComponent<Rigidbody2D>();


                rigidB9.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidBR9.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidBL9.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidBRR9.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidBLL9.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                

                break;


           
            


        }


        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    } 

    void Boom()
    {
        // if (!Input.GetButton("Fire2"))
        //    return;

        if (!isButtonB)
            return;

        if (isBoomTime)
            return;

        if (boom == 0)
            return;
        boom--;
        isButtonB = false;
        isBoomTime = true;
        gameManager.UpdateBoomIcon(boom);



        // 1. Effect visible;
        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 4f);


        // 2. Remove Enemy
        GameObject[] enemiesL = objectManager.GetPool("EnemyL");
        GameObject[] enemiesM = objectManager.GetPool("EnemyM");
        GameObject[] enemiesS = objectManager.GetPool("EnemyS");

        for (int index = 0; index < enemiesL.Length; index++)
        {
            if(enemiesL[index].activeSelf)
            {
                Enemy enemyLogic = enemiesL[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesM.Length; index++)
        {
            if (enemiesM[index].activeSelf)
            {
                Enemy enemyLogic = enemiesM[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        for (int index = 0; index < enemiesS.Length; index++)
        {
            if (enemiesS[index].activeSelf)
            {
                Enemy enemyLogic = enemiesS[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
        }

        // 3. Remove Enemy Bullet
        GameObject[] bulletsA = objectManager.GetPool("BulletEnemyA");
        GameObject[] bulletsB = objectManager.GetPool("BulletEnemyB");

        for (int index = 0; index < bulletsA.Length; index++)
        {
            if (bulletsA[index].activeSelf)
            {
                bulletsA[index].SetActive(false);
            }
        }

        for (int index = 0; index < bulletsB.Length; index++)
        {
            if (bulletsB[index].activeSelf)
            {
                bulletsB[index].SetActive(false);

            }
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            switch(collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;

                    break;
                case "Right":
                    isTouchRight = true;

                    break;
                case "Left":
                    isTouchLeft = true;

                    break;
            }
        }

        else if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {



            
            if (isRespawnTime)
                return;
            

            if (isHit)
                return;

            isHit = true;
            life--;
            
            gameManager.UpdateLifeIcon(life);
            gameManager.CallExplosion(transform.position, "P");
          

            if (life == 0)
            {
                gameManager.GameOver();

            }

            else
            {
                gameManager.RespawnPlayer();
            }

            gameObject.SetActive(false);
           // Destroy(collision.gameObject);
          }

        else if(collision.gameObject.tag == "Item")
            {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case "Coin":
                    score += 1000;
                    

                    break;

                case "Power":
                    if (power == maxPower)
                        score += 500;
                    else
                    {
                        power++;
                        AddFollower();
                    }
                        
                    
                    break;

                case "Boom":
                    if (boom == maxBoom)
                        score += 500;
                    else
                    {
                        boom++;
                        gameManager.UpdateBoomIcon(boom);
                    }
                      
                    
                    break;
            }
            collision.gameObject.SetActive(false);

            //Destroy(collision.gameObject);

        }
    }

    void AddFollower()
    {
        if (power == 3)
            followers[0].SetActive(true);
        else if (power == 6)
            followers[1].SetActive(true);
        else if (power == 9)
            followers[2].SetActive(true);

    }


    void OffBoomEffect()
    {
         boomEffect.SetActive(false);
        isBoomTime = false;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;

                    break;
                case "Right":
                    isTouchRight = false;

                    break;
                case "Left":
                    isTouchLeft = false;

                    break;
            }
        }
    }


}

