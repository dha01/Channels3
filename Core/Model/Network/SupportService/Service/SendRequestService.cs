using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Data.DataModel;
using Core.Model.Network.DataModel;
using System.Net.Http;
using Core.Model.Methods.Base.DomainModel;
using Core.Model.Methods.CSharp.DomainModel;
using Newtonsoft.Json;

namespace Core.Model.Network.Service
{
	/*public class SendRequestService : ISendRequestService
	{

		public bool SendData(NodeInfo receive_node, DataInvoke data_invoke)
		{
			return true;
		}

		public AssemblyFile GetAssemblyFile(NodeInfo receive_node, Guid assembly_file_id)
		{
			using (var client = new HttpClient())
			{
				var content = new StringContent(JsonConvert.SerializeObject(assembly_file_id));

				var response = client.PostAsync(string.Format("http://{0}:{1}/Default/GetAssemblyFile", receive_node.URL, receive_node.Port), content);

				var responseString = response.Result.Content.ReadAsStringAsync().Result;

				return JsonConvert.DeserializeObject<AssemblyFile>(responseString);
			}
		}

		public DataInvoke GetData(NodeInfo receive_node, Guid guid)
		{
			Console.WriteLine("request GetData({0})", guid);
			using (var client = new HttpClient())
			{
				var content = new StringContent(JsonConvert.SerializeObject(guid));

				var response = client.PostAsync(string.Format("http://{0}:{1}/Default/GetData", receive_node.URL, receive_node.Port), content);

				var responseString = response.Result.Content.ReadAsStringAsync().Result;

				return JsonConvert.DeserializeObject<DataInvoke>(responseString);
			}
		}

		public bool AddData(NodeInfo receive_node, DataInvoke data_invoke)
		{

			Console.WriteLine("request AddData({0})", data_invoke.Id);
			using (var client = new HttpClient())
			{
				var content = new StringContent(JsonConvert.SerializeObject(data_invoke));

				var response = client.PostAsync(string.Format("http://{0}:{1}/Default/AddData", receive_node.URL, receive_node.Port), content);

				var responseString = response.Result.Content.ReadAsStringAsync().Result;

				return JsonConvert.DeserializeObject<bool>(responseString);
			}
		}
	}*/
}
