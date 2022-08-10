﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnlightedWorkService.Models
{
	
	[XmlRoot(ElementName = "fixture")]
	public class Fixture
	{

		[XmlElement(ElementName = "id")]
		public int Id { get; set; }

		[XmlElement(ElementName = "name")]
		public string Name { get; set; }

		[XmlElement(ElementName = "xaxis")]
		public int Xaxis { get; set; }

		[XmlElement(ElementName = "yaxis")]
		public int Yaxis { get; set; }

		[XmlElement(ElementName = "groupId")]
		public int GroupId { get; set; }

		[XmlElement(ElementName = "macAddress")]
		public string MacAddress { get; set; }

		[XmlElement(ElementName = "class")]
		public string Class { get; set; }
	}

	[XmlRoot(ElementName = "fixtures")]
	public class Fixtures
	{

		[XmlElement(ElementName = "fixture")]
		public List<Fixture> Fixture { get; set; }
	}
}
