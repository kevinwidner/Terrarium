using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class spider : MonoBehaviour {

    public GameObject SelectPrefab;
    private GameObject m_selectedGameObject;
    private Animation m_spiderAnimation;
    private const string ANIMATION_ATTACK_LEFT = "Attack_Left";
    private const string ANIMATION_RUN = "Run";
    private const string ANIMATION_IDLE = "Idle";

    [SerializeField][Range(1, 20)]
    private float speed = 10;
    [SerializeField][Range(1, 20)]
    private float rotationSpeed = 3.0f;
    private float attackRadius = 7.0f;
    private float health = 100;
    private float damage = 20;
    float attackCooldown = 3.0f;
    private float m_maxAngleForAttack = 20.0f;
    private float m_maxAngleForMovement = 40.0f;
  
    private GOAP.GoalPlanner m_goalPlanner = new GOAP.GoalPlanner();
    private GOAP.ActionPlanner m_actionPlanner = new GOAP.ActionPlanner();
    private List<GOAP.Goal> m_insectGoals;
    private List<GOAP.Action> m_insectActions;
    private GOAP.Memory m_memory;

    private Stack m_currentActionSet;

    // Use this for initialization
    void Start () 
    {
        m_spiderAnimation = this.GetComponent<Animation>();

        m_memory = new GOAP.Memory();
        m_memory.VisibleInsects = new List<spider>();
        m_memory.TargetPosition = transform.position;

        m_insectGoals = new List<GOAP.Goal>();
        m_insectGoals.Add(new GOAP.Goal("EatFood", null, "FoodEaten", 10));
        m_insectGoals.Add(new GOAP.Goal("GotoTarget", null, "TargetReached", 100));

        m_insectActions = new List<GOAP.Action>();
//        m_insectActions.Add(new GOAP.Action("EatFood", "FoodInRange", "FoodEaten", 10));
//        m_insectActions.Add(new GOAP.Action("MoveToFood", "FoodExists", "FoodInRange", 10));
        m_insectActions.Add(new GOAP.Action_MoveTo(this.m_memory));
    }

    // Update is called once per frame
    void Update () 
    {
        if (!Alive)
            return;

        Observe();

        Think();
    }

    void Observe()
    {
        spider targetInsect = null;
        float targetDist = float.MaxValue;

        foreach (spider testInsect in m_memory.VisibleInsects)
        {
            if (targetInsect == null)
            {
                targetInsect = testInsect;
                targetDist = (testInsect.transform.position - this.transform.position).magnitude;
            }
            else
            {
                float testDist = (testInsect.transform.position - this.transform.position).magnitude;
                if (testDist < targetDist)
                {
                    targetInsect = testInsect;
                }
            }
        }

        if (m_memory.TargetInsect != targetInsect)
            SetTargetInsect(targetInsect);

        // Quick dumb check to make sure that targetInsect is still alive.  If not, null it out.
        if (m_memory.TargetInsect != null && !m_memory.TargetInsect.Alive)
            SetTargetInsect(null);
    }

    void SetTargetInsect(spider targetInsect)
    {
        m_memory.TargetInsect = targetInsect;
        if (m_memory.TargetInsect != null)
        {
            SetTargetPosition(targetInsect.transform.position);
        }
        else
        {
            ClearTargetPosition();
        }
    }

    void Think()
    {
        if (m_currentActionSet == null || m_currentActionSet.Count == 0)
        {
            GOAP.ActionPlan actionPlan = m_actionPlanner.Plan(m_insectGoals[1], m_insectActions.ToArray());

            if (actionPlan != null && actionPlan.Valid)
            {
                m_currentActionSet = new Stack(actionPlan.Plan);
            }
        }



        if (m_memory.TargetInsect != null)
        {
            Move(m_memory.TargetInsect.transform.position);

            if ((m_memory.TargetInsect.transform.position - this.transform.position).magnitude > attackRadius)
                Attack(m_memory.TargetInsect);
        }
        else if (m_memory.ReachedDestination != true)
        {
            Move(m_memory.TargetPosition);            
        }
        else
        {
            //FindNewTargetSpot();
        }
    }

    void Act()
    {
        if (m_currentActionSet != null && m_currentActionSet.Count > 0)
        {
            GOAP.Action action = m_currentActionSet.Peek() as GOAP.Action;

            if (action.TickAction())
            {
                m_currentActionSet.Pop(); //Action complete, take off stack.
            }
        }
        else
        {
            m_spiderAnimation.CrossFade(ANIMATION_IDLE);
        }

    }


    void FindNewTargetSpot()
    {
        SetTargetPosition(new Vector3(Random.Range(-50.0f, 50.0f), 0, Random.Range(-50.0f, 50.0f)));
    }

    void Attack(spider targetSpider)
    {
        if (targetSpider == null || !targetSpider.Alive)
        {
            Debug.Log("The target spider is dead!");
            return;
        }

        Move(targetSpider.transform.position);

        Vector3 directionToTargetPosition = targetSpider.transform.position - transform.localPosition;
        float targetAngleOff = Vector3.Angle(this.transform.forward, directionToTargetPosition);

        if (targetAngleOff < m_maxAngleForAttack)
        {
            if (m_memory.AttackReady)
            {
                m_memory.AttackReady = false;
                m_spiderAnimation.CrossFade(ANIMATION_ATTACK_LEFT);
                StartCoroutine(AttackCoroutine(targetSpider));
                StartCoroutine(AttackCooldownCoroutine(attackCooldown));
            }
        }
    }

    IEnumerator AttackCoroutine(spider targetSpider)
    {
        yield return new WaitForSeconds(1.0f);

        this.transform.LookAt(targetSpider.transform);
        Debug.Log(string.Format("{0} attacking {1} with {2} damage", this.name, targetSpider.name, damage.ToString()));
        targetSpider.TakeDamage(this, damage);
    }

    IEnumerator AttackCooldownCoroutine(float coolDownTimer)
    {
        yield return new WaitForSeconds(coolDownTimer);

        m_memory.AttackReady = true;
    }

    void Move(Vector3 targetPosition)
    {
        Rotate(targetPosition);

        Vector3 directionToTargetPosition = targetPosition - transform.position;
        float targetAngleOff = Vector3.Angle(directionToTargetPosition, this.transform.forward);
        if (targetAngleOff < m_maxAngleForMovement)
        {
            float distThisFrame = speed * Time.deltaTime;
            if (directionToTargetPosition.magnitude > distThisFrame)
            {
                transform.Translate(directionToTargetPosition.normalized * distThisFrame, Space.World);
            }
            else
            {
                m_memory.ReachedDestination = true;
            }

            if (m_spiderAnimation != null)
            {
                if (directionToTargetPosition.magnitude > distThisFrame)
                {
                    m_spiderAnimation.CrossFade(ANIMATION_RUN);
                }
                else
                {
                    m_spiderAnimation.CrossFade(ANIMATION_IDLE);
                }
            }
        }
    }

    void Rotate(Vector3 targetPosition)
    {
        bool rotationAnimation = false;
        Vector3 dir = targetPosition - transform.position;
        float rotThisFrame = rotationSpeed * Time.deltaTime;
        float targetAngleOff = Vector3.Angle(this.transform.forward, dir);

        if (dir != Vector3.zero && targetAngleOff > rotThisFrame)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, rotThisFrame);
            rotationAnimation = true;
        }

        if (m_spiderAnimation != null)
        {
            if (rotationAnimation)
            {
                m_spiderAnimation.CrossFade(ANIMATION_RUN);
            }
            else
            {
                m_spiderAnimation.CrossFade(ANIMATION_IDLE);
            }
        }
    }

    public void SetTargetPosition(Vector3 newTargetPosition)
    {
        m_memory.TargetPosition = newTargetPosition;
        m_memory.ReachedDestination = false;
    }

    void ClearTargetPosition()
    {
        m_memory.TargetPosition = Vector3.zero;
        m_memory.ReachedDestination = true;
    }

    void TakeDamage(spider attacker, float damage)
    {
        m_memory.AttackingInsect = attacker;

        health -= damage;
        Debug.Log(string.Format("{0}'s Health is: {1}", this.name, health));

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Die here
        m_spiderAnimation.CrossFade("Death");
    }

    public bool Alive
    {
        get
        {
            return (health > 0);
        }
    }

    public bool Selected
    {
        get
        {
            return m_memory.Selected;
        }
        set
        {
            m_memory.Selected = value;
            if (m_memory.Selected == true)
            {
                m_selectedGameObject = GameObject.Instantiate(SelectPrefab);
                m_selectedGameObject.transform.SetParent(this.transform, false);
            }
            else
            {
                GameObject.Destroy(m_selectedGameObject);
            }

        }
    }

    void OnTriggerEnter (Collider other)
    {
        spider testSpider = other.GetComponent<spider>();
        if (testSpider != null && !m_memory.VisibleInsects.Contains(testSpider))
        {
            Debug.Log(string.Format("{0} SPIES an insect named {1}.", this.name, testSpider.name));
            m_memory.VisibleInsects.Add(testSpider);
            m_memory.VisibleInsects.ForEach(DebugTrigger);
        }
    }

    void OnTriggerStay (Collider other)
    {
        spider testSpider = other.GetComponent<spider>();
        if (testSpider != null)
        {
            Debug.DrawLine(this.transform.position, testSpider.transform.position, Color.red);
        }
    }

    void OnTriggerExit (Collider other)
    {
        spider testSpider = other.GetComponent<spider>();
        if (testSpider != null && m_memory.VisibleInsects.Contains(testSpider))
        {
            Debug.Log(string.Format("{0} LOST sight of insect named {1}.", this.name, testSpider.name));
            m_memory.VisibleInsects.Remove(testSpider);
            m_memory.VisibleInsects.ForEach(DebugTrigger);
        }
    }

    private static void DebugTrigger(spider s)
    {
 //       Debug.Log(s.name);
    }

}
