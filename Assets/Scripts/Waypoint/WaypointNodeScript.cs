using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public enum Direction
{
    North = 0,
    East,
    South,
    West,

    Total
}

[System.Serializable]
public class WaypointNodeData
{
    public WaypointNodeScript[] directionalNodes;


	public WaypointNodeScript rightNode(Direction fromDir)
	{
		return rightNode((int) fromDir);
	}

	public WaypointNodeScript rightNode(int fromDir)
	{
		int right = (fromDir + 1) % (int)Direction.Total;
		return directionalNodes[right];
	}

	public WaypointNodeScript forwardNode(Direction fromDir)
	{
		return forwardNode((int) fromDir);
	}

	public WaypointNodeScript forwardNode(int fromDir)
	{
		int forward = (fromDir) % (int)Direction.Total;
		return directionalNodes[forward];
	}

	public WaypointNodeScript leftNode(Direction fromDir)
	{
		return leftNode((int) fromDir);
	}

	public WaypointNodeScript leftNode(int fromDir)
	{
		int left = (fromDir + 3) % (int)Direction.Total;
		return directionalNodes[left];
	}

	public WaypointNodeData()
	{
        directionalNodes = new WaypointNodeScript[(int)(Direction.Total)];
	}
}

[System.Serializable]
public class WaypointNodeScript : MonoBehaviour
{
    [SerializeField]
    int instanceID = 0;

    public WaypointNodeData data;
	public WaypointNodeData prevData;

	//By KK for distance calculation
	public int value;
	public WaypointNodeScript nextNode;
	public List<WaypointNodeScript> connectedNodeList;

	void Awake()
	{
		value = GetComponent<WaypointValue>().value;

		for(int i = 0; i < GetComponent<WaypointValue>().connectedNodeList.Count; i++)
		{
			connectedNodeList.Add(GetComponent<WaypointValue>().connectedNodeList[i]);
		}
	}

    void OnValidate()
    {
        UpdateNodes();

        // If the object is copied, soft-reset the node
        if (instanceID == 0)
        {
            instanceID = GetInstanceID();
        }
        else if (instanceID != GetInstanceID() && GetInstanceID() < 0)
        {
            instanceID = GetInstanceID();
            ResetNodes(true);
            Debug.LogWarning("Resetting " + gameObject.name + " as it's copied.");
        }
    }

    [ContextMenu("Reset Nodes")]
    void ResetNodes()
    {
        ResetNodes(false);
        Debug.LogWarning("Resetting " + gameObject.name + " with context menu.");
    }

    void ResetNodes(bool softReset)
    {
        Debug.LogWarning("Resetting " + gameObject.name + ".");
        for (int i = 0; i < (int)Direction.Total; i++)
		{
			if(!softReset)
			{
				if(data.directionalNodes[i])
				{
					data.directionalNodes[i].data.directionalNodes[GetOppositeDir(i)] = null;
					data.directionalNodes[i].BackupPreviousData();
				}
			}

			data.directionalNodes[i] = null;
		}
		BackupPreviousData();
	}

	void UpdateNodes()
	{
		for(int i = 0; i < (int)Direction.Total; i++)
		{
			if(data.directionalNodes[i] != prevData.directionalNodes[i])
			{
				if(data.directionalNodes[i])
				{
					// Deny node assignment when assigning this node to itself
					if(data.directionalNodes[i] == this)
                    {
                        Debug.LogError("Unable to assign " + gameObject.name + " to itself.");
                        data.directionalNodes[i] = prevData.directionalNodes[i];
                    }
					else
					{
						bool isDuplicated = false;
						for(int j = 0; j < (int)Direction.Total; j++)
						{
							// Deny node assignment when it is used in other directions of this node
							if(i != j && data.directionalNodes[i] == data.directionalNodes[j])
                            {
                                Debug.LogError(gameObject.name + " has already existed in " + ((Direction)j).ToString() + " node.");
                                data.directionalNodes[i] = prevData.directionalNodes[i];
								isDuplicated = true;
								break;
							}
						}

						if(!isDuplicated)
						{
							// Deny node assignment when the slot is occupied
							if(data.directionalNodes[i].data.directionalNodes[GetOppositeDir(i)])
                            {
                                Debug.LogError(data.directionalNodes[i].gameObject.name + "'s " + ((Direction)GetOppositeDir(i)).ToString() + " node has been occupied.");
                                data.directionalNodes[i] = prevData.directionalNodes[i];
							}
							else
                            {
                                data.directionalNodes[i].data.directionalNodes[GetOppositeDir(i)] = this;
                                data.directionalNodes[i].BackupPreviousData();

								#if UNITY_EDITOR
								// Set the object dirty to make sure it's updated
								EditorUtility.SetDirty(data.directionalNodes[i]);
								#endif
                            }
						}
					}
				}
                
                if (prevData.directionalNodes[i])
                {
                    prevData.directionalNodes[i].data.directionalNodes[GetOppositeDir(i)] = null;
					prevData.directionalNodes[i].BackupPreviousData();

					#if UNITY_EDITOR
					// Set the object dirty to make sure it's updated
					EditorUtility.SetDirty(prevData.directionalNodes[i]);
					#endif
                }
            }
		}
		BackupPreviousData();
	}

	int GetOppositeDir(int i)
	{
		return (i + 2) % (int)Direction.Total;
	}

	public void BackupPreviousData()
	{
		for(int i = 0; i < (int)Direction.Total; i++)
		{
			prevData.directionalNodes[i] = data.directionalNodes[i];
		}
	}

	// Only In Play Mode
	void OnTriggerEnter(Collider other)
	{
		if(Application.isPlaying)
		{
			if(other.GetComponent<PlayerCoreController>())
			{
				WaypointManagerScript.Instance.RegisterNode(this);;
//				WaypointDistanceScript.Instance.ResetNodes(this);
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(Application.isPlaying)
		{
			if(other.GetComponent<PlayerCoreController>())
			{
				WaypointManagerScript.Instance.UnregisterNode(this);
			}
		}
	}
}
