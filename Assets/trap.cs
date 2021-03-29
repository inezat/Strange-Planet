using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour
{
    [SerializeField] private bool isActive = true;
    Controller player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (isActive)
            {
                player.takeDamage();
            }
            else
            {
                if(player.keys > 0)
                {
                    player.keys--;
                    turnActive();
                    Debug.Log("fixed");
                }
            }
        }else if (collision.tag == "Wolf" || collision.tag == "Snake" || collision.tag == "Snake2")    //Interact with enemies 
        {
            Debug.Log("Collide with enemies");
            if (isActive)
            {
                player.monsterKilled++;
                Destroy(collision.gameObject);
            }

        }
    }
    public void turnActive()
    {
        isActive = true;
    }
    public bool getActive()
    {
        return isActive;
    }
}
