using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDriver : MonoBehaviour
{

    [SerializeField] private float unitMoveSpeed;
    [SerializeField] private float walkingPositionMoveBuffer = 0.01f;
    [SerializeField] private float finalDestinationPositionBuffer = 1;

    public int unitID;

    private List<Vector3> moveWayPoints;

    private Vector3 currentNodePosition;

    private Vector3 movePosition;

    private bool attacking;

    private bool isAttacking_Anim;
    private bool isMoving_Anim;

    private GameObject enemyBeingAttacked;

    private Object_Info object_Info;

    private void Awake()
    {

        currentNodePosition = Vector3.zero;
        moveWayPoints = new List<Vector3>();
        enemyBeingAttacked = null;

        movePosition = Constants.current.rayCastMiss;

        attacking = false;

        isMoving_Anim = false;
        isAttacking_Anim = false;

        unitID = this.GetComponent<CapsuleCollider>().GetInstanceID();

        //Debug.Log(unitID);

        this.GetComponent<Rigidbody>().freezeRotation = true;

        object_Info = GetComponent<Object_Info>();

    }

    private void Start()
    {
        GameEvents.current.OnMoveUnit += GetUnitWaypoints;
        GameEvents.current.OnMoveUnitToUnit += MoveUnitToUnit;
    }

    // Update is called once per frame
    void Update()
    {
        SendPositionToNodeNetwork();
        if (attacking == true && enemyBeingAttacked != null)
        {
            MoveToAttack(enemyBeingAttacked);
        }
        else
        {
            if (movePosition != Constants.current.rayCastMiss)
            {
                
                GetUnitWaypoints(unitID, movePosition);

            }
            else
            {
                isMoving_Anim = false;
            }
        }

        if (transform.position.y < 0)
        {
            Vector3 newPosition = NodeNetwork.current.FindNearestGridPointToPosition(transform.position);
            newPosition.y += this.GetComponent<Collider>().bounds.size.y/2;
            transform.position = newPosition;
        }



        if (GetComponent<Animator>())
        {
            GetComponent<Animator>().SetBool("isWalking", isMoving_Anim);
            GetComponent<Animator>().SetBool("isAttacking", isAttacking_Anim);
        }

        
    }

    private void SendPositionToNodeNetwork()
    {
        Vector3 newNodePosition = NodeNetwork.current.AddUnitToNodeGraph(transform.position, currentNodePosition);
        if (newNodePosition != Constants.current.rayCastMiss)
        {
            currentNodePosition = newNodePosition;
        }

    }

    private void GetUnitWaypoints(int unitID, Vector3 moveToLocation)
    {

        if (this.unitID != unitID)
        {
            return;
        }

        if (moveWayPoints.Count == 1)
        {
            MoveUnit();
            return;
        }
        else if (moveWayPoints.Count == 0)
        {
            movePosition = Constants.current.rayCastMiss;
        }

        attacking = false;


        movePosition = moveToLocation;

        //Change

        List<Vector3> locationWaypoints = new();
        //locationWaypoints.Add(moveToLocation);

        locationWaypoints.AddRange(NodeNetwork.current.GetPath(this.transform.position, moveToLocation));
        //end change

        moveWayPoints.Clear();

        moveWayPoints.AddRange(locationWaypoints);

        MoveUnit();
    }


    private void MoveUnit()
    {

        if (moveWayPoints.Count == 0)
        {
            isMoving_Anim = false;
            return;
        }
        else
        {
            isMoving_Anim = true;
        }

        float unitY = 1;

        Vector3 firstWaypoint = moveWayPoints[0];
        firstWaypoint.y = unitY;

        Vector3 differenceInWaypointToCurrentPosition = this.transform.position - firstWaypoint;

        differenceInWaypointToCurrentPosition.y = 0;

        if (differenceInWaypointToCurrentPosition.magnitude < walkingPositionMoveBuffer)
        {
            moveWayPoints.Remove(moveWayPoints[0]);

            firstWaypoint = moveWayPoints[0];
            firstWaypoint.y = unitY;

        }
        else if (moveWayPoints.Count == 1)
        {
            if (differenceInWaypointToCurrentPosition.magnitude < finalDestinationPositionBuffer)
            {
                moveWayPoints.Remove(moveWayPoints[0]);


                if (moveWayPoints.Count < 1)
                {
                    return;
                }

                firstWaypoint = moveWayPoints[0];
                firstWaypoint.y = unitY;

            }
        }

        

        
        Vector3 currentPosition = this.transform.position;


        Vector3 moveVector = firstWaypoint - currentPosition;

        moveVector.y = 0;
        //moveVector.Normalize();

        moveVector = moveVector.normalized;


        if (moveVector.sqrMagnitude > 1)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(moveVector.x, 0, moveVector.z), Vector3.up);
            transform.rotation = lookRotation;
        }

        moveVector *= unitMoveSpeed * Time.deltaTime;

        //moveVector = Vector3.ClampMagnitude(moveVector, unitMoveSpeed);


        moveVector += currentPosition;

        //moveVector = Vector3.ClampMagnitude(moveVector, unitMoveSpeed);

        moveVector.y = transform.position.y;

        this.transform.position = moveVector;
        //this.transform.position = moveVector;

        //Debug.Log("Current Position: " + currentPosition + " Waypoint: " + firstWaypoint + " MoveVector: " + moveVector + " Magnitude: " + moveVector.magnitude);

    }

    public void BuildBuilding(Vector3 moveLocation)
    {
        GetUnitWaypoints(unitID, moveLocation);
    }


    private void MoveUnitToUnit(int unitID, GameObject unitGameObject)
    {
        if (this.unitID != unitID)
        {
            return;
        }

        if (object_Info.Team != unitGameObject.GetComponent<Object_Info>().Team)
        {
            MoveToAttack(unitGameObject);
            
        }
        else
        {
            Constants.GameObjectType unitGameObjectType = unitGameObject.GetComponent<Object_Info>().ObjectType;

            if (unitGameObjectType == Constants.GameObjectType.building)
            {
                Debug.Log("Move unit to building" + unitGameObject);
                List<Vector3> locationsAroundBuilding = NodeNetwork.current.NodesAroundBuilding(unitGameObject.transform.position, unitGameObject.GetComponent<Object_Info_Buildings>().BuildingSize);

                Vector3 moveLocation = Vector3.zero;
                float distanceToLocation = int.MaxValue;

                foreach (Vector3 location in locationsAroundBuilding)
                {
                    float distanceFromLocation = Vector3.Distance(transform.position, location);
                    if (distanceFromLocation < distanceToLocation)
                    {
                        distanceToLocation = distanceFromLocation;
                        moveLocation = location;
                    }
                }

                GetUnitWaypoints(unitID, moveLocation);
            }

        }


    }

    private void MoveToAttack(GameObject enemyUnit)
    {
        attacking = true;

        if (enemyUnit == null)
        {
            attacking = false;
        }

        enemyBeingAttacked = enemyUnit;

        GetWayPointToUnit(enemyUnit);

        float distanceBetweenSelfEnemy = Vector3.Distance(transform.position, enemyUnit.transform.position);

        if (distanceBetweenSelfEnemy <= object_Info.AttackRange)
        {
            isMoving_Anim = false;
            Attack(enemyUnit);
        }
        else
        {
            isMoving_Anim = true;
            MoveUnit();
        }

    }

    private void GetWayPointToUnit(GameObject enemyUnit)
    {

        if (enemyUnit == null)
        {
            return;
        }

        //Change

        List<Vector3> locationWaypoints = new();
        //locationWaypoints.Add(moveToLocation);

        locationWaypoints.AddRange(NodeNetwork.current.GetPath(this.transform.position, enemyUnit.transform.position));
        //end change

        moveWayPoints.Clear();

        moveWayPoints.AddRange(locationWaypoints);

    }


    bool freeToAttack = true;
    private void Attack(GameObject enemyUnit)
    {
        if (freeToAttack == true && enemyUnit != null)
        {
            isAttacking_Anim = true;
            Debug.Log("Attacks :" + enemyUnit.name);
            GameEvents_Attacking.current.AttackUnitTrigger(enemyUnit.GetComponent<Object_Info>().Object_ID, object_Info.AttackDamage);
            StartCoroutine(AttackCooldown(object_Info.AttackSpeed));
        }
        else
        {
            isAttacking_Anim = false;
        }
    }

    private IEnumerator AttackCooldown(float time)
    {
        Debug.Log("Waiting to attack");
        freeToAttack = false;
        yield return new WaitForSeconds(time);
        freeToAttack = true;
        Debug.Log("Ready To Attack");
    }
}
