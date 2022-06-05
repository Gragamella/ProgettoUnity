using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class EnemyAttPicker : Enemy
{

    public GameObject enemyObj;
    public Enemy enemy;
    public List<Sprite> spritesMorte;

    public EnemyAttPicker(GameObject _enemyObject)
    {
        enemyObj = _enemyObject;
        enemy = enemyObj.GetComponent<Enemy>();
        spritesMorte = new List<Sprite>();
    }

    public void assignEnemyAtt()
    {
        switch (enemyObj.name)
        {
            default:
            case "Red-skelly":
                spritesMorte.Add(enemy.atlas.GetSprite("skelly-morte-0"));
                spritesMorte.Add(enemy.atlas.GetSprite("skelly-morte-1"));
                spritesMorte.Add(enemy.atlas.GetSprite("skelly-morte-2"));
                enemy.enemyAnimationDict["enemy_idle"] = "Red-Skelly-Idle";
                enemy.enemyAnimationDict["enemy_walk"] = "Red-Skelly-walk";
                enemy.enemyAnimationDict["enemy_attack"] = "Red-Skelly-Attack";
                enemy.forza = 2;
                enemy.costituzione = 3;
                enemy.destrezza = 3;
                enemy.fortuna = 2;
                enemy.stoppingDistance = 0.2f;
                enemy.jumpTimer = 2f;
                calcuteEnemyAtt(enemy.forza, enemy.destrezza, enemy.costituzione, enemy.fortuna);
                break;
            case "Blue-skelly":
                spritesMorte.Add(enemy.atlas.GetSprite("skelly-morte-3"));
                spritesMorte.Add(enemy.atlas.GetSprite("skelly-morte-4"));
                spritesMorte.Add(enemy.atlas.GetSprite("skelly-morte-5"));
                enemy.enemyAnimationDict["enemy_idle"] = "Blue-Skelly-Idle";
                enemy.enemyAnimationDict["enemy_walk"] = "Blue-Skelly-walk";
                enemy.enemyAnimationDict["enemy_attack"] = "Blue-Skelly-Attack";
                enemy.forza = 4;
                enemy.costituzione = 5;
                enemy.destrezza = 1;
                enemy.fortuna = 2;
                enemy.stoppingDistance = 0.3f;
                enemy.jumpTimer = 2f;
                calcuteEnemyAtt(enemy.forza, enemy.destrezza, enemy.costituzione, enemy.fortuna);
                break;
        }
    }

    private void calcuteEnemyAtt(float forza, float dex, float costituzione, float fortuna)
    {
        enemy.healt = costituzione * 10;
        enemy.speed = dex * 0.3f;
        enemy.jump = (forza + dex / 2) * 5;     
        enemy.force = forza;
    }

}
