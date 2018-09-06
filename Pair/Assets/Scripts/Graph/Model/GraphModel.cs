using System.Collections.Generic;
using System;

[Serializable]
public class GraphModel
{
	public List<NodeModel> nodes = new List<NodeModel>();

	public int startNodeID = -1;

	public int lastNodeID;

	public CameraViewModel camera = new CameraViewModel();
}