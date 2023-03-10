using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDesc", menuName = "UpgradeDesc", order = 0)]
public class UpgradeDescription : ScriptableObject{
    public string name;
    public Sprite icon;
    public string description;
    public int ID;

}