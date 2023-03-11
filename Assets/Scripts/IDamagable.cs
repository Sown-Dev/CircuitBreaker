using UnityEngine;

public interface IDamagable{
    public void takeDamage(int dmg, Vector3 hitpoint, bool tazer, float stun, int owner);
    
}