using UnityEngine;

[CreateAssetMenu(fileName = "New Save Channel", menuName = "SO Channels/Save")]
public class SaveChannelSO : ScriptableObject
{
	public SaveData DefaultValues;
}

public struct SaveData
{
	public int Level;
	public int Coin;
}
