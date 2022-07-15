using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDamageHandler : MonoBehaviour
{
    StatsHandler statsHandler;

    GameObject attackSprite;

    //weapon dist = 1.62
    private void Start()
    {
        statsHandler = gameObject.GetComponent<StatsHandler> ();
    }

    bool isAttacking = false;
    public void Attack()
    {
        if (isAttacking)
        {
            return;
        }
        isAttacking = true;
        Sprite weaponTex = statsHandler.MainWeapon.WeaponTexture;
        attackSprite = new GameObject("Player Attack");
        SpriteRenderer temp = attackSprite.AddComponent<SpriteRenderer>();
        temp.sprite = weaponTex;

        StartCoroutine(WeaponAttack());
    }

    IEnumerator WeaponAttack()
    {
        float Angle = UtilClass.GetAngleBetweenVector2(transform.position, UtilClass.getMouseWorldPosition()) + statsHandler.MainWeapon.AttackLeftArcLimit;

        if (statsHandler.MainWeapon.WeaponAttackType.Equals(Weapon.AttackType.Slash))
        {
            float changeAngleBy = (statsHandler.MainWeapon.AttackRightArcLimit + statsHandler.MainWeapon.AttackLeftArcLimit) / statsHandler.MainWeapon.AttackTime;
            //Debug.Log(changeAngleBy);

            float CurrentTotalAngleMoved = 0;
            while (CurrentTotalAngleMoved <= statsHandler.MainWeapon.AttackRightArcLimit + statsHandler.MainWeapon.AttackLeftArcLimit)
            {
                Quaternion WHY = new Quaternion();
                WHY.eulerAngles = new Vector3(0, 0, Angle);
                attackSprite.transform.rotation = WHY;

                Angle -= changeAngleBy * Time.deltaTime;
                CurrentTotalAngleMoved += Mathf.Abs(changeAngleBy) * Time.deltaTime;
                Vector2 tempPos;
                tempPos = UtilClass.GetVector2FromAngle(Angle).normalized * statsHandler.MainWeapon.AttackRange;
                Vector3 newPos = new Vector3(transform.position.x + tempPos.x, transform.position.y + tempPos.y, -2);
                attackSprite.transform.position = newPos;
                handleHits(UtilClass.GetVector2FromAngle(Angle));
                yield return null;
            }
        }

        Destroy(attackSprite);
        attackSprite = null;
        isAttacking = false;
        alreadyHit.Clear();
        yield break;
    }

    List<GameObject> alreadyHit = new List<GameObject>();
    void handleHits(Vector2 Angle)
    {
        //Debug.DrawRay(transform.position, Angle, Color.red, 1);
        Vector2 why = transform.position;
        RaycastHit2D[] hits;
        hits = Physics2D.RaycastAll(transform.position, Angle.normalized, Vector2.Distance((why + Angle), transform.position));

        foreach (RaycastHit2D rayHit in hits)
        {
            CombatDamageHandler HitTemp = rayHit.rigidbody.gameObject.GetComponent<CombatDamageHandler>();
            if (HitTemp != null && !rayHit.rigidbody.gameObject.Equals(gameObject))
            {
                if (alreadyHit.Contains(HitTemp.gameObject))
                {
                    continue;
                }
                else
                {
                    alreadyHit.Add(HitTemp.gameObject);
                    HitTemp.HandleHit(getHitDamage());
                    Debug.Log("HIT");
                }
            }
        }
    }

    public void HandleHit(int IncomingDamage)
    {
        IncomingDamage = Mathf.Clamp(IncomingDamage - (statsHandler.getIncomingDamageReduciton()/2), 0, 100);
        statsHandler.HandleDamageTaken(IncomingDamage);
    }

    int getHitDamage()
    {
        return statsHandler.MainWeapon.AttackDamage + statsHandler.getOutgoingDamageIncrease();
    }



}
