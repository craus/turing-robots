﻿using System.Collections.Generic;
using System;

[Serializable]
public class GraphModel
{
	public List<NodeModel> nodes = new List<NodeModel>();

	public CameraViewModel camera = new CameraViewModel();
}