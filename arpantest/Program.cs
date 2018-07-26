using System;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Runtime.Serialization;
namespace arpantest
{
	[DataContract]
	internal class SampleData
	{
		[DataMember]
		public string Prop1 { get; set; }
		[DataMember]
		public string Prop2 { get; set; }
	}


	[ServiceContract]
	internal interface IPingPong
	{
		[OperationContract]
		[WebGet(UriTemplate = "/{ping}", RequestFormat = WebMessageFormat.Json,
				ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
		SampleData ping(string ping);

		[OperationContract]
        [WebGet(UriTemplate = "/{pingstr}/all", RequestFormat = WebMessageFormat.Json,
                ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
		SampleData[] pingall(string pingstr);

		[OperationContract]
		[WebInvoke(Method = "DELETE",
			UriTemplate = "/{ping}", RequestFormat = WebMessageFormat.Json,
				ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
		string pingpongdelete(string ping);


		[OperationContract]
		[WebInvoke(Method = "POST",
			UriTemplate = "/ping/pong", RequestFormat = WebMessageFormat.Json,
				ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
		SampleData pingpong(SampleData ping);

		[OperationContract]
		[WebInvoke(Method = "PUT",
			UriTemplate = "/ping/pong", RequestFormat = WebMessageFormat.Json,
				ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
		SampleData pingpong2(SampleData ping);
	}

	class PingPongServer : IPingPong
	{
		public SampleData ping(string ping)
		{
			Console.WriteLine("got {0}", ping);
			return new SampleData()
			{
				Prop1 = "1",
				Prop2 = "2"
			};
		}

		public SampleData[] pingall(string pingstr)
		{
			Console.WriteLine("got {0}", pingstr);
			return new SampleData[2] { new SampleData()
			{
				Prop1 = "1",
				Prop2 = "2"
				}, new SampleData()
			{
				Prop1 = "3",
				Prop2 = "4"
			}
			};
		}

		public SampleData pingpong(SampleData ping)
		{
		    Console.WriteLine(ping.Prop1);
			return new SampleData()
			{
				Prop1 = "hello",
				Prop2 = "Mello"
			};
		}

		public SampleData pingpong2(SampleData ping)
		{
			Console.WriteLine(ping.Prop1);
			return new SampleData()
			{
				Prop1 = "hello",
				Prop2 = "Mello"
			};
		}

		public string pingpongdelete(string ping)
		{
			Console.Write(ping);
			return "delete hello";
		}
	}

	class WebServer
	{
		public void Start()
		{
			Uri[] baseUri = new Uri[1] { new Uri("http://localhost:2021/pingserver") };
			WebServiceHost host = new WebServiceHost(typeof(PingPongServer), baseUri);
			host.AddServiceEndpoint(typeof(IPingPong), new WebHttpBinding(), "");
			host.Open();
		}
	}

	class MainClass
	{
		public static void Main(string[] args)
		{
			new WebServer().Start();
			Console.WriteLine("server started");
            Console.ReadKey();
		}
	}
}
